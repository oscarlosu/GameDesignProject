using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Mine : Projectile
{
    public float TimeTillActive; // The time from it's spawned till the mine is active (will start detect objects in its vicinity).
    public float DetectionToExplosionTime; // The time it takes from something being in its vicinity till it will blow up.
    public float GracePeriod;

    public GameObject ExplosionPrefab;

    private float elapsedTime;
    private bool detected = false; // If any object has been detected in its vicinity.
    private float timeFromDetection;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (detected)
        {
            timeFromDetection += Time.deltaTime;
            if (timeFromDetection >= DetectionToExplosionTime)
            {
                Activate();
            }
        }
    }
	// If we want mines to explode as soon as they collide with anything with a non-trigger collider, we just need to uncomment this function
	private void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("Mine detected collision with " + coll.gameObject.name);
		/*if(elapsedTime > GracePeriod || coll.gameObject.GetInstanceID() != SourceCore.GetInstanceID())
		{
			Activate();
		}*/

		// The mine explosion is triggered by explosions, lasers and shields. In the case of the shields, 
		// the mine will only activate if the grace period is over or the shield is not from the same ship
		if(coll.gameObject.GetComponent<Explosion>() != null || coll.gameObject.GetComponent<Laser>() != null || 
		   (coll.gameObject.GetComponent<Shield>() != null && (elapsedTime > GracePeriod || coll.gameObject.GetComponent<Shield>().ShipCore.GetInstanceID() != SourceCore.GetInstanceID())))
		{
			Activate();
		}
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
		if (elapsedTime > TimeTillActive)
        {
            detected = true;
            anim.SetTrigger("TriggerDamage");
        }

    }

    private void Activate()
    {
        // Create the explosion, when the rocket is destroyed. The explosion should be the one doing the damage.
        // When destroyed, create an explosion, which damages objects around it.
        var explosion = GameObject.Instantiate(ExplosionPrefab);
        explosion.transform.position = transform.position;
        explosion.GetComponent<Explosion>().Damage = Damage;
        GameObject.Destroy(gameObject);
    }
}
