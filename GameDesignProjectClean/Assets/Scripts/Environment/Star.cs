using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Star : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create(Vector3 starPos, int starRadius, Sprite starSprite)
    {
        GetComponent<SpriteRenderer>().sprite = starSprite;
        transform.position = starPos;
        transform.localScale = new Vector2(starRadius, starRadius);
    }
}
