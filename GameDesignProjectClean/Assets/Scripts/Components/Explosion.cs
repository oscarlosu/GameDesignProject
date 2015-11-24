using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    public float scaleFactor = 1.0f;
    public float duration = 1.0f;
    public AnimationCurve explosionAnim = new AnimationCurve(new Keyframe(0, 0.05f), new Keyframe(0.5f, 1), new Keyframe(1, 0.05f));
    [Space(10)]
    public int Damage;
    public float PushForce;

    public Material[] explosionMats;

    private float counter = 0.0f;

    void Awake()
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
        Material mat = explosionMats[Random.Range(0, explosionMats.Length)];
        GetComponent<MeshRenderer>().material = mat;
        StartCoroutine("DestroyExplosion");
    }

    void Update()
    {
        float newScale = explosionAnim.Evaluate(counter) * scaleFactor;
        transform.localScale = Vector3.one * newScale;
        counter += Time.deltaTime/duration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		////Debug.Log ("Explosion detected OnTriggerEnter2D with " + other.gameObject.name);
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
			// Calculate push force direction
			Vector3 dir = other.transform.position - transform.position;
			dir.Normalize();
			ast.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * PushForce, transform.position);
        }
		// Only affects rigidbodies with a mass 
		/*else if (other.GetComponent<Rigidbody2D>() != null && 
		         other.GetComponent<Collider2D>() != null && !other.GetComponent<Collider2D>().isTrigger/*other.GetComponent<Rigidbody2D>().mass > GlobalValues.EffectiveZeroMass)
        /*{
			Vector3 dir = other.transform.position - transform.position;
			dir.Normalize();
			other.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * PushForce, transform.position);
        }*/
    }

    

    IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(duration);
        GameObject.Destroy(gameObject);
    }
}
