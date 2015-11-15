using UnityEngine;
using System.Collections.Generic;

public class Structure : ShipComponent
{
    public int MaxHp = 1;
    public int hp;
    public Animator Anim;

    public void Start()
    {
        hp = MaxHp;
    }

    public bool TakeDamage(int dmg)
    {
        // Get all the child modules and structures
        List<Module> modules = new List<Module>();
        List<Structure> structures = new List<Structure>();
        for(int index = 0; index < transform.childCount; ++index)
        {
            GameObject child = transform.GetChild(index).gameObject;
            if (child.GetComponent<Module>() != null)
            {
                modules.Add(child.GetComponent<Module>());
            }
            else if(child.GetComponent<Structure>() != null)
            {
                structures.Add(child.GetComponent<Structure>());
            }
        }
        // If the structure has child modules or has no children, lose module, else propagate damage to a random child struture
        if(modules.Count > 0 || structures.Count == 0)
        {
            hp -= dmg;
            TriggerAnimation("TriggerDamage");
            LoseModule(modules);
            return true;
        }
        else
        {
            // Find a child structure that either has child modules or has no children, or has descendants with child modules or no children
            for(int index = 0; index < structures.Count; ++index)
            {
                // We choose which strcuture to try first at random
                int rnd = Random.Range(0, structures.Count);
                if(structures[rnd].TakeDamage(dmg))
                {
                    // Trigger visual feedback and return true
                    TriggerAnimation("TriggerDamage");
                    //Debug.Log("Damage visual feedback not implemented.");
                    return true;
                }
                else
                {
                    // If the child structure couldnt take the damage, remove it from the list and try the next structure
                    structures.RemoveAt(rnd);
                }
            }
            // This point should never be reached
            Debug.LogWarning("Structure couldnt propagate the damage! This should never happen!");
            return true;
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
            if (coll.rigidbody.mass >= Core.GetComponent<Rigidbody2D>().mass)
            {
                TakeDamage(GlobalValues.CrashDamage);
                Debug.LogWarning("Ship " + gameObject.name + " collided with something with greater or equal mass.");
            }

        }
    }

    private void LoseModule(List<Module> modules)
    {
        // As long as the hp is lower or equal than zero
        while(hp <= 0)
        {
            // If any child modules left on the structure, lose one
            if (modules.Count > 0)
            {
                // Defensive modules are removed first
                foreach (Module m in modules)
                {
                    if (m.gameObject.tag == GlobalValues.DefensiveModuleTag)
                    {
                        modules.Remove(m);
                        Destroy(m.gameObject);
                        return;
                    }
                }
                // If there are no defensive modules, lose a random module            
                int rnd = Random.Range(0, modules.Count);
                GameObject module = modules[rnd].gameObject;
                Destroy(module);
                // Reset Hp, but apply excess damage
                hp = MaxHp - Mathf.Abs(hp);
            }
            else
            {
                // If there are no child modules, destroy this structure
                Destroy(gameObject);
                return;
            }
        }
        
    }


    //IEnumerator HandleHitColor(GameObject obj)
    //{
    //    SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
    //    rend.color = Color.red;
    //    for (int index = 0; index < obj.transform.childCount; ++index)
    //    {
    //        GameObject child = obj.transform.GetChild(index).gameObject;
    //        if (child.GetComponent<Module>() != null)
    //        {
    //            child.GetComponent<SpriteRenderer>().color = Color.red;
    //        }
    //    }
    //    yield return new WaitForSeconds(GlobalValues.HitFeedbackDuration);
    //    rend.color = Color.white;
    //    for (int index = 0; index < obj.transform.childCount; ++index)
    //    {
    //        GameObject child = obj.transform.GetChild(index).gameObject;
    //        if (child.GetComponent<Module>() != null)
    //        {
    //            child.GetComponent<SpriteRenderer>().color = Color.white;
    //        }
    //    }
    //}

}
