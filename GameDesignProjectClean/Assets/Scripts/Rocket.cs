using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : Projectile
{
    public float ThrustPower;
    public float ExplosionSpeed;
    public float ExplosionRadius;
    public float ThrusterDuration;
    public float GracePeriod;

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
        if(elapsedTime <= ThrusterDuration)
        {
            elapsedTime += Time.deltaTime;
            rb.AddForceAtPosition(transform.up * ThrustPower, transform.position);
        }        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == GlobalValues.StructureTag)
        {
            // Ignore source ship for a short period of time after the missile is fired
            if (other.gameObject != SourceStructure || elapsedTime >= GracePeriod)
            {
                // TODO Move this to the explosion.
                // Make ship take damage.
                /*Structure str = other.gameObject.GetComponent<Structure>();
                str.TakeDamage(Damage);*/
                // Destroy Missile
                GameObject.Destroy(this.gameObject);
            }            
        }
        else if (other.gameObject.tag == GlobalValues.AsteroidTag)
        {
            // TODO Move damage to the explosion.
            //other.gameObject.GetComponent<Asteroid>().Breakdown();
            // Destroy Missile
            GameObject.Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == GlobalValues.ProjectileTag)
        {
            // If hit by another projectile, destroy.
            GameObject.Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Create the explosion, when the rocket is destroyed. The explosion should be the one doing the damage.
    }
}
