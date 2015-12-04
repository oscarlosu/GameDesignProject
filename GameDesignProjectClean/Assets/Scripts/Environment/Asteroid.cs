using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
	public AsteroidSize Size;

    public Sprite[] SmallSprites;
    public Sprite[] MediumSprites;
	public Sprite[] LargeSprites;

	public float SmallColRadius, MediumColRadius, LargeColRadius;
	public float SmallMinWeight, SmallMaxWeight;
    public float MediumMinWeight, MediumMaxWeight;
	public float LargeMinWeight, LargeMaxWeight;

    public float BreakdownVelocity;

    public float DisableTime;

	public GameObject AsteroidPrefab;

    private float elapsedTime;

	public enum AsteroidSize
	{
		Small = 0,
		Medium = 1,
		Large = 2
	}

	void Awake()
	{
		Initialize (Size);
	}
    void Update()
    {
        if(!GetComponent<CircleCollider2D>().enabled)
        {
            elapsedTime += Time.deltaTime;
        }
        if(!GetComponent<CircleCollider2D>().enabled && elapsedTime >= DisableTime)
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }

	public void Initialize()
	{
		Size = (AsteroidSize)Random.Range (0, System.Enum.GetNames(typeof(AsteroidSize)).Length);
		Initialize(Size);
	}

    public void Initialize(AsteroidSize size)
    {
        int index;
        switch (size)
        {
		case AsteroidSize.Small:
			index = Random.Range(0, SmallSprites.Length);
            GetComponent<SpriteRenderer>().sprite = SmallSprites[index];
            GetComponent<Rigidbody2D>().mass = Random.Range(SmallMinWeight, SmallMaxWeight);
            GetComponent<CircleCollider2D>().radius = SmallColRadius;
            break;
		case AsteroidSize.Medium:
			index = Random.Range(0, MediumSprites.Length);
            GetComponent<SpriteRenderer>().sprite = MediumSprites[index];
            GetComponent<Rigidbody2D>().mass = Random.Range(MediumMinWeight, MediumMaxWeight);
            GetComponent<CircleCollider2D>().radius = MediumColRadius;
            break;
		case AsteroidSize.Large:
			index = Random.Range(0, LargeSprites.Length);
			GetComponent<SpriteRenderer>().sprite = LargeSprites[index];
			GetComponent<Rigidbody2D>().mass = Random.Range(LargeMinWeight, LargeMaxWeight);
			GetComponent<CircleCollider2D>().radius = LargeColRadius;
			break;
		default:
			throw new System.Exception("Invalid asteroid size");
            break;
        }
    }

    public void Breakdown(Collision2D coll)
	{
        // Reduce size
		if(Size == AsteroidSize.Small)
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
			return;
		}
		Size = Size - 1;
		Initialize(Size);
        // Make asteroid move in the opposite direction of the collision
        Vector2 dir = Vector2.zero;
        if(coll != null)
        {
            dir = transform.position;
            dir -= coll.contacts[0].point;
            dir.Normalize();
        }
        else
        {
            dir = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            dir.Normalize();
        }
        GetComponent<Rigidbody2D>().velocity = dir * BreakdownVelocity;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Only "crash" if the force with which the objects hit each other is large enough.
        if (coll.relativeVelocity.magnitude > GlobalValues.MinCrashMagnitude)
        {
            // If the other object has a higher mass, break down.
            Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
            float otherMass = rb.mass;
			float mass = GetComponent<Rigidbody2D>().mass;
            if (otherMass > mass)
            {
                Breakdown(coll);
				GetComponent<AudioSource>().Play();

            }
        }
    }
}
