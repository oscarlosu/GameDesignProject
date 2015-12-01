using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlackHoleController : MonoBehaviour, ILevelHandler
{
    // Animation
    public SpriteRenderer[] blackHoleParts;
    [Space(10)]
    public float speed = 0.0f;
    public float moveModifier = 0.0f;
    public AnimationCurve fade = new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(5, 1));

    // Physics
    public float PullRadius;
    public float ForceTowardsCenter;
    public float ForceTangent;

    public float PushRadius;
    public float PushForce;

    private List<GameObject> ships;

    // Use this for initialization
    void Start()
    {
        var gameHandler = GameObject.FindGameObjectWithTag("GameHandler");
        if (gameHandler != null)
        {
            gameHandler.GetComponent<GameHandler>().LevelHandler = this;
            StartLevel(gameHandler.GetComponent<GameHandler>().GetPlayerShips());
        }

        ships = GameObject.FindGameObjectsWithTag(GlobalValues.ShipTag).ToList<GameObject>();        
    }

    // Update is called once per frame
    void Update()
    {
        // Animate
        Rotate();
        Move();
        Fade();
        // Pull ships in
        WhirlpoolEffect();
    }


    public void StartLevel(GameObject[] playerShips)
    {
        if (playerShips[0] != null)
        {
            playerShips[0].transform.position = new Vector3(-45, 45);
            playerShips[0].transform.eulerAngles = new Vector3(0, 0, 135);
            playerShips[0].SetActive(true);
        }
        if (playerShips[1] != null)
        {
            playerShips[1].transform.position = new Vector3(45, 45);
            playerShips[1].transform.eulerAngles = new Vector3(0, 0, 45);
            playerShips[1].SetActive(true);
        }
        if (playerShips[2] != null)
        {
            playerShips[2].transform.position = new Vector3(-45, -45);
            playerShips[2].transform.eulerAngles = new Vector3(0, 0, 225);
            playerShips[2].SetActive(true);
        }
        if (playerShips[3] != null)
        {
            playerShips[3].transform.position = new Vector3(45, -45);
            playerShips[3].transform.eulerAngles = new Vector3(0, 0, 315);
            playerShips[3].SetActive(true);
        }
    }




    void Rotate()
    {
        for (int i = 0; i < blackHoleParts.Length - 1; i++)
        {
            blackHoleParts[i].transform.Rotate(new Vector3(0, 0, (((i + 1) * 1.5f) * speed) * Time.deltaTime));
        }
    }

    void Move()
    {
        for (int i = 0; i < blackHoleParts.Length; i++)
        {
            blackHoleParts[i].transform.position = (new Vector3(Mathf.Sin(Time.realtimeSinceStartup) * (i * moveModifier),
                Mathf.Sin(Time.realtimeSinceStartup) * (i * moveModifier),
                0));
        }
    }

    void Fade()
    {
        for (int i = 0; i < blackHoleParts.Length - 1; i++)
        {
            float flo = Time.realtimeSinceStartup + i;
            blackHoleParts[i].color = new Color(1, 1, 1, fade.Evaluate(flo));
        }
    }

    void WhirlpoolEffect()
    {
        //Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, PullRadius);

        foreach(GameObject ship in ships)
        {
            if(ship != null)
            {
                Rigidbody2D rb = ship.GetComponent<Rigidbody2D>();
                Vector2 dirToCenter = transform.position - rb.transform.position;
                float distanceToCenter = dirToCenter.magnitude;
                if (distanceToCenter > PushRadius)
                {
                    // Pull in
                    dirToCenter.Normalize();
                    Vector2 tangentDir = new Vector2(-dirToCenter.y, dirToCenter.x);
                    tangentDir.Normalize();
                    //float tangentForce = Mathf.Lerp(0, ForceTangent, (distanceToCenter / EffectRadius));
                    //col.attachedRigidbody.velocity += ((dirToCenter * ForceTowardsCenter / distanceToCenter) * Time.deltaTime);
                    //col.attachedRigidbody.velocity -= (tangentDir * ForceTangent * Time.deltaTime);


                    Vector2 finalCenterForce = dirToCenter * ForceTowardsCenter * rb.mass;
                    Vector2 finalTangentForce = -tangentDir * ForceTangent * rb.mass;
                    rb.AddForce(finalCenterForce);
                    rb.AddForce(finalTangentForce);

                    //Debug.DrawRay(rb.transform.position, finalCenterForce, Color.red, 0.5f);
                    //Debug.DrawRay(rb.transform.position, finalTangentForce, Color.green, 0.5f);
                    //Debug.DrawRay(rb.transform.position, finalCenterForce + finalTangentForce, Color.blue, 0.5f);
                }


            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            // Push out
            Vector2 pushDir = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            pushDir.Normalize();
            other.attachedRigidbody.velocity = other.attachedRigidbody.velocity.normalized * PushForce;

           // Debug.DrawRay(other.attachedRigidbody.transform.position, pushDir * PushForce, Color.red, 0.5f);
        }
    }
}