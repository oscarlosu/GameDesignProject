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

		
	public int SplitMinNum, SplitMaxNum;
    public float MinBreakdownForce, MaxBreakdownForce;

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
        switch (Size)
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

    public void Breakdown()
    {
        // Reduce size
        Size = (AsteroidSize)((int)Size - 1);
		switch(Size)
		{
		case AsteroidSize.Large:
			Size = AsteroidSize.Medium;
			break;
		case AsteroidSize.Medium:
			Size = AsteroidSize.Small;
			break;
		case AsteroidSize.Small:
			gameObject.SetActive(false);
			Destroy(gameObject);
			return;
		default:
			throw new System.Exception("Invalid asteroid size");
			break;
		}
		// Split
        int num = Random.Range(SplitMinNum, SplitMaxNum);
        for(int i = 0; i < num; ++i)
        {
			GameObject go = (GameObject)Instantiate(AsteroidPrefab, transform.position, Quaternion.identity);
			go.transform.parent = transform.parent;
			go.GetComponent<Asteroid>().Initialize(Size);
            // Disable collider
            go.GetComponent<CircleCollider2D>().enabled = false;
            // Generate random force
            Vector2 forceDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            forceDir.Normalize();
            float breakdownForce = Random.Range(MinBreakdownForce, MaxBreakdownForce);
            go.GetComponent<Rigidbody2D>().AddForce(forceDir * breakdownForce);
        }
    }

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

	/*void OnTriggerStay2D(Collider2D col)
	{
		// If the other object has a higher mass, break down.
		if (col.gameObject.GetComponent<Rigidbody2D>().mass > GetComponent<Rigidbody2D>().mass)
		{
			Debug.Log(this.gameObject + " destroyed!");
			Breakdown();
		}
	}*/
}
