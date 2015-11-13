using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentGenerator : MonoBehaviour
{
    public GameObject Camera;

    public int NSectorX, NSectorY;
    public int SectorSizeX, SectorSizeY;
    public int StarRadiusMin, StarRadiusMax;
    public int PlanetRadiusMin, PlanetRadiusMax;
    public int MoonRadiusMin, MoonRadiusMax;

    public float StarLikelihood;
    public float PlanetLikelihood, MoonLikelihood;
    public float AsteroidFieldLikelihood, InFieldAsteroidLikelihood;
    public float GlobalAsteroidLikelihood;
    public float GlobalDebrisLikelihood;

    public GameObject StarPrefab, PlanetPrefab, MoonPrefab, AsteroidPrefab, DebrisPrefab;
    public List<Sprite> StarSprites, PlanetSprites, MoonSprites, AsteroidSmallSprites, AsteroidMediumSprites, AsteroidLargeSprites, DebrisSprites;

    public int RandomNumberMax;
    public int MaxSpawnAttempts;
    

    private int timeSeed;

    private Dictionary<Vector2, GameObject> sectors = new Dictionary<Vector2, GameObject>();

    // Use this for initialization
    void Start()
    {
        // Save seed
        timeSeed = (int)System.DateTime.Now.Ticks.GetHashCode();
        Initialize();

    }

    int CalculatePositionId(Vector3 position)
    {
        return (int)position.GetHashCode() * timeSeed;
    }



    int count = 0;

    // Update is called once per frame
    void Update()
    {
        // Testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(count, count * 2, count * 3);
            int rnd = new System.Random(CalculatePositionId(pos)).Next(0, 1000);
            Debug.Log("Position: " + pos + " Random int assigned: " + rnd);
            //Debug.Log("Position: " + pos + " Random int assigned (second): " + rnd);
            ++count;
        }


        // Handle sector generation

        // Handle sector destruction

    }

    public void Initialize()
    {
        // Create sectors
        for(int x = 0; x < NSectorX; ++x)
        {
            for (int y = 0; y < NSectorY; ++y)
            {
                // Create sector, set this object as its parent and add it to the dictionary with its x,y grid coords as key
                GameObject sector = new GameObject("Sector");
                sector.transform.parent = transform;
                sectors.Add(new Vector2(x, y), sector);
                // Set the sectors position
                sector.transform.position = new Vector3(x * SectorSizeX, y * SectorSizeY, 0);
                // Generate environment inside that sector
                GenerateSector(sector);
            }
        }

    }

    void GenerateSector(GameObject sector)
    {
        // Calculate position bounds for sector
        int minX = (int)sector.transform.position.x;
        int maxX = minX + SectorSizeX;
        int minY = (int)sector.transform.position.y;
        int maxY = minY + SectorSizeY;
        // Get random number generator seeded with sector id
        System.Random gen = new System.Random(CalculatePositionId(sector.transform.position));
        // Stars
        GenerateStars(sector, gen, minX, maxX, minY, maxY);
        // Planets

        // Asteroid fields

        // Asteroids

        // Debris

    }

    void GenerateStars(GameObject sector, System.Random gen, int minX, int maxX, int minY, int maxY)
    {
        if (gen.Next(0, RandomNumberMax) < StarLikelihood)
        {
            // Choose position for star
            Vector3 starPos;
            int starRadius;
            for (int n = 0; n < MaxSpawnAttempts; ++n)
            {
                starPos = new Vector3(gen.Next(minX, maxX), gen.Next(minY, maxY));
                starRadius = gen.Next(StarRadiusMin, StarRadiusMax);
                // Check if the star is guaranteed to be completely inside of the sector
                if (starPos.x + StarRadiusMax < maxX && starPos.x - StarRadiusMax > minX &&
                   starPos.y + StarRadiusMax < maxY && starPos.y - StarRadiusMax > minY)
                {
                    // Create star
                    GameObject star = Instantiate(StarPrefab);
                    // Make it a child of this sector
                    star.transform.parent = sector.transform;
                    // Select a sprite
                    Sprite starSprite = StarSprites[gen.Next(0, StarSprites.Count)];
                    star.GetComponent<Star>().Create(starPos, starRadius, starSprite);
                    break;
                }
            }
        }
    }

}
