using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NebulaParallax : MonoBehaviour
{

    public Transform gameCamera;
    [Space(10)]
    public float parallaxModifier;
    public float xWrapAroundBuffer;
    public float yWrapAroundBuffer;

    Vector3 prevCamPos;
    Transform[] nebulaArray;
    float numOfNubulas;
    float xNebSize;
    float yNebSize;

    // Use this for initialization
    void Start()
    {
        prevCamPos = gameCamera.position;
        nebulaArray = GetComponentsInChildren<Transform>();
        InvokeRepeating("CheckBackground", 0.1f, 0.1f);
        xNebSize = transform.localScale.x;
        yNebSize = transform.localScale.y;
        numOfNubulas = Mathf.Sqrt((float)nebulaArray.Length-1f);
        

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

    void CheckBackground()
    {
        float xOffset;
        float yOffset;
        for(int i = 0; i < nebulaArray.Length; i++)
        {
            if (nebulaArray[i].name == "Nebula")
                continue;

            //print("out of the if");
            xOffset = gameCamera.position.x - nebulaArray[i].position.x;
            yOffset = gameCamera.position.y - nebulaArray[i].position.y;

            if(xOffset > xWrapAroundBuffer)
            {
                transform.position += transform.right * xNebSize * numOfNubulas;
            }
            else if(xOffset < -xWrapAroundBuffer)
            {
                transform.position -= transform.right * xNebSize * numOfNubulas;
            }

            if (yOffset > yWrapAroundBuffer)
            {
                transform.position += transform.up * yNebSize * numOfNubulas;
            }
            else if (yOffset < -yWrapAroundBuffer)
            {
                transform.position -= transform.up * yNebSize * numOfNubulas;
            }
        }
    }
}