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
	new void Start ()
	{
		base.Start ();
	}

	new void OnEnable()
	{
		base.OnEnable();
		elapsedTime = 0;
		ready = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
        // If in build mode, don't do anything.
        if (ShipCore.GetComponent<Core>().InBuildMode)
        {
            return;
        }

        // If the ship hasn't been set yet, don't do ANYTHING!
        if (ShipCore == null)
		{
			return;
		}
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
		GameObject pulse = pool.RequestPoolObject(ObjectPool.ObjectType.Pulse, transform.position, Quaternion.identity);
		//GameObject pulse = (GameObject)Instantiate (PulsePrefab, transform.position, Quaternion.identity);
		// Set common projectile variables.
		var projectile = pulse.GetComponent<Projectile>();
		if (projectile != null)
		{
			projectile.SourceCore = ShipCore;
			projectile.SourceStructure = transform.parent.gameObject;
		}
		//pulse.transform.parent = transform;
		// Reduce ship's velocity to zero
		ShipCore.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		ShipCore.GetComponent<Rigidbody2D>().angularVelocity = 0;

		ready = false;
	}
}
