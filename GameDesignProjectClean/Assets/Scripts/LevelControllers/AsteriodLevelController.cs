using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteriodLevelController : MonoBehaviour {

    public AnimationCurve asteroidCurve = new AnimationCurve(new Keyframe(-0.5f, -0.35f), new Keyframe(0.7f, 0.7f));
    public float speed = 0.0f;
    public float yOffsetRange = 0.0f;
    [Space(10)]
    public GameObject smallAsteroid;
    public GameObject mediumAsteroid;
    public GameObject largeAsteroid;
    [Space(10)]
    public int maxSmallAsteroids = 0;
    public int maxMediumAsteroids = 0;
    public int maxLargeAsteroids = 0;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
}