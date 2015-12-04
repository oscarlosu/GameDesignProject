using UnityEngine;
using System.Collections;

public class BackgroundScaler : MonoBehaviour {
    public Camera cam;
	public float scale = 0.2f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(cam.orthographicSize * scale, cam.orthographicSize * scale, 1);
	}
}
