using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class Structure : ShipComponent
{
    public int MaxHp;
    public int hp;
    public Animator Anim;

    private List<Shield> nearbyShields;
    private List<Module> currentModules = new List<Module>();
    private List<Structure> currentStructures = new List<Structure>();

    public void Start()
    {
        // Get all the child modules and structures
        currentModules.Clear();
        currentStructures.Clear();
        for (var index = 0; index < transform.childCount; ++index)
        {
            GameObject child = transform.GetChild(index).gameObject;
            if (child.GetComponent<Module>() != null)
            {
                currentModules.Add(child.GetComponent<Module>());

                // Increase maxHP for each armor.
                if (child.GetComponent<Armor>() != null)
                {
                    IncreaseMaxHp(child.GetComponent<Armor>().ExtraHp);
                }
            }
            else if (child.GetComponent<Structure>() != null)
            {
                currentStructures.Add(child.GetComponent<Structure>());
            }
        }
        // Set the HP to the possibly new maxHP.
        hp = MaxHp;
    }

    public void TakeDamage(int dmg, Core originator)
    {
        bool hasShieldAttached;
        bool isProtected = IsProtected(out hasShieldAttached);

        if (!isProtected)
        {
            // If the structure has shields attached (which aren't active), disable one of them.
            if (hasShieldAttached)
            {
                // Find the shield and disable it.
                foreach (Shield s in currentModules.Select(m => m.gameObject.GetComponent<Shield>()).Where(s => s != null))
                {
                    s.DisableShield();
                    break;
                }
            }
            // Lose HP, because ship was damaged.
            hp -= dmg;
            // Trigger damage animation.
            TriggerAnimation("TriggerDamage");
            // If the structure has any modules, lose a module.
            LoseComponentOrSelf(originator);
        }

    }


    /// <summary>
    /// Runs an animation of this object, if exists.
    /// </summary>
    /// <param name="animation">The animation name: (TriggerDamage)</param>
    public void TriggerAnimation(string animation)
    {
        // Trigger visual feedback and return true
        Anim.SetTrigger(animation);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Only "crash" if the force with which the objects hit each other is large enough.
        if (coll.relativeVelocity.magnitude > GlobalValues.MinCrashMagnitude)
        {
            // If the other object has a higher mass, lose one random module.
            if (coll.rigidbody.mass >= ShipCore.GetComponent<Rigidbody2D>().mass)
            {
                Core core = null;
                if (coll.gameObject.GetComponent<Structure>() != null)
                {
                    core = coll.gameObject.GetComponent<Structure>().ShipCore.GetComponent<Core>();
                }
                TakeDamage(GlobalValues.CrashDamage, core);
            }

        }
    }

    private void LoseComponentOrSelf(Core originator)
    {
        // As long as the hp is lower or equal than zero lose module or structure.
        while (hp <= 0)
        {
            // If the structure has at least one module, lose one.
            if (currentModules.Count > 0)
            {
                if (originator != null && originator.gameObject.GetInstanceID() != ShipCore.GetInstanceID())
                {
                    originator.ModulesDestroyed++; // Add a point to the ship that destroyed this module.
                }
                if (RemoveDefensiveModule())
                {
                    // Reset Hp, but apply excess damage.
                    hp = MaxHp - Mathf.Abs(hp);
                    continue;
                }
                // If there are no defensive modules, lose a random module.    
                RemoveRandomModule();
                // Reset Hp, but apply excess damage.
                hp = MaxHp - Mathf.Abs(hp);
            }
            // If no more modules are left, but it has child structures, damage one of those.
            else if (currentStructures.Count > 0)
            {
                var rnd = Random.Range(0, currentStructures.Count); // Find random index.
                var structure = currentStructures[rnd]; // Get structure at random index.
                structure.TakeDamage(Mathf.Abs(hp), originator); // Damage the next structure with the leftover damage.
                return; // Return without adding new HP to the structure. This means it's next hit will be fatal (again).
            }
            else
            {
                // If there are no child modules or structures, destroy this structure
                var parent = transform.parent; // Find parent.
                if (parent != null && parent.GetComponent<Structure>() != null)
                // If parent is structure, remove this from parent's list of structures.
                {
                    parent.GetComponent<Structure>().RemoveFromStructureList(this);
                }
                else
                {
                    var core = ShipCore.GetComponent<Core>();
                    core.DestroyShip();
                    return;
                }

                Destroy(gameObject);
                return;
            }
        }
    }

    private bool RemoveDefensiveModule()
    {
        foreach (Module m in currentModules.Where(m => m.gameObject.GetComponent<Armor>() != null))
        {
            currentModules.Remove(m);
            IncreaseMaxHp(-m.GetComponent<Armor>().ExtraHp);
            Destroy(m.gameObject);
            return true;
        }
        return false;
    }

    private void RemoveRandomModule()
    {
        var rnd = Random.Range(0, currentModules.Count); // Find random index.
        var module = currentModules[rnd]; // Get module at random index.
        currentModules.Remove(module); // Remove module from module list.
        Destroy(module.gameObject); // Destroy the game object.
    }

    public void IncreaseMaxHp(int inc)
    {
        MaxHp += inc;
        hp = MaxHp;
    }

    public bool IsProtected(out bool hasShieldAttached)
    {
        hasShieldAttached = false;
        // Find active shields in this ship near this structure
        for (var i = nearbyShields.Count - 1; i >= 0; i--)
        {
            if (nearbyShields[i] == null)
            {
                nearbyShields.RemoveAt(i);
                continue;
            }
            if (nearbyShields[i].ShipCore.GetInstanceID() == ShipCore.GetInstanceID() &&
               nearbyShields[i].Active && Vector3.Distance(transform.position, nearbyShields[i].transform.position) < nearbyShields[i].Radius)
            {
                // If the shield is attached to this structure, save it in the attachedShield values
                if (nearbyShields[i].transform.parent.GetInstanceID() == transform.GetInstanceID())
                {
                    hasShieldAttached = true;
                }
                return true;
            }
        }
        return false;
    }

    public void FindNearbyShipShields()
    {
        // Find all shield modules in the scene.
        Shield[] shields = (Shield[])FindObjectsOfType(typeof(Shield));
        // Find active shields in this ship near this structure.
        nearbyShields = shields.Where(t => t.ShipCore.GetInstanceID() == ShipCore.GetInstanceID() && Vector3.Distance(transform.position, t.transform.position) < t.Radius).ToList();
    }

    public void RemoveFromStructureList(Structure s)
    {
        currentStructures.Remove(s);
    }

    protected void OnEnable()
    {
        FindNearbyShipShields();

        // Rework lists of modules and structures.
        //MaxHp = originalMaxHp; // Reset maxHP before adding it up again.
        // Get all the child modules and structures
        currentModules.Clear();
        currentStructures.Clear();
        for (var index = 0; index < transform.childCount; ++index)
        {
            GameObject child = transform.GetChild(index).gameObject;
            if (child.GetComponent<Module>() != null)
            {
                currentModules.Add(child.GetComponent<Module>());

                // Increase maxHP for each armor.
                if (child.GetComponent<Armor>() != null)
                {
                    IncreaseMaxHp(child.GetComponent<Armor>().ExtraHp);
                }
            }
            else if (child.GetComponent<Structure>() != null)
            {
                currentStructures.Add(child.GetComponent<Structure>());
            }
        }
    }

    public IEnumerator DisplayCannotRemove()
    {
        if (transform.childCount > 0)
        {
            TriggerAnimation("TriggerCannotRemove");
            yield return new WaitForSeconds(0.08f);
            for (var index = 0; index < transform.childCount; ++index)
            {
                GameObject child = transform.GetChild(index).gameObject;
                if (child.GetComponent<Structure>() != null)
                {
                    StartCoroutine(child.GetComponent<Structure>().DisplayCannotRemove());
                }
            }
        }
        else
        {
            TriggerAnimation("TriggerCannotRemoveLast");
            yield return new WaitForSeconds(1f);
            TriggerAnimation("TriggerCannotRemoveLast");
        }
    }

}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Structure), true)]
public class StructureEditor : ShipComponentEditor
{

    protected new void DrawCustomInspector()
    {
        // Create heading.
        GUIStyle heading = new GUIStyle { fontSize = 14 };
        EditorGUILayout.LabelField("Structure settings", heading);

        // Get target and show/edit fields.
        Structure t = (Structure)target;
        t.MaxHp = EditorGUILayout.IntField("Hp", t.MaxHp);

        // If the target was changed, set the target to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (CustomInspectorOpen)
        {
            DrawCustomInspector();
        }
    }
}
