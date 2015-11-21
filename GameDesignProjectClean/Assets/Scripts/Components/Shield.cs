using UnityEngine;
using System.Collections;

public class Shield : Module
{
	public bool Active;

	public float Radius;
	public float DurationAfterFirstHit;
	public float Cooldown;
	public float GracePeriod;

	private CircleCollider2D col;
	private Animator anim;
	private bool deactivating;
	private float elapsedTime;

	// Use this for initialization
	void Start ()
	{
		base.Start ();
		col = GetComponent<CircleCollider2D>();
		col.radius = Radius;
		Active = true;
		deactivating = false;
		anim = GetComponent<Animator>();
		anim.SetBool("Active", true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		elapsedTime += Time.deltaTime;
		if(deactivating)
		{
			// When the shield blocked something, it remains active for a short period of time
			if(elapsedTime > DurationAfterFirstHit)
			{
				deactivating = false;
				Active = false;
				anim.SetBool("Active", false);
				elapsedTime = 0;
			}
		}
		else if(!Active)
		{
			// The shield reactivates automatically when thge cooldown period is over
			if(elapsedTime > Cooldown)
			{
				Active = true;
				anim.SetBool("Active", true);
				elapsedTime = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		// The shield starts deactivating when it collides with a projectile (only when it is active)
		if(Active && other.gameObject.GetComponent<Projectile>() != null && other.gameObject.GetComponent<Projectile>().SourceCore.GetInstanceID() != ShipCore.GetInstanceID())
		{
			elapsedTime = 0;
			deactivating = true;

		}
	}


}
