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
	public float Radius;
	public float Speed;

	private CircleCollider2D col;
    public Material[] pulseMats;

    private float counter = 0.0f;
    private Material mat;
    // Use this for initialization
    void Start ()
	{
		col = GetComponent<CircleCollider2D>();
        mat = pulseMats[Random.Range(0, pulseMats.Length)];
        GetComponent<MeshRenderer>().material = mat;
        mat = GetComponent<MeshRenderer>().material;
        InGrace = true;
        StartCoroutine("DestroyPulse");
    }
	
	// Update is called once per frame
	void Update ()
	{
        mat.color  = new Color(mat.color.r, mat.color.g, mat.color.b, 1 - explosionAnim.Evaluate(counter));
        float newScale = explosionAnim.Evaluate(counter) * scaleFactor;
        transform.localScale = Vector3.one * newScale;
        counter += Time.deltaTime / duration;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Pulse detected OnTriggerEnter2D with " + other.gameObject.name);
		if(!other.isTrigger && other.attachedRigidbody != null && other.attachedRigidbody.gameObject.GetInstanceID() != SourceCore.GetInstanceID())
		{
			// Cancel velocity in the direction of the shield device and apply force to rigidbody
			Vector2 dir = other.attachedRigidbody.transform.position - transform.position;
			dir.Normalize();
			// Calculate orthogonal vector to dir
			Vector2 orthogonal = new Vector2(- dir.y, dir.x);
			// Project velocity on orthogonal direction
			other.attachedRigidbody.velocity = Vector2.Dot (orthogonal, other.attachedRigidbody.velocity) * orthogonal;
			other.attachedRigidbody.AddForce(RepulsionForce * dir);
			other.attachedRigidbody.AddTorque(TorqueMagnitude * (Random.Range(0, 1) == 0 ? -1 : 1));
		}
	}

	IEnumerator DestroyPulse ()
	{
        yield return new WaitForSeconds(duration);
		GameObject.Destroy (gameObject);
	}
}
