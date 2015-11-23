using UnityEngine;
using System.Collections;

public class Implosion : MonoBehaviour
{
    public float scaleFactor = 1.0f;
    public float duration = 1.0f;
    public AnimationCurve explosionAnim = new AnimationCurve(new Keyframe(0, 0.05f), new Keyframe(0.95f, 1), new Keyframe(1, 0.05f));
    [Space(10)]
    public float PullForce;
    public Material[] implosionMats;

    private float counter = 0.0f;

	void Awake ()
	{
        //GetComponent<AudioSource> ().pitch = Random.Range (0.5f, 1.5f);
        Material mat = implosionMats[Random.Range(0, implosionMats.Length)];
        GetComponent<MeshRenderer>().material = mat;
        StartCoroutine("DestroyImplosion");
    }

	void Start()
	{

	}

    void Update()
    {
        float newScale = explosionAnim.Evaluate(counter) * scaleFactor;
        transform.localScale = Vector3.one * newScale;
        counter += Time.deltaTime / duration;
    }

    private void OnTriggerExit2D (Collider2D other)
	{
		if (other.attachedRigidbody != null) {
			// Calculate pull force direction
			Vector3 dir = transform.position - other.transform.position;
			dir.Normalize ();
			other.attachedRigidbody.AddForce(dir * PullForce);
		}
	}
	
	IEnumerator DestroyImplosion ()
	{
        yield return new WaitForSeconds(duration);
		GameObject.Destroy (gameObject);
	}
}
