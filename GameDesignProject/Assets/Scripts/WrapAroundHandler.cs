using UnityEngine;
using System.Collections;

public class WrapAroundHandler : MonoBehaviour
{
    public Vector2 TeleportOuterOffset; // Offset from the border of the world where the teleportation is triggered
    public Vector2 TeleportInnerOffset; // Offset from the border of the world where the object will be teleported to
    public Vector2 WorldSize;

    public string[] Tags;

    private Vector2 WorldBorderOffset;    

    // Use this for initialization
    void Start()
    {
        // Set border offset
        WorldBorderOffset = WorldSize / 2;
        // Scale background
        Sprite spr = GetComponent<SpriteRenderer>().sprite;
        transform.localScale = new Vector3(transform.localScale.x  * WorldSize.x / spr.bounds.size.x, transform.localScale.y * WorldSize.y / spr.bounds.size.y, transform.localScale.z);
        // Debugging
        Debug.Log(WorldBorderOffset);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] goList;
        foreach(string tag in Tags)
        {
            goList = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject go in goList)
            {
                WrapAround(go.transform);
            }
        }
    }

    void WrapAround(Transform t)
    {
        
        // X axis
        if (t.position.x > WorldBorderOffset.x + TeleportOuterOffset.x)
        {
            t.position = new Vector3(-WorldBorderOffset.x - TeleportInnerOffset.x, t.position.y, t.position.z);
        }
        if (t.position.x < - WorldBorderOffset.x - TeleportOuterOffset.x)
        {
            t.position = new Vector3(WorldBorderOffset.x + TeleportInnerOffset.x, t.position.y, t.position.z);
        }
        // Y axis
        if (t.position.y > WorldBorderOffset.y + TeleportOuterOffset.y)
        {
            t.position = new Vector3(t.position.x, - WorldBorderOffset.y - TeleportInnerOffset.y, t.position.z);
        }
        if (t.position.y < - WorldBorderOffset.y - TeleportOuterOffset.y)
        {
            t.position = new Vector3(t.position.x, WorldBorderOffset.y + TeleportInnerOffset.y, t.position.z);
        }
    }
}
