using UnityEngine;
using System.Collections;

public class ShipComponent : MonoBehaviour
{
    public GameObject Ship;
    public int Mass;

    protected void Start()
    {
    }


    protected void OnDestroy()
    {
        Ship.GetComponent<Rigidbody2D>().mass -= Mass;
    }
}
