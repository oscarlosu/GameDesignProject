using UnityEngine;

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


    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        this.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        this.GetComponent<AudioSource>().volume = Random.Range(0.9f, 1.1f);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        // Handle thruster
        if (elapsedTime <= ThrusterDuration)
        {
            rb.AddForceAtPosition(transform.up * ThrustPower, transform.position);
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		// With something that is not the source ship or after the grace period
		if (elapsedTime >= GracePeriod || other.gameObject.GetInstanceID() != SourceStructure.GetInstanceID())
		{
			Debug.Log("Rocket detected collision with " + other.gameObject.name);
			Activate();
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		// Missiles only activate with shield or laser triggers
		if ((other.gameObject.GetComponent<Shield>() != null && (elapsedTime > GracePeriod || other.gameObject.GetComponent<Shield>().ShipCore.GetInstanceID() != SourceCore.GetInstanceID()))
		    || other.gameObject.GetComponent<Laser>() != null)
		{
			Debug.Log("Rocket trigger detected trigger with " + other.gameObject.name);
			Activate();
		}
	}

	void Activate()
	{
		// Create the explosion, when the rocket is destroyed. The explosion should be the one doing the damage.
		// When destroyed, create an explosion, which damages objects around it.
		var explosion = GameObject.Instantiate(ExplosionPrefab);
		explosion.transform.position = transform.position;
		explosion.GetComponent<Explosion>().Damage = Damage;
		GameObject.Destroy(gameObject);
	}
}
