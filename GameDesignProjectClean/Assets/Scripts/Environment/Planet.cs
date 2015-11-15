using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Planet : MonoBehaviour
{
	public GameObject Star;
	public int Mass;
	public int AngularSpeed;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.RotateAround(Star.transform.position, Vector3.forward, Time.deltaTime * AngularSpeed);
	}

	public void Create (Vector3 planetPos, int planetRadius, int planetMass, GameObject planetStar, int planetAngularSpeed, Sprite planetSprite)
	{
		GetComponent<SpriteRenderer> ().sprite = planetSprite;
		transform.position = planetPos;
		transform.localScale = new Vector2 (2 * planetRadius, 2 * planetRadius);
		Mass = planetMass;
		GetComponent<Rigidbody2D>().mass = planetMass;
		Star = planetStar;
		AngularSpeed = planetAngularSpeed;
		tag = GlobalValues.PlanetTag;
		Debug.Log ("Planet created!");
	}
}
