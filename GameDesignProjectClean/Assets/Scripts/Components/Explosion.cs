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
        if (other.gameObject.tag == GlobalValues.StructureTag)
        {
            // Make ship take damage.
            Structure str = other.gameObject.GetComponent<Structure>();
            str.TakeDamage(Damage);
            str.Core.GetComponent<Rigidbody2D>().AddForceAtPosition((-transform.up) * PushForce, transform.position);
        }
        else if (other.gameObject.tag == GlobalValues.AsteroidTag)
        {
            other.gameObject.GetComponent<Asteroid>().Breakdown();
        }
        else if (other.GetComponent<Rigidbody2D>() != null)
        {
            
            other.GetComponent<Rigidbody2D>().AddForceAtPosition((-transform.up) * PushForce, transform.position);
        }
    }

    private void DestroyExplosion()
    {
        GameObject.Destroy(gameObject);
    }
}
