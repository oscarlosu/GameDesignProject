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
    public float minBreakdownForce, maxBreakdownForce;
    public float DisableTime;

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
                float breakdownForce = Random.Range(minBreakdownForce, maxBreakdownForce);
                go.GetComponent<Rigidbody2D>().AddForce(forceDir * breakdownForce);
            }
        }
        GameObject.Destroy(this.gameObject);
    }
}
