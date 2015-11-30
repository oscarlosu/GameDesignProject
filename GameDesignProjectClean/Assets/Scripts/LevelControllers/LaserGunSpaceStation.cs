using UnityEngine;
using System.Collections;

public class LaserGunSpaceStation : MonoBehaviour {

    public bool isMe = true;
    public GameObject laserbeam;


    void FireLaserGun(Vector2 laserSize)
    {
        if (isMe)
        {
            //Debug.Log("Activated!");
            // Instantiate laser
            GameObject l = (GameObject)GameObject.Instantiate(laserbeam, transform.position, transform.rotation);
            //GameObject laser = (GameObject)Instantiate(LaserPrefab, transform.position, transform.rotation);
            //laser.transform.parent = transform;
            // Calculate breadth and length of laser and scale
            Vector2 scale = laserSize;
            l.transform.localScale = scale;
            l.transform.position += l.transform.up * 0.5f;
            l.transform.parent = transform;
            isMe = false;
        }
        else
            isMe = true;
    }
}
