using UnityEngine;
using System.Collections;

public class EnvironmentGenerator : MonoBehaviour
{
    public GameObject Camera;

    public int QuadrantSizeX, QuadrantSizeY;
    public int StarRadiusMin, StarRadiusMax;
    public int PlanetRadiusMin, PlanetRadiusMax;
    public int MoonRadiusMin, MoonRadiusMax;

    public float StarDensity;
    public float PlanetDensity, MoonDensity;
    public float AsteroidFieldDensity, InFieldAsteroidDensity;
    public float GlobalAsteroidDensity;
    public float GlobalDebrisDensity;

    private int seed;

    // Use this for initialization
    void Start()
    {
        // Save seed
    }

    // Update is called once per frame
    void Update()
    {

    }
}
