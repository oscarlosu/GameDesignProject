using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour
{
    public int Size;
    public int SizeMin, SizeMax;
    public Sprite[] SmallSprites;
    public Sprite[] MediumSprites;
    public float SmallColRadius, MediumColRadius;
    public float SmallMinWeight, SmallMaxWeight;
    public float MediumMinWeight, MediumMaxWeight;
    public int SplitMinNum, SplitMaxNum;
    public float MinBreakdownForce, MaxBreakdownForce;
    public float DisableTime;
    public float MinCrashMagnitude;

    private float elapsedTime;

	
	
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
    public void Initialise()
    {
        int index;
        switch (Size)
        {
            case 1:
                index = Random.Range(0, SmallSprites.Length);
                GetComponent<SpriteRenderer>().sprite = SmallSprites[index];
                GetComponent<Rigidbody2D>().mass = Random.Range(SmallMinWeight, SmallMaxWeight);
                GetComponent<CircleCollider2D>().radius = SmallColRadius;
                break;
            case 2:
                index = Random.Range(0, MediumSprites.Length);
                GetComponent<SpriteRenderer>().sprite = MediumSprites[index];
                GetComponent<Rigidbody2D>().mass = Random.Range(MediumMinWeight, MediumMaxWeight);
                GetComponent<CircleCollider2D>().radius = MediumColRadius;
                break;
            default:
                Size = 1;
                Initialise();
                break;
        }
    }

    public void Breakdown()
    {
        // Reduce size
        --Size;
        // Split and destroy
        if(Size > 0)
        {
            int num = Random.Range(SplitMinNum, SplitMaxNum);
            for(int i = 0; i < num; ++i)
            {
                var go = (GameObject)Instantiate(this.gameObject);
                go.GetComponent<Asteroid>().Size = Size;
                go.GetComponent<Asteroid>().Initialise();
                // Disable collider
                go.GetComponent<CircleCollider2D>().enabled = false;
                // Generate random force
                Vector2 forceDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                Debug.Log(forceDir);
                forceDir.Normalize();
                float breakdownForce = Random.Range(MinBreakdownForce, MaxBreakdownForce);
                go.GetComponent<Rigidbody2D>().AddForce(forceDir * breakdownForce);
            }
        }
        // Play sound just before destroying the asteroid game object.
        
        // Destroy game object.
        GameObject.Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Only "crash" if the force with which the objects hit each other is large enough.
        if (coll.relativeVelocity.magnitude > MinCrashMagnitude)
        {
            Debug.Log("Crashed with a magnitude of: " + coll.relativeVelocity.magnitude);
            // If the other object has a higher mass, break down.
            if (coll.gameObject.GetComponent<Rigidbody2D>().mass > GetComponent<Rigidbody2D>().mass)
            {
                Debug.Log(this.gameObject + " destroyed!");
                Breakdown();
            }
        }
    }
}
