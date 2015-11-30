using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	public List<GameObject> ObjectPrefabs;
	public List<int> ObjectPoolsDefaultSize;
	public int HardSizeLimit;
	private Dictionary<ObjectType, Stack<GameObject>> objectPools;
	private int objectCount;

	public enum ObjectType
	{
		Rocket = 0,
		Missile = 1,
		Mine = 2,
		ImplosionBomb = 3,
		Explosion = 4,
		Implosion = 5,
		Pulse = 6,
		Laser = 7
	}

	// Use this for initialization
	void Start ()
	{
		/*System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
		watch.Start ();*/
		// Initialize pools
		objectPools = new Dictionary<ObjectType, Stack<GameObject>>();
		objectCount = 0;
		foreach(ObjectType type in System.Enum.GetValues(typeof(ObjectType)))
		{
			objectPools.Add(type, new Stack<GameObject>());
			// Initialize with the default amount of game objects
			for(int i = 0; i < ObjectPoolsDefaultSize[(int)type]; ++i)
			{
				GameObject obj = Instantiate (ObjectPrefabs[(int)type]);
				obj.SetActive(false);
				obj.transform.parent = transform;
				objectPools[type].Push (obj);
			}
			objectCount += ObjectPoolsDefaultSize[(int)type];
		}
		/*watch.Stop ();
		Debug.Log ("Pool initialization required: " + watch.ElapsedMilliseconds + " ms");
		Debug.Log ("ObjectCount: " + objectCount);*/
		InvokeRepeating("ReportStatus", 0, 10);
		//Debug.Log ("Pool says: " + gameObject.GetInstanceID());

	}

	public GameObject RequestPoolObject(ObjectType type, Vector3 position, Quaternion rotation)
	{
		if(objectPools[type].Count > 0)
		{
			// Get object of the specified kind from the pool
			GameObject obj = objectPools[type].Pop();
			// Set the specified position and rotation
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			// Enable the object
			obj.SetActive(true);
			return obj;
		}
		else if (objectCount < HardSizeLimit)
		{
			// Create new object of the specified type and set it as a child of the pool game object
			GameObject obj = Instantiate (ObjectPrefabs[(int)type]);
			obj.transform.parent = transform;
			// Set the specified position and rotation
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			// Update object count
			++objectCount;
			return obj;
		}
		else
		{
			throw new System.Exception("Object Pool hard size limit reached. No more pool objects will be instantiated.");
		}
	}

	public void DisablePoolObject(GameObject obj, ObjectType type)
	{
		// Disable the object
		obj.SetActive(false);
		// Put it in the pool for this kind of object
		objectPools[type].Push(obj);
	}

	private void ReportStatus()
	{
		string status = "Pool status:\n";
		foreach(KeyValuePair<ObjectType, Stack<GameObject>> pair in objectPools)
		{
			status += pair.Key.ToString() + " pool has " + pair.Value.Count + " objects\n";
		}
		Debug.LogFormat(status);
		Debug.Log ("Size: " + objectCount);

	}

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
