using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Laser : Projectile
{
    public float EffectDuration;
    public float FadeDuration;

    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private float elapsedTime;
    
    void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
		this.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {        
        elapsedTime += Time.deltaTime;
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, Mathf.Lerp(1, 0, elapsedTime / FadeDuration));
        if (elapsedTime > EffectDuration)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        if(elapsedTime > FadeDuration)
        {
            // Destroy laser
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Laser OnTriggerEnter2D with " + other.gameObject.name);
		Structure str = other.gameObject.GetComponent<Structure>();
		Asteroid ast = other.gameObject.GetComponent<Asteroid>();
        if (str != null && other.gameObject != SourceStructure)
        {            
            // Make ship lose hp            
            str.TakeDamage(Damage);            
        }
        else if (ast != null)
        {
			// Make asteroid break into pieces
            ast.Breakdown();            
        }
		else
		{
			Debug.Log("Laser OnTriggerEnter2D with UNEXPECTED ENTITY: " + other.gameObject.name);
		}
    }

	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log("Laser OnCollisionEnter2D with " + coll.gameObject.name);
		Structure str = coll.gameObject.GetComponent<Structure>();
		Asteroid ast = coll.gameObject.GetComponent<Asteroid>();
		if (str != null && coll.gameObject != SourceStructure)
		{            
			// Make ship lose hp
			str.TakeDamage(Damage);
			
		}
		else if (ast != null)
		{
			// Make asteroid break into pieces
			ast.Breakdown();            
		}
		else
		{
			Debug.Log("Laser OnCollisionEnter2D with UNEXPECTED ENTITY: " + coll.gameObject.name);
		}
	}
}
