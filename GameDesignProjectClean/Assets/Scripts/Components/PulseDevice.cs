using UnityEngine;
using System.Collections;
using GamepadInput;

public class PulseDevice : Module
{
	public GameObject PulsePrefab;
	public float Cooldown;

	private float elapsedTime;
	private bool ready;


	// Use this for initialization
	void Start ()
	{
		base.Start ();
		elapsedTime = 0;
		ready = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(ready)
		{
			switch (InputType)
			{
			case InputKeyType.Button:
				if (GamePad.GetButton(ButtonKey, ShipCore.GetComponent<Core>().ControllerIndex))
				{
					Activate();
				}
				break;
			case InputKeyType.Trigger:
				var value = GamePad.GetTrigger(TriggerKey, ShipCore.GetComponent<Core>().ControllerIndex);
				if (value > 0.5) // Activation threshold.
				{
					Activate();
				}
				break;
			}
			ready = false;
		}
		else
		{
			elapsedTime += Time.deltaTime;
			if(elapsedTime > Cooldown)
			{
				ready = true;
				elapsedTime = 0;
			}
		}


	}

	void Activate()
	{
		// Create pulse
		GameObject pulse = Instantiate (PulsePrefab);
		//pulse.transform.parent = transform;
		// Reduce ship's velocity to zero
		ShipCore.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
}
