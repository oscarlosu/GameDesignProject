﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    public float scaleFactor = 1.0f;
    public float duration = 1.0f;
    public AnimationCurve explosionAnim = new AnimationCurve(new Keyframe(0, 0.05f), new Keyframe(0.5f,0.5f), new Keyframe(1, 1));
    [Space(10)]
    public int Damage;
    public float PushForce;
    public GameObject SourceCore;

    public Sprite[] explosionSprites;
    private SpriteRenderer sr;

    private float elapsedTime;
	private ObjectPool pool;

	private AudioSource audio;

    void Awake()
    {
		pool = GameObject.FindGameObjectWithTag(GlobalValues.ObjectPoolTag).GetComponent<ObjectPool>();
        GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
        Sprite sprite = explosionSprites[Random.Range(0, explosionSprites.Length)];
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
		audio = GetComponent<AudioSource>();
    }

	void OnEnable()
	{
		elapsedTime = 0;
		StartCoroutine("DestroyExplosion");
		audio.Play();
	}

    void Update()
    {
        float newScale = explosionAnim.Evaluate(elapsedTime) * scaleFactor;
        transform.localScale = Vector3.one * newScale;
        sr.color = new Color(1, 1, 1, 1 - elapsedTime);
        elapsedTime += Time.deltaTime/duration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		////Debug.Log ("Explosion detected OnTriggerEnter2D with " + other.gameObject.name);
		Structure str = other.gameObject.GetComponent<Structure>();
		Asteroid ast = other.gameObject.GetComponent<Asteroid>();
		Debris deb = other.gameObject.GetComponent<Debris>();
        if (str != null)
        {
            // Make ship take damage.            
            str.TakeDamage(Damage, SourceCore.GetComponent<Core>());
			// Calculate push force direction
			Vector3 dir = other.transform.position - transform.position;
			dir.Normalize();
            str.ShipCore.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * PushForce, transform.position);
        }
        else if (ast != null)
        {
            other.gameObject.GetComponent<Asteroid>().Breakdown(null);
			// Calculate push force direction
			Vector3 dir = other.transform.position - transform.position;
			dir.Normalize();
			ast.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * PushForce, transform.position);
        }
		else if(deb != null)
		{
			deb.Breakdown ();
		}
		// Only affects rigidbodies with a mass 
		/*else if (other.GetComponent<Rigidbody2D>() != null && 
		         other.GetComponent<Collider2D>() != null && !other.GetComponent<Collider2D>().isTrigger/*other.GetComponent<Rigidbody2D>().mass > GlobalValues.EffectiveZeroMass)
        /*{
			Vector3 dir = other.transform.position - transform.position;
			dir.Normalize();
			other.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * PushForce, transform.position);
        }*/
    }

    

    IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(duration);
		pool.DisablePoolObject(gameObject, ObjectPool.ObjectType.Explosion);
    }
}
