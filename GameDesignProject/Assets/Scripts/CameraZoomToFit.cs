using UnityEngine;
using System.Collections;

public class CameraZoomToFit : MonoBehaviour {
    public Transform ship1;
    public Transform ship2;
    private float zOffset = 0.0f;
    private Camera camera;

    public AnimationCurve cameraScale = new AnimationCurve() { keys = new Keyframe[] { new Keyframe(0, 0), new Keyframe(50, 25) } };

    void Start()
    {
        zOffset = transform.position.z;
        camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 centre = ((ship2.position - ship1.position)*0.5f) + ship1.position;
        centre.z = zOffset;
        transform.position = centre;
        camera.orthographicSize = cameraScale.Evaluate(Vector3.Distance(ship1.position, ship2.position));
    }
}
