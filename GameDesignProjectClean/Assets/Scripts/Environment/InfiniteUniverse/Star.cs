using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Star : MonoBehaviour
{
	public List<int> OrbitRadii;
	public int Mass;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create(Vector3 starPos, int starRadius, int starMass, List<int> planetOrbitRadii, Sprite starSprite)
    {
        GetComponent<SpriteRenderer>().sprite = starSprite;
        transform.position = starPos;
        transform.localScale = new Vector2(2 * starRadius, 2 * starRadius);
		OrbitRadii = planetOrbitRadii;
		Mass = starMass;
		GetComponent<Rigidbody2D>().mass = starMass;
		tag = GlobalValues.StarTag;
    }
}
