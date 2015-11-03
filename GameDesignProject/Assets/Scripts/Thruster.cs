using UnityEngine;
using System.Collections;
using GamepadInput;

public class Thruster : Module
{
    public GamePad.Button Button;
    public float ThrustPower;
    public AudioSource ThrusterSound;

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
            //plays sound and makes it loop, sets pitch to equal speed of rigidbody. Would be nice to translate in a 
            //better way, but it's difficult when we dont have a max velocity
            ThrusterSound.pitch = Mathf.Clamp(rb.velocity.magnitude, 0, 1);
			ThrusterSound.enabled = true;
			ThrusterSound.loop = true;
		}
		else
		{
			//Disables sound by setting pitch to 0
			//There probably a nice way of doing this. It would be nice with some rolloff
			ThrusterSound.pitch = 0;
		}
    }

    public void Activate()
    {
        rb.AddForceAtPosition(transform.up * ThrustPower, transform.position);
		
	}
}
