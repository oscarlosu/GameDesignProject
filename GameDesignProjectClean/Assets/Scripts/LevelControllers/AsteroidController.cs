using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour
{

	private bool active = false;
	private Vector3 perpVec;
	private Vector3 newPos;
	private float count = 1.0f;

	private AsteriodLevelController alc;
	private float offsetFromPath;

	// Use this for initialization
	void Awake ()
	{
		AsteriodLevelController alc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<AsteriodLevelController> ();  
		offsetFromPath = Vector3.Distance (GetPositionInPath(float t))
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!active)
		{
			posOnLine.x += (Time.deltaTime * speed);
			if (posOnLine.x > playfieldSizeX * 0.6f)
				posOnLine.x = -(playfieldSizeX * 0.6f);

			SetNewPos ();
		}
	}

	Vector2 GetPositionInPath(float t)
	{
		return new Vector2(t, alc.asteroidCurve.Evaluate(t));
	}

	void SetNewPos ()
	{
		newPos = new Vector3 (posOnLine.x, (asteroidCurve.Evaluate (posOnLine.x / playfieldSizeX) * playfieldSizeY), 0);
		newPos += perpVec;
		count += Time.deltaTime;
		if (count >= 0.5f) {
			UpdatePerpVec (newPos);
			count = 0.0f;
		}
		transform.position = newPos;
	}

	void UpdatePerpVec (Vector3 np)
	{
		Vector3 dir = np - transform.position;

		Debug.DrawRay (transform.position, dir * 100f, Color.red, 0.5f);

		perpVec = Vector3.Cross (dir, Vector3.forward);
		perpVec.Normalize ();
		Debug.DrawRay (transform.position, perpVec, Color.blue, 0.5f);
		perpVec *= yOffset;
	}
}