using UnityEngine;
using System.Collections;

public class LaserGunSpaceStation : MonoBehaviour {

    public bool isMe = true;
    protected ObjectPool pool;

    protected void OnEnable()
    {
        pool = GameObject.FindGameObjectWithTag(GlobalValues.ObjectPoolTag).GetComponent<ObjectPool>();
    }

    void FireLaserGun(Vector2 laserSize)
    {
        if (isMe)
        {
            //Debug.Log("Activated!");
            // Instantiate laser
            GameObject laser = pool.RequestPoolObject(ObjectPool.ObjectType.Laser, transform.position, transform.rotation);
            //GameObject laser = (GameObject)Instantiate(LaserPrefab, transform.position, transform.rotation);
            //laser.transform.parent = transform;
            // Calculate breadth and length of laser and scale
            Vector2 scale = laserSize;
            laser.transform.localScale = scale;
            laser.transform.position += laser.transform.up * 0.5f;
            isMe = false;
        }
        else
            isMe = true;
    }
}
