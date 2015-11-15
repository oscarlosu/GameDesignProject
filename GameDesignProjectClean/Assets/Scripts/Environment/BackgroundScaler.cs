using UnityEngine;
using System.Collections;

public class BackgroundScaler : MonoBehaviour {
    public Camera cam;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(cam.orthographicSize * 0.2f, cam.orthographicSize * 0.2f, 1);
	}
}
