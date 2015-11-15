using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class CameraHandler : MonoBehaviour
{
    // Zoom to fit params
    public float Margin;
    public float MaxSize;
    public float MinSize;
    // Warning phase params
    public float CameraShakeMagnitude;
    public float WarningDuration;
    // Shrink phase params    
    public float PushOffsetFromCenter;
    public float PushForceFactor;
    public float PushTorqueMagnitude;
    public float ShrinkSpeed;
    public float ShrinkExtraDistance;
    public float EarlyPushFactor;

    private List<GameObject> ships = new List<GameObject>();
    private Camera camera;

    private enum CameraState
    {
        ZoomToFit,
        Warning,
        Shrink
    }
    private CameraState state;
    private float elapsedTime;


    // Use this for initialization
    void Start()
    {
        camera = GetComponent<Camera>();
        ships = GameObject.FindGameObjectsWithTag(GlobalValues.ShipTag).OfType<GameObject>().ToList();
        state = CameraState.ZoomToFit;
        elapsedTime = 0;

        CenterAndZoomToFit();
    }

    // Update is called once per frame
    void Update()
    {
		ships = GameObject.FindGameObjectsWithTag(GlobalValues.ShipTag).OfType<GameObject>().ToList();
        UpdateState();
        switch(state)
        {
            case CameraState.ZoomToFit:
                elapsedTime = 0;
                CenterAndZoomToFit();
                break;
            case CameraState.Warning:
                elapsedTime += Time.deltaTime;
                CenterAndZoomToFit();
                ShakeCamera();                
                break;
            case CameraState.Shrink:
                Shrink();
                break;
        }
        KeepShipsInsideViewport();

    }

    private void UpdateState()
    {
        switch(state)
        {
            case CameraState.ZoomToFit:
                if(camera.orthographicSize > MaxSize)
                {
                    state = CameraState.Warning;
                }
                break;
            case CameraState.Warning:
                if(camera.orthographicSize <= MaxSize)
                {
                    state = CameraState.ZoomToFit;
                }
                else if(elapsedTime > WarningDuration)
                {
                    state = CameraState.Shrink;
                }
                break;
            case CameraState.Shrink:
                if (camera.orthographicSize <= MaxSize - ShrinkExtraDistance)
                {
                    state = CameraState.ZoomToFit;
                }
                break;
        }
    }

    

    private void CenterAndZoomToFit()
    {
        // Update the orthographic size of the camera
        float targetSize = GetMaxAxisDistanceToCamera() + Margin;
        camera.orthographicSize = targetSize;
        // Update position of the camera
        Vector3 targetCenter = CameraTargetPosition();
        camera.transform.position = new Vector3(targetCenter.x, targetCenter.y, camera.transform.position.z);
        
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
            return camera.transform.position;
        }
    }

    private void ShakeCamera()
    {
        Vector3 shakeOffset = Random.insideUnitCircle * Mathf.Lerp(0, CameraShakeMagnitude, elapsedTime / WarningDuration);
        camera.transform.position += shakeOffset;
    }

    private float GetMaxAxisDistanceToCamera()
    {
        float max = - Mathf.Infinity;
        for(int index = 0; index < ships.Count; ++index)
        {
            if (ships[index] != null)
            {
                float val = Mathf.Max(Mathf.Abs(Mathf.Abs(ships[index].transform.position.x) - Mathf.Abs(camera.transform.position.x)),
                                  Mathf.Abs(Mathf.Abs(ships[index].transform.position.y) - Mathf.Abs(camera.transform.position.y)));
                if (val > max)
                {
                    max = val;
                }
            }
            else
            {
                ships.RemoveAt(index);
            }
        }
        return Mathf.Clamp(max, MinSize, Mathf.Infinity);
    }

    private void Shrink()
    {
        // Reduce orthographic size of camera
        camera.orthographicSize -= ShrinkSpeed;
        // Update position of the camera
        Vector3 targetCenter = CameraTargetPosition();
        camera.transform.position = new Vector3(targetCenter.x, targetCenter.y, camera.transform.position.z);
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
        float cameraHalfWidth = camera.aspect * camera.orthographicSize;
        return Mathf.Abs(ship.transform.position.x - camera.transform.position.x) >= cameraHalfWidth * EarlyPushFactor ||
               Mathf.Abs(ship.transform.position.y - camera.transform.position.y) >= camera.orthographicSize * EarlyPushFactor;
    }

    private void ThrowShip(GameObject ship)
    {
        // Set velocity of ship to zero
        ship.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ship.GetComponent<Rigidbody2D>().angularVelocity = 0;
        // Add force roughly towaards the center of the viewport
        Vector3 offsetFromCenter = Random.insideUnitCircle * PushOffsetFromCenter;
        Vector3 squeezeDest = camera.transform.position;
        squeezeDest += offsetFromCenter;
        Vector2 direction = squeezeDest - ship.transform.position;
        direction.Normalize();
        ship.GetComponent<Rigidbody2D>().AddForce(direction * PushForceFactor * camera.orthographicSize);
        // Add torque
        // Positive rotation (counterclockwise)
        if (Random.value >= 0.5f)
        {
            ship.GetComponent<Rigidbody2D>().AddTorque(Random.Range(0.5f, 1) * PushTorqueMagnitude, ForceMode2D.Impulse);
        }
        // Negative rotation (clockwise)
        else
        {
            ship.GetComponent<Rigidbody2D>().AddTorque(-Random.Range(0.5f, 1) * PushTorqueMagnitude, ForceMode2D.Impulse);
        }
    }    
}
