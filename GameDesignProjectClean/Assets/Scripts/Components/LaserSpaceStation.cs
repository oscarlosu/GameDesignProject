using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class LaserSpaceStation : MonoBehaviour
{
    public float EffectDuration;
    public float FadeDuration;
    public int Damage;
	
    private SpriteRenderer rend;
    private float elapsedTime;

    
    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
		GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
		GetComponent<AudioSource> ().Play ();
    }

	void Start()
	{
		elapsedTime = 0;
	}

    // Update is called once per frame
    void Update()
    {        
        elapsedTime += Time.deltaTime;
        //rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, Mathf.Lerp(1, 0, elapsedTime / FadeDuration));
        if (elapsedTime > EffectDuration)
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, Mathf.Lerp(1, 0, elapsedTime / FadeDuration));
            GetComponent<BoxCollider2D>().enabled = false;
        }
        if(elapsedTime > EffectDuration + FadeDuration)
        {
            // Destroy laser
			GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Laser OnTriggerEnter2D with " + other.gameObject.name);
		Structure str = other.gameObject.GetComponent<Structure>();
		Asteroid ast = other.gameObject.GetComponent<Asteroid>();
		Debris deb = other.gameObject.GetComponent<Debris>();
        if (str != null)
        {            
            // Make ship lose hp
            str.TakeDamage(Damage, null);
        }
        else if (ast != null)
        {
			// Make asteroid break into pieces
            ast.Breakdown();            
        }
		else if(deb != null)
		{
			deb.Breakdown ();
		}
    }
}
