using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteriodLevelController : MonoBehaviour
{
	public BezierSpline Path;

	public float Speed;
	public float AsteroidRespawnDist;


	public float GetPathLength()
	{
		return Vector3.Distance (Path.GetControlPoint(0), Path.GetControlPoint(Path.ControlPointCount - 1));
	}


    // Use this for initialization
    void Start ()
	{

	}
}