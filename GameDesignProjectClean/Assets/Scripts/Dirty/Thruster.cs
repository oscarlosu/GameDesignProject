using UnityEngine;
using System.Collections;
using GamepadInput;

public class Thruster : Module
{
    public GamePad.Button Button;
    public GamePad.Index Controller;
    public float ThrustPower;


    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    public void Update()
    {
        if (GamePad.GetButton(Button, Controller))
        {
            Activate();
        }
    }

    public void Activate()
    {
        rb.AddForceAtPosition(transform.up * ThrustPower, transform.position);

    }
}
