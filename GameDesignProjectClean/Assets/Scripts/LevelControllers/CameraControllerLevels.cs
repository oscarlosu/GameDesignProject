﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CameraControllerLevels : MonoBehaviour {
    // Zoom to fit params
    public float MinSize;
    public float InsideMargin;
    public float camDelay;

    public float OutsideMargin;

    public float PushInForce;
    public float PushInTorque;
    public float ThrusterDisableTime;

    public float camBufferX = 0.0f;
    public float camBufferY = 0.0f;

    public AnimationCurve CameraScaleGraph = new AnimationCurve() { keys = new Keyframe[] { new Keyframe(0, 15), new Keyframe(50, 50) } };

    private List<GameObject> ships = new List<GameObject>();
    private Camera cam;
    private int currentShips = 0;
    float count;


    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        CenterAndZoomToFit();
        ships = GameObject.FindGameObjectsWithTag(GlobalValues.ShipTag).OfType<GameObject>().ToList();
        currentShips = ships.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (ships.Count != currentShips)
        { ships = GameObject.FindGameObjectsWithTag(GlobalValues.ShipTag).OfType<GameObject>().ToList(); currentShips = ships.Count; }
        // Only zoom to fit if the camera size is below the maximum size
        CenterAndZoomToFit();
        KeepShipsInsideViewport();
        count += Time.deltaTime;
    }

    private void CenterAndZoomToFit()
    {
        // Update the orthographic size of the camera
        float maxAxisDistance = GetMaxAxisDistanceToCamera() + InsideMargin;
        cam.orthographicSize = CameraScaleGraph.Evaluate(maxAxisDistance);
        Vector3 targetCenter;
        if (count > camDelay)
             targetCenter = CameraTargetPosition();
        else
            targetCenter = Vector3.zero;
        
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
            avg /= counter;
            if (avg.x > camBufferX)
                avg.x = camBufferX;
            if (avg.x < -camBufferX)
                avg.x = -camBufferX;
            if (avg.y > camBufferY)
                avg.y = camBufferY;
            if (avg.y < -camBufferY)
                avg.y = -camBufferY;
            return avg;
        }
        else
        {
            return cam.transform.position;
        }
    }

    private float GetMaxAxisDistanceToCamera()
    {
        Vector2 max = new Vector2(MinSize, MinSize);
        for (int index = 0; index < ships.Count; ++index)
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
        if (max.x > max.y)
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
        Rigidbody2D rb = ship.GetComponent<Rigidbody2D>();
        // Set velocity of ship to zero
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        // Add force towards the center of the viewport
        Vector2 direction = cam.transform.position - ship.transform.position;
        direction.Normalize();
        rb.AddForce(direction * PushInForce * rb.mass);
        // Add torque
        // Positive rotation (counterclockwise)
        if (Random.value >= 0.5f)
        {
            rb.AddTorque(Random.Range(0.5f, 1) * PushInTorque * rb.mass, ForceMode2D.Impulse);
        }
        // Negative rotation (clockwise)
        else
        {
            rb.AddTorque(-Random.Range(0.5f, 1) * PushInTorque * rb.mass, ForceMode2D.Impulse);
        }
        // Disable ship's thrusters for DisableTime seconds
        Thruster[] thrusters = ship.GetComponentsInChildren<Thruster>();
        foreach (Thruster t in thrusters)
        {
            StartCoroutine(t.Disable(ThrusterDisableTime));
        }

    }
}