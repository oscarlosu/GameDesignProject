using UnityEngine;
using System.Collections;

public class TriggerTest : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Trigger OnCollisionEnter2D called with " + other.gameObject.name);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Trigger OnTriggerEnter2D called with " + other.gameObject.name);
	}
}
