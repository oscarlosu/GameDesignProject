using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Structure : Module
{

    // Update is called once per frame
    void Update()
    {

    }

    public void LoseModule()
    {
        // Check if shield or armor module is present and destroy before others.
        foreach (var go in Sockets)
        {
            // If current socket is empty, check the next one.
            if (go == null)
                continue;

            if (go.tag == "DefensiveModule")
            {
                if (go.GetComponent<Armor>())
                {
                    go.GetComponent<Armor>().Activate();
                }

                // If defensive module was found,
                //don't destroy random module after activating defensive module.
                return;
            }
        }

        // Destroy random module attached to this structure
        int index = Random.Range(0, Sockets.Count);
        
        if(Sockets[index] != null && Sockets[index].tag == "DestructibleModule")
        {
            // Remove from sockets list and destroy
            GameObject go = Sockets[index];
            Sockets.RemoveAt(index);
            GameObject.Destroy(go);
        }
    }

    public IList<Structure> GetAllStructureModules()
    {
        List<Structure> structureModules = new List<Structure>();
        foreach (GameObject module in Sockets)
        {
            if (module != null)
            {
                var structure = module.GetComponent<Structure>();
                if (structure != null)
                {
                    var structures = structure.GetAllStructureModules();
                    structureModules.AddRange(structures);
                }
            }
        }

        structureModules.Add(this);

        return structureModules;
    }

    // Returns true if it has sockets and there is at least one module in one of the sockets.
    public bool HasAnyModules()
    {
        foreach (GameObject module in Sockets)
        {
            if (module != null)
                return true;
        }
        return false;
    }
}
