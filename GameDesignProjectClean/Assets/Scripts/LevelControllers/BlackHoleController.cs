using UnityEngine;
using System.Collections;

public class BlackHoleController : MonoBehaviour {
    public SpriteRenderer[] blackHoleParts;
    [Space(10)]
    public float speed = 0.0f;
    public float moveModifier = 0.0f;
    public AnimationCurve fade = new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(5, 1));
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Move();
        Fade();
	}

    void Rotate()
    {
        for(int i = 0; i < blackHoleParts.Length - 1; i++)
        {
            blackHoleParts[i].transform.Rotate(new Vector3(0, 0, (((i + 1) * 1.5f) * speed) * Time.deltaTime));
        }
    }

    void Move()
    {
        for (int i = 0; i < blackHoleParts.Length; i++)
        {
            blackHoleParts[i].transform.position = (new Vector3(Mathf.Sin(Time.realtimeSinceStartup) * (i * moveModifier),
                Mathf.Sin(Time.realtimeSinceStartup) * (i * moveModifier),
                0));
        }
    }

    void Fade()
    {
        for (int i = 0; i < blackHoleParts.Length - 1; i++)
        {
            float flo = Time.realtimeSinceStartup + i;
            blackHoleParts[i].color = new Color(1, 1, 1, fade.Evaluate(flo));
        }
    }
}