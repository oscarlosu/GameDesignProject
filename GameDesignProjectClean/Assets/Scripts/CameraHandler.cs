using UnityEngine;
using System.Collections.Generic;


public class CameraHandler : MonoBehaviour
{
    public float Margin;
    public float MaxSize;
    public float SqueezeForce;



    private GameObject[] ships;
    private Camera camera;

    // Use this for initialization
    void Start()
    {
        camera = GetComponent<Camera>();
        // Find all Ships in the scene
        ships = GameObject.FindGameObjectsWithTag(GlobalValues.ShipTag);
    }

    // Update is called once per frame
    void Update()
    {
        CenterAndZoomToFit();      

        if (camera.orthographicSize > MaxSize)
        {
            Squeeze();
        }
    }

    void Squeeze()
    {
        
        // Push any ship that is too close to the border of the camera towards the center
        foreach(GameObject ship in ships)
        {
            if(IsTooClose(ship))
            {
                Vector2 direction = camera.transform.position - ship.transform.position;
                direction.Normalize();
                ship.GetComponent<Rigidbody2D>().AddForce(direction * SqueezeForce);
            }
        }
    }

    bool IsTooClose(GameObject go)
    {
        float cameraHalfWidth = camera.aspect * camera.orthographicSize;
        return go.transform.position.x < camera.transform.position.x - cameraHalfWidth + 2 * Margin ||
               go.transform.position.x > camera.transform.position.x + cameraHalfWidth - 2 * Margin ||
               go.transform.position.y < camera.transform.position.y - camera.orthographicSize + 2 * Margin ||
               go.transform.position.y > camera.transform.position.y + camera.orthographicSize - 2 * Margin;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OntriggerEnter");
        if (other.attachedRigidbody != null && other.attachedRigidbody.gameObject.tag == GlobalValues.ShipTag)
        {
            Vector2 direction = camera.transform.position - other.attachedRigidbody.gameObject.transform.position;
            direction.Normalize();
            other.attachedRigidbody.AddForce(direction * SqueezeForce);
        }
    }




    private void CenterAndZoomToFit()
    {
        if (ships.Length > 0)
        {
            // Center and zoom to fit
            Vector3 center = Vector3.zero;
            int counter = 0;
            Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity), max = new Vector2(- Mathf.Infinity, - Mathf.Infinity);
            for (int i = 0; i < ships.Length; ++i)
            {
                // Check if the ship has been destroyed
                if (ships[i] != null)
                {
                    // Keep count of how many ships are still in the scene
                    ++counter;
                    // Get the minimum and maximum coordinates
                    if (ships[i].transform.position.x < min.x)
                        min.x = ships[i].transform.position.x;
                    if (ships[i].transform.position.x > max.x)
                        max.x = ships[i].transform.position.x;
                    if (ships[i].transform.position.y < min.y)
                        min.y = ships[i].transform.position.y;
                    if (ships[i].transform.position.y > max.y)
                        max.y = ships[i].transform.position.y;
                    // Add positions
                    center += ships[i].transform.position;
                }
            }
            // Calculate the average and update the target position of the camera
            center /= counter;

            Vector3 targetCenter;
            targetCenter.x = center.x;
            targetCenter.y = center.y;
            targetCenter.z = transform.position.z;

            // Update the target size of the camera

            float targetSize = Vector2.Distance(min, max) / 2 + Margin;

            camera.transform.position = targetCenter;
            camera.orthographicSize = targetSize;

        }
    }
}
