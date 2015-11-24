using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteriodLevelController : MonoBehaviour {

    public AnimationCurve asteroidCurve = new AnimationCurve(new Keyframe(-0.5f, -0.35f), new Keyframe(0.7f, 0.7f));
    [Space(10)]
    public GameObject smallAsteroid;
    public GameObject mediumAsteroid;
    public GameObject largeAsteroid;
    [Space(10)]
    public int maxSmallAsteroids = 0;
    public int maxMediumAsteroids = 0;
    public int maxLargeAsteroids = 0;

    private List<GameObject> smallAstList = new List<GameObject>();
    private List<GameObject> medAstList = new List<GameObject>();
    private List<GameObject> largeAstList = new List<GameObject>();
    private Vector2 spawnLocation = Vector2.zero;

    // Use this for initialization
    void Start () {
        spawnLocation = new Vector2(-(transform.localScale.x * 0.6f), -(transform.localScale.y * 0.35f));
        InvokeRepeating("SpawnNewAsteroids", 0.0f, 0.1f);
	}
	
	// Update is called once per frame
	void SpawnNewAsteroids () {
        if (smallAstList.Count < maxSmallAsteroids)
            SpawnAsteroid(AsteroidType.Small);
        if (smallAstList.Count < maxMediumAsteroids)
            SpawnAsteroid(AsteroidType.Medium);
        if (smallAstList.Count < maxLargeAsteroids)
            SpawnAsteroid(AsteroidType.Large);
    }

    void SpawnAsteroid(AsteroidType type)
    {
        switch (type)
        {
            case AsteroidType.Small:
                break;
            case AsteroidType.Medium:
                break;
            case AsteroidType.Large:
                break;
            default:
                break;
        }
    }

    enum AsteroidType
    {
        Small = 0,
        Medium = 1,
        Large = 2
    };
}