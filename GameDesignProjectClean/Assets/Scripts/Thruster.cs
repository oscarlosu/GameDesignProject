using UnityEngine;
using System.Collections;
using GamepadInput;

public class Thruster : Module
{
    public GamePad.Index Controller;
    public GamePadKey Key;

    public float ThrustPower;


    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    public void Update()
    {
        if (GamePadInputWrapper.GetButton(Key, Controller))
        {
            Activate();
        }
    }

    public void Activate()
    {
        Ship.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * ThrustPower, transform.position);

    }
}
