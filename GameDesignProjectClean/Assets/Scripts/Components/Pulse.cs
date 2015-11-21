using UnityEngine;
using System.Collections;

public class Pulse : Projectile
{
	public float RepulsionForce;
	public float Radius;
	public float Speed;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Pulse detected OnTriggerEnter2D with " + other.gameObject.name);
		if(!other.isTrigger && other.attachedRigidbody != null && other.attachedRigidbody.gameObject.GetInstanceID() != SourceCore.GetInstanceID())
		{
			// Cancel velocity in the direction of the shield device and apply force to rigidbody
			Vector2 dir = other.attachedRigidbody.transform.position - transform.position;
			dir.Normalize();
			// Calculate orthogonal vector to dir
			Vector2 orthogonal = new Vector2(- dir.y, dir.x);
			// Project velocity on orthogonal direction
			other.attachedRigidbody.velocity = Vector2.Dot (orthogonal, other.attachedRigidbody.velocity) * orthogonal;
			other.attachedRigidbody.AddForce(RepulsionForce * dir);
		}
	}
}
