using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    public float Weight;
    public float ThrustPower;
    public float ExplosionSpeed;
    public float ExplosionRadius;
    public float ThrusterDuration;
    public float GracePeriod;

    private Rigidbody2D rb;
    private float elapsedTime;

    public GameObject SourceStructure;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Handle thruster
        if(elapsedTime <= ThrusterDuration)
        {
            elapsedTime += Time.deltaTime;
            rb.AddForceAtPosition(transform.up * ThrustPower, transform.position);
        }        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Structure")
        {
            // Ignore source ship for a short period of time after the missile is fired
            if (other.gameObject != SourceStructure || elapsedTime >= GracePeriod)
            {
                // Make ship lose a module
                Structure str = other.gameObject.GetComponent<Structure>();
                str.LoseModule();
                // Destroy Missile
                GameObject.Destroy(this.gameObject);
            }            
        }
        else if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Asteroid>().Breakdown();
            // Destroy Missile
            GameObject.Destroy(this.gameObject);
        }
    }
}
