using UnityEngine;
using System.Collections;

public class Debris : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D coll)
	{
		// Only "crash" if the force with which the objects hit each other is large enough.
		if (coll.relativeVelocity.magnitude > GlobalValues.MinCrashMagnitude)
		{
			// If the other object has a higher mass, break down.
			if (coll.gameObject.GetComponent<Rigidbody2D>().mass > GetComponent<Rigidbody2D>().mass)
			{
				Breakdown();
			}
		}
	}

	public void Breakdown()
	{
		gameObject.SetActive(false);
		Destroy (gameObject);
	}
}
