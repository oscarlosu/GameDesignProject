﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Laser : Projectile
{
    public float EffectDuration;
    public float FadeDuration;

    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private float elapsedTime;
    
    void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
		this.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {        
        elapsedTime += Time.deltaTime;
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, Mathf.Lerp(1, 0, elapsedTime / FadeDuration));
        if (elapsedTime > EffectDuration)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        if(elapsedTime > FadeDuration)
        {
            // Destroy laser
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == GlobalValues.StructureTag && other.gameObject != SourceStructure)
        {            
            // Make ship lose hp
            Structure str = other.gameObject.GetComponent<Structure>();
            str.TakeDamage(Damage);
            
        }
        else if (other.gameObject.tag == GlobalValues.AsteroidTag)
        {
            // TODO Make it collide with asteroid.
            //other.gameObject.GetComponent<Asteroid>().Breakdown();            
        }
    }
}