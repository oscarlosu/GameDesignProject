using UnityEngine;
using System.Collections;

public class ColliderTest : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collider OnCollisionEnter2D called with " + other.gameObject.name);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Collider OnTriggerEnter2D called with " + other.gameObject.name);
	}
}
