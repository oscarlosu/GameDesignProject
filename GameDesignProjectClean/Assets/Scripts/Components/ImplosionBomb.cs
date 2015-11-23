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
	public bool InGrace {get; private set;}
	
	// Use this for initialization
	void Awake ()
	{
		rb = GetComponent<Rigidbody2D> ();
		/*this.GetComponent<AudioSource> ().pitch = Random.Range (0.9f, 1.1f);
		this.GetComponent<AudioSource> ().volume = Random.Range (0.9f, 1.1f);*/
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update elapsed time until the mine is active
		if(elapsedTime <= ThrusterDuration)
		{
			elapsedTime += Time.deltaTime;
		}
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
			//GetComponent<AudioSource>().Stop();
		}
	}
	
	void OnCollisionEnter2D (Collision2D other)
	{
		// With something that is not the source ship or after the grace period
		if (!InGrace || other.gameObject.GetInstanceID () != SourceStructure.GetInstanceID ()) {
			Debug.Log ("Implosion bomb detected collision with " + other.gameObject.name);
			Activate ();
		}
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		// Missiles only activate with shield or laser triggers
		if ((other.gameObject.GetComponent<Shield> () != null && (!InGrace || other.gameObject.GetComponent<Shield> ().ShipCore.GetInstanceID () != SourceCore.GetInstanceID ()))
			|| other.gameObject.GetComponent<Laser> () != null) {
			Debug.Log ("Implosion bomb trigger detected trigger with " + other.gameObject.name);
			Activate ();
		}
	}
	
	public void Activate ()
	{
		// Create the explosion, when the rocket is destroyed. The explosion should be the one doing the damage.
		// When destroyed, create an explosion, which damages objects around it.
		var implosion = (GameObject)GameObject.Instantiate (ImplosionPrefab, transform.position, Quaternion.identity);
		GameObject.Destroy (gameObject);
	}
}
