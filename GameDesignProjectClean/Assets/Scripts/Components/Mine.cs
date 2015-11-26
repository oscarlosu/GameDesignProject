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
	public Collider2D MineCollider;

    public GameObject ExplosionPrefab;

    private float elapsedTime;
    private bool detected; // If any object has been detected in its vicinity.
    private float timeFromDetection;

    private Animator anim;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
		InvokeRepeating("GarbageCollect", 5, 5);
    }

	void OnEnable()
	{
		InGrace = true;
		detected = false;
		elapsedTime = 0;
	}

    // Update is called once per frame
    void Update()
    {
		// Update elapsed time until the mine is active
		elapsedTime += Time.deltaTime;
		// Handle grace period
		if(InGrace && elapsedTime > GracePeriod)
		{
			InGrace = false;
		}        

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
		//Debug.Log ("Mine detected collision with " + coll.gameObject.name);
		// Rockets and missiles will be activated and will their explosion will make the mine explode in an OnTriggerEnter2D call
	}

    private void OnTriggerEnter2D(Collider2D other)
    {

		if (elapsedTime > TimeTillActive && ShouldTrigger(other))
        {
            detected = true;
            anim.SetTrigger("TriggerDamage");
        }
		else if(ShouldExplode(other) && other.IsTouching (MineCollider))
		{
			Activate();
		}

    }

    private void Activate()
    {
        // Create the explosion, when the rocket is destroyed. The explosion should be the one doing the damage.
        // When destroyed, create an explosion, which damages objects around it.
		GameObject explosion = pool.RequestPoolObject(ObjectPool.ObjectType.Explosion, transform.position, Quaternion.identity);
		//var explosion = GameObject.Instantiate(ExplosionPrefab);
        //explosion.transform.position = transform.position;
        explosion.GetComponent<Explosion>().Damage = Damage;
		pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.Mine);
		//GameObject.Destroy(gameObject);
    }

	private bool ShouldTrigger(Collider2D other)
	{
		return other.gameObject.GetComponent<Structure>() != null || other.gameObject.GetComponent<Rocket>() != null ||
		       other.gameObject.GetComponent<HomingMissile>() != null || other.gameObject.GetComponent<Mine>() != null;
	}

	private bool ShouldExplode(Collider2D other)
	{
		return other.gameObject.GetComponent<Explosion>() != null || other.gameObject.GetComponent<Laser>() != null || 
			(other.gameObject.GetComponent<Shield>() != null && (!InGrace || other.gameObject.GetComponent<Shield>().ShipCore.GetInstanceID() != SourceCore.GetInstanceID()));
	}

	void GarbageCollect()
	{
		// Return to object pool if too far from the camera position
		if(Vector3.Distance (cam.transform.position, transform.position) > DisableDistance ||
		   elapsedTime > MaxLifespan)
		{
			pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.Mine);
		}
	}

	void OnDisable()
	{
		CancelInvoke();
	}
}
