﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Rocket : Projectile
{
    public float ThrustPower;
    public float ThrusterDuration;
    public float GracePeriod;

    public GameObject ExplosionPrefab;

    private Rigidbody2D rb;
    private float elapsedTime;

    private ParticleSystem[] childParticles;


    // Use this for initialization
	new void Awake()
    {
		base.Awake();
        rb = GetComponent<Rigidbody2D>();
        this.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        this.GetComponent<AudioSource>().volume = Random.Range(0.9f, 1.1f);

        childParticles = gameObject.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < childParticles.Length; i++)
        {
            if (!childParticles[i].isPlaying)
                childParticles[i].Play();
        }
    }

	void OnEnable()
	{
		elapsedTime = 0;
		InGrace = true;
        InvokeRepeating("GarbageCollect", 5, 5);
    }

    // Update is called once per frame
    void Update()
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
            GetComponent<AudioSource>().Stop();
            for (int i = 0; i < childParticles.Length; i++)
            {
                if (childParticles[i].isPlaying)
                    childParticles[i].Stop();
            }
        }

    }

	void OnCollisionEnter2D(Collision2D other)
	{
		// With something that is not the source ship or after the grace period
		if (!InGrace || other.gameObject.GetInstanceID() != SourceStructure.GetInstanceID())
		{
			//Debug.Log("Rocket detected collision with " + other.gameObject.name);
			Activate();
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		// Missiles only activate with shield or laser triggers
		if ((other.gameObject.GetComponent<Shield>() != null && (!InGrace || other.gameObject.GetComponent<Shield>().ShipCore.GetInstanceID() != SourceCore.GetInstanceID()))
		    || other.gameObject.GetComponent<Laser>() != null || other.gameObject.GetComponent<LaserSpaceStation>() != null)
		{
			//Debug.Log("Rocket trigger detected trigger with " + other.gameObject.name);
			Activate();
		}
	}

	void Activate()
	{
		// Create the explosion, when the rocket is destroyed. The explosion should be the one doing the damage.
		// When destroyed, create an explosion, which damages objects around it.
		GameObject explosion = pool.RequestPoolObject(ObjectPool.ObjectType.Explosion, transform.position, Quaternion.identity);
		//var explosion = GameObject.Instantiate(ExplosionPrefab);
		//explosion.transform.position = transform.position;
		explosion.GetComponent<Explosion>().Damage = Damage;
	    explosion.GetComponent<Explosion>().SourceCore = SourceCore;
		pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.Rocket);
	}

	void GarbageCollect()
	{
        if (elapsedTime > MaxLifespan || Vector3.Distance(transform.position, SourceCore.transform.position) > MaxDistance)
        {
            Activate();
		}
	}

	void OnDisable()
	{
		CancelInvoke();
	}
}
