using UnityEngine;
using System.Collections;

public class Structure : Module
{

    // Update is called once per frame
    void Update()
    {

    }

    public void LoseModule()
    {
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
}
