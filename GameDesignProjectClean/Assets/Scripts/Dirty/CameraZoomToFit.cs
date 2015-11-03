using UnityEngine;
using System.Collections;

public class CameraZoomToFit : MonoBehaviour {
    public Transform ship1;
    public Transform ship2;
    private float zOffset = 0.0f;
    private Camera camera;

    public AnimationCurve cameraScale = new AnimationCurve() { keys = new Keyframe[] { new Keyframe(0, 0), new Keyframe(50, 25) } };
    public AnimationCurve cameraScaleY = new AnimationCurve() { keys = new Keyframe[] { new Keyframe(0, 0), new Keyframe(50, 25) } };

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

        float xDistance = Mathf.Abs(Mathf.Abs(ship1.position.x) - Mathf.Abs(ship2.position.x));
        float yDistance = Mathf.Abs(Mathf.Abs(ship1.position.y) - Mathf.Abs(ship2.position.y));
        float desiredSize;

        if (xDistance > yDistance)
           desiredSize = cameraScale.Evaluate(Vector3.Distance(ship1.position, ship2.position));
        else
            desiredSize = cameraScaleY.Evaluate(Vector3.Distance(ship1.position, ship2.position));

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, desiredSize, Time.deltaTime * 3);
    }
}
