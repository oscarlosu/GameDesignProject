using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Module : MonoBehaviour
{
    public float Weight;
    public List<GameObject> Sockets;
    public Rigidbody2D rb;
    public GameObject Ship;

    protected void Start()
    {
        rb.mass += Weight;
    }


    protected void OnDestroy()
    {
        rb.mass -= Weight;
    }



}
