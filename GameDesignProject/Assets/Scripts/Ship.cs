using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour
{

    public float MinCrashMagnitude;
    public Structure CoreStructureModule;

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Only "crash" if the force with which the objects hit each other is large enough.
        if (coll.relativeVelocity.magnitude > MinCrashMagnitude)
        {
            Debug.Log("Crashed with a magnitude of: " + coll.relativeVelocity.magnitude);
            // If the other object has a higher mass, lose one random module.
            if (coll.gameObject.GetComponent<Rigidbody2D>().mass > GetComponent<Rigidbody2D>().mass)
            {
                Debug.Log(this.gameObject + " lost a module due to collision!");
                IList<Structure> structureModules = CoreStructureModule.GetAllStructureModules();
                LoseModule(structureModules);
            }
        }

    }

    private void LoseModule(IList<Structure> structureModules)
    {
        // If any modules left on the ship, lose one.
        if (GetAllModules().Count > 0)
        {
            // Check if any defensive modules are present and remove that, if there is.
            foreach (Structure s in structureModules)
            {
                foreach (var m in s.Sockets)
                {
                    if (m.tag == "DefensiveModule")
                    {
                        s.LoseModule();
                        return;
                    }
                }
            }
            // If there are no defensive modules, pick a random structure module (with modules attached) and make it lose a module.
            while (true)
            {
                int index = Random.Range(0, structureModules.Count);
                if (structureModules[index].HasAnyModules())
                {
                    structureModules[index].LoseModule();
                    return;
                }
            }
        }
    }

    private IList<Module> GetAllModules()
    {
        IList<Module> modules = new List<Module>();
        foreach (Structure s in CoreStructureModule.GetAllStructureModules())
        {
            foreach (GameObject m in s.Sockets)
            {
                modules.Add(m.GetComponent<Module>());
            }
        }
        return modules;
    }

}
