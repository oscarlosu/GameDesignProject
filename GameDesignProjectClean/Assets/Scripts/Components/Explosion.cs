using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    public int Damage;
    public float PushForce;

    void Awake()
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		//Debug.Log ("Explosion detected OnTriggerEnter2D with " + other.gameObject.name);
		Structure str = other.gameObject.GetComponent<Structure>();
		Asteroid ast = other.gameObject.GetComponent<Asteroid>();
        if (str != null)
        {
            // Make ship take damage.            
            str.TakeDamage(Damage);
			// Calculate push force direction
			Vector3 dir = other.transform.position - transform.position;
			dir.Normalize();
            str.ShipCore.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * PushForce, transform.position);
        }
        else if (ast != null)
        {
            other.gameObject.GetComponent<Asteroid>().Breakdown();
        }
		// Only affects rigidbodies with a mass 
		else if (other.GetComponent<Rigidbody2D>() != null && other.GetComponent<Rigidbody2D>().mass > GlobalValues.EffectiveZeroMass)
        {
			Vector3 dir = other.transform.position - transform.position;
			dir.Normalize();
			other.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * PushForce, transform.position);
        }
    }



    private void DestroyExplosion()
    {
        GameObject.Destroy(gameObject);
    }
}
