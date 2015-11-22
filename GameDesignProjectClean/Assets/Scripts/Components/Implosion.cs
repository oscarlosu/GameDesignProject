using UnityEngine;
using System.Collections;

public class Implosion : MonoBehaviour
{	
	public float PullForce;

	private Animator anim;
	void Awake ()
	{
		//GetComponent<AudioSource> ().pitch = Random.Range (0.5f, 1.5f);
	}

	void Start()
	{
		anim = GetComponent<Animator>();
		ParticleManager.Instance.SpawnParticle(ParticleType.Implosion, transform.position);

	}
	
	private void OnTriggerExit2D (Collider2D other)
	{
		if (other.attachedRigidbody != null) {
			// Calculate pull force direction
			Vector3 dir = transform.position - other.transform.position;
			dir.Normalize ();
			other.attachedRigidbody.AddForce(dir * PullForce);
		}
	}
	
	private void DestroyImplosion ()
	{
		GameObject.Destroy (gameObject);
	}
}
