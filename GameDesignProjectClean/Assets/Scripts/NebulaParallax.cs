using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NebulaParallax : MonoBehaviour
{

    public Transform gameCamera;
    [Space(10)]
    public float parallaxModifier;

    Vector3 prevCamPos;

    // Use this for initialization
    void Start()
    {
        prevCamPos = gameCamera.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameCamera.position != prevCamPos)
        {
            Vector3 parallax = (prevCamPos - gameCamera.position) * parallaxModifier;
            parallax.z = 0;
            transform.position -= parallax;
        }

        prevCamPos = gameCamera.position;
    }
}