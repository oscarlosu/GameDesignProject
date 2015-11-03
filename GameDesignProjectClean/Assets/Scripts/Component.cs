using UnityEngine;
using System.Collections;

public class Component : MonoBehaviour
{
    public GameObject Ship;
    public int Mass;

    protected void Start()
    {
        Ship.GetComponent<Rigidbody2D>().mass += Mass;
    }


    protected void OnDestroy()
    {
        Ship.GetComponent<Rigidbody2D>().mass -= Mass;
    }
}
