using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentGenerator : MonoBehaviour
{
    public GameObject Camera;

    public int NSectorX, NSectorY;
    public int SectorSizeX, SectorSizeY;

    public int StarRadiusMin, StarRadiusMax;
	public int StarMassMin, StarMassMax;
	public float StarToPlanetDistanceFactor;
	public float PlanetToPlanetDistanceFactor;

    public int PlanetRadiusMin, PlanetRadiusMax;
	public int PlanetMassMin, PlanetMassMax;
	public int PlanetAngularSpeedMin, PlanetAngularSpeedMax;

	public int MoonRadiusMin, MoonRadiusMax;

	public int AsteroidRadiusMax;

	public int AsteroidFieldMinRadius, AsteroidFieldMaxRadius;
	public int AsteroidFieldMinSize, AsteroidFieldMaxSize;

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

    //int count = 0;

    // Update is called once per frame
    void Update()
    {
        // Testing
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(count, count * 2, count * 3);
            int rnd = new System.Random(CalculatePositionId(pos)).Next(0, 1000);
            Debug.Log("Position: " + pos + " Random int assigned: " + rnd);
            //Debug.Log("Position: " + pos + " Random int assigned (second): " + rnd);
            ++count;
        }*/

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
		GeneratePlanets(sector, gen, minX, maxX, minY, maxY);
        // Asteroid fields
		GenerateAsteroidFields(sector, gen, minX, maxX, minY, maxY);
        // Asteroid cloud
		GenerateAsteroidCloud(sector, gen, minX, maxX, minY, maxY);
        // Debris

    }

	private void GenerateStars(GameObject sector, System.Random gen, int minX, int maxX, int minY, int maxY)
    {
        if (gen.Next(0, RandomNumberMax) < StarLikelihood)
        {
            // Choose position for star
			int starRadius = gen.Next(StarRadiusMin, StarRadiusMax);
			Vector3 starPos = new Vector3(gen.Next(minX + starRadius, maxX - starRadius), gen.Next(minY + starRadius, maxY - starRadius));
			int starMass = gen.Next(StarMassMin, StarMassMax);
            // Create star
            GameObject star = Instantiate(StarPrefab);
            // Make it a child of this sector
            star.transform.parent = sector.transform;
            // Select a sprite
            Sprite starSprite = StarSprites[gen.Next(0, StarSprites.Count)];
			// Define the orbits around the star
			List<int> orbits = DefineOrbits(sector, gen, starPos, starRadius);
			star.GetComponent<Star>().Create(starPos, starRadius, starMass, orbits, starSprite);
        }
    }


	private List<int> DefineOrbits(GameObject sector, System.Random gen, Vector3 starPos, int starRadius)
	{
		List<int> orbits = new List<int>();
		// Calculate distance from star to origin of sector
		float distBorder = Vector3.Distance (starPos, sector.transform.position);
		// Define orbit radius so that the planets wont collide with the star and so they dont go too far outside of the sector
		int minOrbit = starRadius + Mathf.CeilToInt(StarToPlanetDistanceFactor * PlanetRadiusMax);
		int maxOrbit = (int)distBorder - PlanetRadiusMax;
		// Only add planets if there is enough space between the star and the border of the sector
		if(minOrbit < maxOrbit)
		{
			// Keep generating orbits until the "spawn check" fails
			bool noSpace = false;
			while(gen.Next(0, RandomNumberMax) < PlanetLikelihood && !noSpace)
			{
				noSpace = true;
				for(int n = 0; n < MaxSpawnAttempts; ++n)
				{
					int newOrbit = gen.Next(minOrbit, maxOrbit);
					// Make sure planets inside the same planetary system dont collide with each other
					bool valid = true;
					foreach(int otherOrbit in orbits)
					{
						if((otherOrbit < newOrbit && otherOrbit + PlanetToPlanetDistanceFactor * PlanetRadiusMax > newOrbit - PlanetRadiusMax) || 
						   (otherOrbit > newOrbit && otherOrbit - PlanetToPlanetDistanceFactor * PlanetRadiusMax < newOrbit + PlanetRadiusMax))
						{
							valid = false;
							break;
						}
					}
					// If the orbit is valid, add it to the star's list of orbits
					if(valid)
					{
						orbits.Add(newOrbit);
						noSpace = false;
						break;
					}
				}
			}
		}
		return orbits;
	}
		
	private void GeneratePlanets(GameObject sector, System.Random gen, int minX, int maxX, int minY, int maxY)
	{
		// Find all objects with a collider in the sector
		// Default parameters: int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity
		Collider2D[] objectsInSector = Physics2D.OverlapAreaAll(new Vector2(minX, minY), new Vector2(maxX, maxY));
		// For every star, create planets to orbit around it
		foreach(Collider2D col in objectsInSector)
		{
			if(col.gameObject.tag == GlobalValues.StarTag)
			{
				Star star = col.gameObject.GetComponent<Star>();
				foreach(int orbit in star.OrbitRadii)
				{
					Vector3 planetDir = Random.insideUnitCircle;
					int planetRadius = gen.Next(PlanetRadiusMin, PlanetRadiusMax);
					int planetMass = gen.Next(PlanetMassMin, PlanetMassMax);
					int planetTranslationDir = gen.Next (0, 1) < 0.5f ? -1 : 1;
					int planetAngularSpeed = planetTranslationDir * gen.Next (PlanetAngularSpeedMin, PlanetAngularSpeedMax);
					Vector3 planetPos = star.transform.position + planetDir * orbit;
					// Create planet
					GameObject planet = Instantiate(PlanetPrefab);
					// Make it a child of this sector
					planet.transform.parent = sector.transform;
					// Select a sprite
					Sprite planetSprite = PlanetSprites[gen.Next(0, PlanetSprites.Count)];
					planet.GetComponent<Planet>().Create(planetPos, planetRadius, planetMass, col.gameObject, planetAngularSpeed, planetSprite);					
				}
			}
		}
	}

	private void GenerateAsteroidFields(GameObject sector, System.Random gen, int minX, int maxX, int minY, int maxY)
	{
		while(gen.Next(0, RandomNumberMax) < AsteroidFieldLikelihood)
		{
			// Select clear area
			Vector3 fieldCenter = Vector3.zero;
			int asteroidFieldRadius = 0;
			bool success = false;
			for(int n = 0; n < MaxSpawnAttempts; ++n)
			{
				fieldCenter = new Vector3(gen.Next(minX, maxX), gen.Next(minY, maxY));
				asteroidFieldRadius = gen.Next (AsteroidFieldMinRadius, AsteroidFieldMaxRadius);
				if(Physics2D.OverlapCircleAll(fieldCenter, asteroidFieldRadius).Length == 0)
				{
					success = true;
					break;
				}
			}
			// If the field could not be placed, assume the sector is overpopulated and dont place any more fields
			if(!success)
			{
				return;
			}
			// Create first generation
			int fieldSize = gen.Next (AsteroidFieldMinSize, AsteroidFieldMaxSize);
			for(int n = 0; n < fieldSize; ++n)
			{
				Vector3 asteroidPos = fieldCenter + (Vector3)(Random.insideUnitCircle * asteroidFieldRadius);
				if(Physics2D.OverlapCircleAll(asteroidPos, AsteroidRadiusMax).Length == 0)
				{
					// Create asteroid
					GameObject asteroid = Instantiate(AsteroidPrefab);
					// Make it a child of this sector
					asteroid.transform.parent = sector.transform;
					//asteroid.GetComponent<Asteroid>().Initialize(asteroidPos, AsteroidRadiusMax);
				}
			}
		}
	}

	private void GenerateAsteroidCloud(GameObject sector, System.Random gen, int minX, int maxX, int minY, int maxY)
	{
		while(gen.Next(0, RandomNumberMax) < GlobalAsteroidLikelihood)
		{
			Vector3 asteroidPos = new Vector3(gen.Next(minX, maxX), gen.Next(minY, maxY));
			if(Physics2D.OverlapCircleAll(asteroidPos, AsteroidRadiusMax).Length == 0)
			{
				// Create asteroid
				GameObject asteroid = Instantiate(AsteroidPrefab);
				// Make it a child of this sector
				asteroid.transform.parent = sector.transform;
				//asteroid.GetComponent<Asteroid>().Initialize(asteroidPos, AsteroidRadiusMax);
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

	private int CalculatePositionId(Vector3 position)
	{
		return (int)position.GetHashCode() * timeSeed;
	}

}
