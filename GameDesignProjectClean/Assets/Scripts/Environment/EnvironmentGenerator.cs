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
	private int firstX;
	private int firstY;

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

		// Calculate new first sector grid position
		int newFirstX = (int)(Camera.transform.position.x / SectorSizeX) - Mathf.CeilToInt (NSectorX / 2);
		int newFirstY = (int)(Camera.transform.position.y / SectorSizeY) - Mathf.CeilToInt (NSectorY / 2);
        // Update sector positions
		// Move right
		if(newFirstX > firstX)
		{
			for(int x = firstX; x < newFirstX; ++x)
			{
				MoveColumn (x, x + NSectorX);
			}
		}
		// Move left
		if(newFirstX < firstX)
		{
			for(int x = firstX + NSectorX - 1; x >= newFirstX + NSectorX; --x)
			{
				MoveColumn (x, x - NSectorX);
			}
		}
		// Update first x grid coordinate
		firstX = newFirstX;
		// Move up
		if(newFirstY > firstY)
		{
			for(int y = firstY; y < newFirstY; ++y)
			{
				MoveRow (y, y + NSectorY);
			}
		}
		// Move down
		if(newFirstY < firstY)
		{
			for(int y = firstY + NSectorY - 1; y >= newFirstY + NSectorY; --y)
			{
				MoveRow (y, y - NSectorY);
			}
		}
		// Update first y grid coordinate
		firstY = newFirstY;

		// Handle sector generation
		
        // Handle sector destruction

    }

	private void MoveColumn(int fromX, int toX)
	{
		Vector2 fromPos = new Vector2(fromX, 0);
		Vector2 toPos = new Vector2(toX, 0);
		for(int y = firstY; y < firstY + NSectorY; ++y)
		{
			fromPos.y = y;
			toPos.y = y;
			// Get sector from the sector dictionary
			GameObject sector = sectors[fromPos];
			// Remove sector from the dictionary
			sectors.Remove(fromPos);
			// Delete all the environment objects in the sector
			ClearSector (sector);
			// Update sector position
			sector.transform.position = new Vector3(toPos.x * SectorSizeX, toPos.y * SectorSizeY, 0);
			// Add sector to the dictionary with the new grid position as the key
			sectors.Add (toPos, sector);
			// Regenerate sector
			GenerateSector(sector);
		}
	}

	private void MoveRow(int fromY, int toY)
	{
		Vector2 fromPos = new Vector2(0, fromY);
		Vector2 toPos = new Vector2(0, toY);
		for(int x = firstX; x < firstX + NSectorX; ++x)
		{
			fromPos.x = x;
			toPos.x = x;
			// Get sector from the sector dictionary
			GameObject sector = sectors[fromPos];
			// Remove sector from the dictionary
			sectors.Remove(fromPos);
			// Delete all the environment objects in the sector
			ClearSector (sector);
			// Update sector position
			sector.transform.position = new Vector3(toPos.x * SectorSizeX, toPos.y * SectorSizeY, 0);
			// Add sector to the dictionary with the new grid position as the key
			sectors.Add (toPos, sector);
			// Regenerate sector
			GenerateSector(sector);
		}
	}

    private void Initialize()
    {
		// Set the first sector gird position
		firstX = (int)(Camera.transform.position.x / SectorSizeX) - Mathf.CeilToInt (NSectorX / 2);
		firstY = (int)(Camera.transform.position.y / SectorSizeY) - Mathf.CeilToInt (NSectorY / 2);
        // Create sectors
        for(int x = firstX; x < firstX + NSectorX; ++x)
        {
            for (int y = firstY; y < firstY + NSectorY; ++y)
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

	private void ClearSector(GameObject sector)
	{
		for(int n = sector.transform.childCount - 1; n >= 0; --n)
		{
			Destroy (sector.transform.GetChild (n).gameObject);
		}
	}

	private void GenerateSector(GameObject sector)
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

	private void GenerateStars(GameObject sector, System.Random gen, int minX, int maxX, int minY, int maxY)
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
