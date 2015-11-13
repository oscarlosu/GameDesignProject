using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Rocket : Projectile
{
    public float ThrustPower;
    public float ExplosionSpeed;
    public float ExplosionRadius;
    public float ThrusterDuration;
    public float GracePeriod;

    public GameObject ExplosionPrefab;

    private Rigidbody2D rb;
    private float elapsedTime;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle thruster
        if (elapsedTime <= ThrusterDuration)
        {
            elapsedTime += Time.deltaTime;
            rb.AddForceAtPosition(transform.up * ThrustPower, transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Create the explosion, when the rocket is destroyed. The explosion should be the one doing the damage.
        // When destroyed, create an explosion, which damages objects around it.
        var explosion = GameObject.Instantiate(ExplosionPrefab);
        explosion.transform.position = transform.position;
    }
}
