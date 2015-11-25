using UnityEngine;
using System.Collections;

public class ImplosionBomb : Projectile
{
	public float ThrustPower;
	public float ThrusterDuration;
	public float GracePeriod;
	public GameObject ImplosionPrefab;
	private Rigidbody2D rb;
	private float elapsedTime;

    private ParticleSystem[] childParticles;


    // Use this for initialization
	new void Awake ()
	{
		base.Awake();
		rb = GetComponent<Rigidbody2D> ();
		/*this.GetComponent<AudioSource> ().pitch = Random.Range (0.9f, 1.1f);
		this.GetComponent<AudioSource> ().volume = Random.Range (0.9f, 1.1f);*/
		InGrace = true;

        childParticles = gameObject.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < childParticles.Length; i++)
        {
            if (!childParticles[i].isPlaying)
                childParticles[i].Play();
        }
    }

	void Start()
	{
		InvokeRepeating("GarbageCollect", 5, 5);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update elapsed time until the mine is active
		elapsedTime += Time.deltaTime;
		// Handle grace period
		if(InGrace)
		{
			if(elapsedTime > GracePeriod)
			{
				InGrace = false;
			}
		}   
		// Handle thruster
		if (elapsedTime <= ThrusterDuration)
		{
			rb.AddForceAtPosition(transform.up * ThrustPower, transform.position);
		}
		else
		{
            for (int i = 0; i < childParticles.Length; i++)
            {
                if (childParticles[i].isPlaying)
                    childParticles[i].Stop();
            }
            //GetComponent<AudioSource>().Stop();
        }
	}
	
	void OnCollisionEnter2D (Collision2D other)
	{
		// With something that is not the source ship or after the grace period
		if (!InGrace || other.gameObject.GetInstanceID () != SourceStructure.GetInstanceID ()) {
			//Debug.Log ("Implosion bomb detected collision with " + other.gameObject.name);
			Activate ();
		}
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		// Missiles only activate with shield or laser triggers
		if ((other.gameObject.GetComponent<Shield> () != null && (!InGrace || other.gameObject.GetComponent<Shield> ().ShipCore.GetInstanceID () != SourceCore.GetInstanceID ()))
			|| other.gameObject.GetComponent<Laser> () != null) {
			//Debug.Log ("Implosion bomb trigger detected trigger with " + other.gameObject.name);
			Activate ();
		}
	}
	
	public void Activate ()
	{
		// When destroyed, create an implosion
		pool.RequestPoolObject(ObjectPool.ObjectType.Implosion, transform.position, Quaternion.identity);
		pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.ImplosionBomb);
	}

	void GarbageCollect()
	{
		// Return to object pool if too far from the camera position
		if(Vector3.Distance (cam.transform.position, transform.position) > DisableDistance ||
		   elapsedTime > MaxLifespan)
		{
			pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.ImplosionBomb);
		}
	}

	void OnDisable()
	{
		CancelInvoke();
	}
}
