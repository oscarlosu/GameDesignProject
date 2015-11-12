using UnityEngine;
using System.Collections;

public class ShipComponent : MonoBehaviour
{
    public GameObject Core;
    public int Mass;

    protected void Start()
    {
    }


    protected void OnDestroy()
    {
        Core.GetComponent<Rigidbody2D>().mass -= Mass;
    }
}
