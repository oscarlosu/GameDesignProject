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
	private bool active;
	private float speed;
	public float PathLength {get; set;}

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
		active = false;
	}

	void Update ()
	{
		if (!active)
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
		active = true;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		active = true;
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