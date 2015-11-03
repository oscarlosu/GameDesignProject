using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour
{
    public float MinCrashMagnitude;

    private List<GameObject> modules;
    private List<GameObject> structures;

    // Public methods

    // Use this for initialization
    void Start()
    {        
        Assemble();
        // Add core  and structure mass to rigidbody
        GetComponent<Rigidbody2D>().mass = GlobalValues.CoreMass;

        print("Modules : " + modules.Count + " Structures : " + structures.Count);
    }

    // Update is called once per frame
    void Update()
    {

    }   

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Only "crash" if the force with which the objects hit each other is large enough.
        if (coll.relativeVelocity.magnitude > MinCrashMagnitude)
        {
            Debug.Log("Crashed with a magnitude of: " + coll.relativeVelocity.magnitude);
            // If the other object has a higher mass, lose one random module.
            if (coll.gameObject.GetComponent<Rigidbody2D>().mass >= GetComponent<Rigidbody2D>().mass)
            {                
                LoseModule();
                Debug.LogWarning("Ship " + gameObject.name + " collided with something with greater mass.");
            }

        }
    }
    

    public void Assemble()
    {
        // Search for components and save references in lists
        // Retrieve all children
        List<GameObject> children = RetrieveChildren(gameObject);
        // Separate into modules and structures
        modules = new List<GameObject>();
        structures = new List<GameObject>();
        foreach(GameObject child in children)
        {            
            if (child.GetComponent<Module>() != null)
            {
                modules.Add(child);
            }
            else if(child.GetComponent<Component>() != null)
            {
                structures.Add(child);
            }
            // Set itself as the ship of the component
            if(child.GetComponent<Component>() != null)
            {
                child.GetComponent<Component>().Ship = gameObject;
                child.GetComponent<Component>().enabled = true;
            }
            

        }            
    }

    // Private methods

    private void LoseModule()
    {
        // If any modules left on the ship, lose one.
        if (modules.Count > 0)
        {
            // Defensive modules are removed first
            foreach (GameObject m in modules)
            {
                if (m.tag == GlobalValues.DefensiveModuleTag)
                {
                    modules.Remove(m);
                    GameObject.Destroy(m);
                    return;
                }
            }
            // If there are no defensive modules, lose a random module            
            int index = Random.Range(0, modules.Count);
            GameObject module = modules[index];
            modules.RemoveAt(index);
            GameObject.Destroy(module);
        }
    }

    private List<GameObject> RetrieveChildren(GameObject go)
    {
        List<GameObject> myChildren = new List<GameObject>();
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            GameObject myChild = go.transform.GetChild(i).gameObject;
            // Add your own child
            myChildren.Add(myChild);
            // Add your child's children
            myChildren.AddRange(RetrieveChildren(myChild));
        }
        return myChildren;
    }
}
