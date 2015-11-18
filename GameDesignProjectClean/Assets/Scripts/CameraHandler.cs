using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class CameraHandler : MonoBehaviour
{
    // Zoom to fit params
	public float MaxSize;
	public float MinSize;
    public float InsideMargin;
	public float OutsideMargin;    

	public float PushInForce;
    public float PushInTorque;
	public float ThrusterDisableTime;

	public AnimationCurve CameraScaleGraph = new AnimationCurve() { keys = new Keyframe[] { new Keyframe(0, 0), new Keyframe(20, 20) } };

    private List<GameObject> ships = new List<GameObject>();
    private Camera cam;


    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
		CenterAndZoomToFit();
    }

    // Update is called once per frame
    void Update()
    {
		// Update the ship list
		ships = GameObject.FindGameObjectsWithTag(GlobalValues.ShipTag).OfType<GameObject>().ToList();
		// Only zoom to fit if the camera size is below the maximum size
		CenterAndZoomToFit();
		KeepShipsInsideViewport();
    }

    private void CenterAndZoomToFit()
    {
        // Update the orthographic size of the camera
		float maxAxisDistance = GetMaxAxisDistanceToCamera() + InsideMargin;
		cam.orthographicSize = CameraScaleGraph.Evaluate(maxAxisDistance);
		//cam.orthographicSize = Mathf.Clamp (maxAxisDistance, MinSize, MaxSize);
		//cam.orthographicSize = Mathf.Lerp (cam.orthographicSize, maxAxisDistance, Time.deltaTime * LerpingFactor);
        // Update position of the camera
        Vector3 targetCenter = CameraTargetPosition();
		targetCenter.z = cam.transform.position.z;
		cam.transform.position = Vector3.Lerp(cam.transform.position, targetCenter, Time.deltaTime);
        
    }

    private Vector3 CameraTargetPosition()
    {
        // The targert position for the camera is the average of the positions of the ships
        int counter = 0;
        Vector3 avg = Vector3.zero;
        for (int i = 0; i < ships.Count; ++i)
        {
            // Check if the ship has been destroyed
            if (ships[i] != null)
            {
                // Keep count of how many ships are still in the scene
                ++counter;
                // Add positions
                avg += ships[i].transform.position;
            }
        }
        // If there are no ships, we keep the current position of the camera
        if (counter > 0)
        {
            return avg / counter;
        }
        else
        {
            return cam.transform.position;
        }
    }

    private float GetMaxAxisDistanceToCamera()
    {
		Vector2 max = new Vector2(MinSize, MinSize);
        for(int index = 0; index < ships.Count; ++index)
        {
            if (ships[index] != null)
            {
                float valX = Mathf.Abs(Mathf.Abs(ships[index].transform.position.x) - Mathf.Abs(cam.transform.position.x));
				float valY = Mathf.Abs(Mathf.Abs(ships[index].transform.position.y) - Mathf.Abs(cam.transform.position.y));
                if (valX > max.x)
                {
                    max.x = valX;
                }
				if (valY > max.y)
				{
					max.y = valY;
				}
            }
            else
            {
                ships.RemoveAt(index);
            }
        }
		// Scale x axis with the aspect ratio
		max.x = max.x / cam.aspect;
		// Take the greatest value
		float targetSize;
		if(max.x > max.y)
		{
			targetSize = max.x;
		}
		else
		{
			targetSize = max.y;
		}
		return targetSize;
    }

    private void KeepShipsInsideViewport()
    {
        // Any ship outside of the viewport is thrown towards the center             
        foreach (GameObject ship in ships)
        {
            if (IsShipOutsideViewport(ship))
            {
                ThrowShip(ship);
            }
        }
    }

    private bool IsShipOutsideViewport(GameObject ship)
    {
        float cameraHalfWidth = cam.aspect * cam.orthographicSize;
        return Mathf.Abs(ship.transform.position.x - cam.transform.position.x) >= cameraHalfWidth + OutsideMargin ||
               Mathf.Abs(ship.transform.position.y - cam.transform.position.y) >= cam.orthographicSize + OutsideMargin;
    }

    private void ThrowShip(GameObject ship)
    {
        // Set velocity of ship to zero
        ship.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ship.GetComponent<Rigidbody2D>().angularVelocity = 0;
        // Add force towards the center of the viewport
		Vector2 direction = cam.transform.position - ship.transform.position;
        direction.Normalize();
        ship.GetComponent<Rigidbody2D>().AddForce(direction * PushInForce);
        // Add torque
        // Positive rotation (counterclockwise)
        if (Random.value >= 0.5f)
        {
            ship.GetComponent<Rigidbody2D>().AddTorque(Random.Range(0.5f, 1) * PushInTorque, ForceMode2D.Impulse);
        }
        // Negative rotation (clockwise)
        else
        {
            ship.GetComponent<Rigidbody2D>().AddTorque(-Random.Range(0.5f, 1) * PushInTorque, ForceMode2D.Impulse);
        }
		// Disable ship's thrusters for DisableTime seconds
		Thruster[] thrusters = ship.GetComponentsInChildren<Thruster>();
		foreach(Thruster t in thrusters)
		{
			StartCoroutine(t.Disable(ThrusterDisableTime));
		}

    }    
}
