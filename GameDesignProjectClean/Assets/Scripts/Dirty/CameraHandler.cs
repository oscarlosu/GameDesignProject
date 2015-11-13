using UnityEngine;
using System.Collections;

public class DirtyCameraHandler : MonoBehaviour
{

    public GameObject[] ObjectsInCamera;
    public float ExtraEdge;
    private Camera currentCamera;

    // Use this for initialization
    void Start()
    {
        currentCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float minX = 0, maxX = 0, minY = 0, maxY = 0;
        foreach (var go in ObjectsInCamera)
        {
            if (go.transform.position.x < minX)
                minX = go.transform.position.x;
            if (go.transform.position.x > maxX)
                maxX = go.transform.position.x;
            if (go.transform.position.y < minY)
                minY = go.transform.position.y;
            if (go.transform.position.y > maxY)
                maxY = go.transform.position.y;
        }

        currentCamera.orthographicSize = Mathf.Max(Mathf.Abs(minX), Mathf.Abs(maxX), Mathf.Abs(minY), Mathf.Abs(maxY)) + ExtraEdge;
    }
}
