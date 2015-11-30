using UnityEditor;
using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour
{
	public AsteriodLevelController LvlController;

	[SerializeField]
	private float posInPath;
	[SerializeField]
	private float offsetFromPath;
	private bool followingPath;
	private float speed;
	public float PathLength {get; set;}

	public float MaxAngularVelocity;

	private Rigidbody2D rb;

	public void SetPosInPath(float newPos)
	{
		// Set posInPath
		posInPath = newPos;
		// Update position of object
		MoveInPath();
	}

	public float GetPosInPath()
	{
		return posInPath;
	}

	public void SetOffsetFromPath(float newOffset)
	{
		// Set setOffsetInPath
		offsetFromPath = newOffset;
		// Update position of object
		MoveInPath();
	}
	
	public float GetOffsetFromPath()
	{
		return offsetFromPath;
	}

	private void MoveInPath()
	{
		float t = posInPath / PathLength;
		Vector3 dir = LvlController.Path.GetDirection(t);
		Vector3 orthDir = new Vector3(- dir.y, dir.x, 0);
		transform.position = LvlController.Path.GetPoint(t) + orthDir * offsetFromPath;
	}

	void Awake()
	{
		speed = LvlController.Speed;
		PathLength = LvlController.GetPathLength();
		followingPath = true;
		rb = GetComponent<Rigidbody2D>();
		// Set angular velocity to zero
		rb.angularDrag = 0;
		//  Add random torque and reduce angular drag to zero
		rb.angularVelocity = (Random.Range (0, 2) == 0 ? -1 : 1) * Random.Range (0, MaxAngularVelocity);
	}

	void Update ()
	{
		if (followingPath)
		{
			// If the object gets too close to the end of the path, teleport it back to the start
			if (Mathf.Abs (posInPath - PathLength) < LvlController.AsteroidRespawnDist)
			{
				posInPath = 0;
				MoveInPath();
			}
			else
			{
				posInPath += Time.deltaTime * speed;
				MoveInPath();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		followingPath = false;
		// Set velocity according to the position in the path
		float t = posInPath / PathLength;
		rb.velocity = LvlController.Path.GetDirection(t) * speed;
		// Reset angular drag
		rb.angularDrag = GlobalValues.DefaultAngularDrag;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		followingPath = false;
		// Set velocity according to the position in the path
		float t = posInPath / PathLength;
		rb.velocity = LvlController.Path.GetDirection(t) * speed;
		// Reset angular drag
		rb.angularDrag = GlobalValues.DefaultAngularDrag;
	}
}
	

/****************
* Editor tools.
****************/

[CustomEditor(typeof(PathFollower), true)]
public class PathFollowerEditor : Editor
{	
	protected new void DrawCustomInspector()
	{
		PathFollower asteroid = (PathFollower)target;
		if(asteroid.LvlController == null)
		{
			asteroid.LvlController = FindObjectOfType<AsteriodLevelController>();
		}
		asteroid.LvlController = (AsteriodLevelController)EditorGUILayout.ObjectField("LvlController", asteroid.LvlController, typeof(AsteriodLevelController));

		asteroid.MaxAngularVelocity = EditorGUILayout.FloatField("MaxAngularVelocity", asteroid.MaxAngularVelocity);
		if(asteroid.LvlController != null)
		{
			// Update path length
			asteroid.PathLength = asteroid.LvlController.GetPathLength();
			// Use pos in path to update the position of the object
			asteroid.SetPosInPath(Mathf.Clamp (EditorGUILayout.FloatField("Position in path", asteroid.GetPosInPath()),
			                                   0, asteroid.LvlController.GetPathLength()));
			
			// Use offset from path to update the postion of the object
			asteroid.SetOffsetFromPath(EditorGUILayout.FloatField("Offset from path", asteroid.GetOffsetFromPath()));
			// If the target was changed, set the target to dirty, so Unity will save the values.
			if (GUI.changed)
			{				
				EditorUtility.SetDirty(target);
			}
		}
	}
	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();
		DrawCustomInspector();
	}
}