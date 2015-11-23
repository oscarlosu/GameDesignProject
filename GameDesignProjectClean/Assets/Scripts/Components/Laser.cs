using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Laser : Projectile
{
    public float EffectDuration;
    public float FadeDuration;
	
    private SpriteRenderer rend;
    private float elapsedTime;
    
    void Awake()
    {
		this.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
        rend = GetComponent<SpriteRenderer>();
		InGrace = true;
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
    }
}
