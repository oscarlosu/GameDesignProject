using UnityEngine;
using System.Collections;

public class Pulse : Projectile
{
    public float scaleFactor = 1.0f;
    public float duration = 1.0f;
    public AnimationCurve explosionAnim = new AnimationCurve(new Keyframe(0, 0.05f), new Keyframe(1, 1));
    [Space(10)]
    public float RepulsionForce;
	public float TorqueMagnitude;
	//public AnimationCurve ForceMassScaleCurve = new AnimationCurve(new Keyframe(0, 0.05f), new Keyframe(1, 1));

	public float Radius;
	public float Speed;

	private CircleCollider2D col;
    public Sprite[] pulseMats;
    private SpriteRenderer sr;

    private float counter = 0.0f;
    private Sprite mat;
	//private CircleCollider2D col;

    private float elapsedTime;
    // Use this for initialization
	new void Awake ()
	{
		col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        mat = pulseMats[Random.Range(0, pulseMats.Length)];
        sr.sprite = mat;
        //mat = GetComponent<MeshRenderer>().material;
		base.Awake ();
		//col = GetComponent<CircleCollider2D>();
        //mat = GetComponent<MeshRenderer>().material;        
    }

	void OnEnable()
	{
		InGrace = true;
		elapsedTime = 0;
		StartCoroutine("DestroyPulse");
	}
	
	// Update is called once per frame
	void Update ()
	{
        sr.color  = new Color(1,1,1, 1.1f - explosionAnim.Evaluate(elapsedTime));
        float newScale = explosionAnim.Evaluate(elapsedTime) * scaleFactor;
        transform.localScale = Vector3.one * newScale;
        elapsedTime += Time.deltaTime / duration;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		//////Debug.Log ("Pulse detected OnTriggerEnter2D with " + other.gameObject.name);
		if(!other.isTrigger && other.attachedRigidbody != null && other.attachedRigidbody.gameObject.GetInstanceID() != SourceCore.GetInstanceID())
		{
			// Cancel velocity in the direction of the shield device and apply force to rigidbody
			Vector2 dir = other.attachedRigidbody.transform.position - transform.position;
			dir.Normalize();
			// Calculate orthogonal vector to dir
			Vector2 orthogonal = new Vector2(- dir.y, dir.x);
			// Project velocity on orthogonal direction
			other.attachedRigidbody.velocity = Vector2.Dot (orthogonal, other.attachedRigidbody.velocity) * orthogonal;
			other.attachedRigidbody.AddForce(RepulsionForce * other.attachedRigidbody.mass * dir);
			other.attachedRigidbody.AddTorque(TorqueMagnitude * other.attachedRigidbody.mass * (Random.Range(0, 1) == 0 ? -1 : 1));
		}
	}

	IEnumerator DestroyPulse ()
	{
        yield return new WaitForSeconds(duration);
		pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.Pulse);
		//GameObject.Destroy (gameObject);
	}
}
