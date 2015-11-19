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

            rb.AddForceAtPosition(transform.up*ThrustPower, transform.position);
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // The grace period should only make the missiles not hurt their own ship
        if (elapsedTime >= GracePeriod || other.gameObject.GetInstanceID() != SourceCore.GetInstanceID())
        {
            // Create the explosion, when the rocket is destroyed. The explosion should be the one doing the damage.
            // When destroyed, create an explosion, which damages objects around it.
            var explosion = GameObject.Instantiate(ExplosionPrefab);
            explosion.transform.position = transform.position;
            explosion.GetComponent<Explosion>().Damage = Damage;
            GameObject.Destroy(gameObject);
        }
    }
}
