using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Implosion : MonoBehaviour
{
    public float scaleFactor;
    public float duration;
    public AnimationCurve explosionAnim = new AnimationCurve(new Keyframe(0, 0.05f), new Keyframe(0.95f, 1), new Keyframe(1, 0.05f));
    [Space(10)]
    public float PullForce;
	//public AnimationCurve ForceMassSccalingCurve = new AnimationCurve(new Keyframe(0, 0.05f), new Keyframe(1, 1));
    public Material[] implosionMats;

    private float elapsedTime;
	private bool triggered;
	private ObjectPool pool;
	void Awake ()
	{
		pool = FindObjectOfType<ObjectPool>();
        //GetComponent<AudioSource> ().pitch = Random.Range (0.5f, 1.5f);
        Material mat = implosionMats[Random.Range(0, implosionMats.Length)];
        GetComponent<MeshRenderer>().material = mat;
        
    }

	void OnEnable()
	{
		triggered = false;
		elapsedTime = 0;
		StartCoroutine("DestroyImplosion");
	}

    void Update()
    {
        float newScale = explosionAnim.Evaluate(elapsedTime) * scaleFactor;
        transform.localScale = Vector3.one * newScale;
        elapsedTime += Time.deltaTime / duration;
		// Trigger implosion effect when the maximum size is reached
		if(elapsedTime >= explosionAnim.keys[1].time && !triggered)
		{
			Pull ();
			triggered = true;
		}
    }

	private void Pull()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius * transform.localScale.magnitude);
		List<GameObject> ships = new List<GameObject>();
		//Debug.Log ("Pos: " + transform.position + " Radius: " + GetComponent<CircleCollider2D>().radius * transform.localScale.magnitude);
		foreach(Collider2D col in cols)
		{
			if (col.attachedRigidbody != null) {
				if(col.attachedRigidbody.gameObject.GetComponent<Core>() != null)
				{
					if(!ships.Contains(col.attachedRigidbody.gameObject))
					{
						ships.Add (col.attachedRigidbody.gameObject);
						// Calculate pull force direction
						Vector3 dir = transform.position - col.transform.position;
						dir.Normalize ();
						col.attachedRigidbody.AddForce(dir * PullForce * col.attachedRigidbody.mass);
					}
				}
				else
				{
					// Calculate pull force direction
					Vector3 dir = transform.position - col.transform.position;
					dir.Normalize ();
					col.attachedRigidbody.AddForce(dir * PullForce * col.attachedRigidbody.mass);
				}

			}
		}
	}

    /*private void OnTriggerExit2D (Collider2D other)
	{
		if (other.attachedRigidbody != null) {
			// Calculate pull force direction
			Vector3 dir = transform.position - other.transform.position;
			dir.Normalize ();
			other.attachedRigidbody.AddForce(dir * PullForce * other.attachedRigidbody.mass);
		}
	}*/
	
	IEnumerator DestroyImplosion ()
	{
        yield return new WaitForSeconds(duration);
		pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.Implosion);
		//GameObject.Destroy (gameObject);
	}
}
