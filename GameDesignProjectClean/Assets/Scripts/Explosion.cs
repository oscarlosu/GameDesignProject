using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Explosion : MonoBehaviour
{
    public int Damage;
    
    void Start(){
    this.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == GlobalValues.StructureTag)
        {
            // Make ship take damage.
            Structure str = other.gameObject.GetComponent<Structure>();
            str.TakeDamage(Damage);
        }
        else if (other.gameObject.tag == GlobalValues.AsteroidTag)
        {
            //other.gameObject.GetComponent<Asteroid>().Breakdown();
        }
    }

    private void DestroyExplosion()
    {
        GameObject.Destroy(gameObject);
    }
}
