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
    
    new void Awake()
    {
		base.Awake();
        rend = GetComponent<SpriteRenderer>();
		GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
		GetComponent<AudioSource> ().Play ();
    }

	void OnEnable()
	{
		elapsedTime = 0;
		InGrace = true;
        GetComponent<BoxCollider2D>().enabled = true;
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
			pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.Laser);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		Structure str = other.gameObject.GetComponent<Structure>();
		Asteroid ast = other.gameObject.GetComponent<Asteroid>();
		Debris deb = other.gameObject.GetComponent<Debris>();
        if (str != null && other.gameObject != SourceStructure)
        {            
            // Make ship lose hp
            str.TakeDamage(Damage, SourceCore.GetComponent<Core>());            
        }
        else if (ast != null)
        {
			// Make asteroid break into pieces
            ast.Breakdown(null);            
        }
		else if(deb != null)
		{
			deb.Breakdown ();
		}
    }
}
