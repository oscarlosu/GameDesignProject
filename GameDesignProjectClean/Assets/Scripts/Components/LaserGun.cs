﻿using System;
using UnityEngine;
using GamepadInput;
using UnityEditor;

public class LaserGun : Module
{
    public GameObject LaserPrefab;

    public float Cooldown;
    public float MinLaserBreadth, MaxLaserBreadth;
    public float MinLaserLength, MaxLaserLength;
    public float MinChargeTime, MaxChargeTime;


    private float elapsedTime;
    private bool ready;

    private bool triggerDown;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        ready = true;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // Handle normal input.
        if (ready)
        {
            switch (InputType)
            {
                case InputKeyType.Button:
                    if (GamePad.GetButtonDown(ButtonKey, Core.GetComponent<Core>().ControllerIndex))
                    {
                        elapsedTime = 0;
                    }
                    else if (GamePad.GetButton(ButtonKey, Core.GetComponent<Core>().ControllerIndex))
                    {
                        elapsedTime += Time.deltaTime;
                    }
                    else if (GamePad.GetButtonUp(ButtonKey, Core.GetComponent<Core>().ControllerIndex))
                    {
                        elapsedTime += Time.deltaTime;
                        Activate();
                    }
                    break;
                case InputKeyType.Trigger:
                    var value = GamePad.GetTrigger(TriggerKey, Core.GetComponent<Core>().ControllerIndex);
                    if (value > 0 && !triggerDown) // Activation threshold.
                    {
                        elapsedTime = 0;
                        triggerDown = true;
                    }
                    else if (value >= 0.5 && triggerDown)
                    {
                        elapsedTime += Time.deltaTime;
                    }
                    else if (value < 0.5 && triggerDown)
                    {
                        Activate();
                        triggerDown = false;
                    }
                    break;
            }
        }

        // Handle cooldown
        if (!ready)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= Cooldown)
            {
                ready = true;
                //Debug.Log("Ready!");
                elapsedTime = 0;
            }
        }

    }

    public void Activate()
    {
        ready = false;
        //Debug.Log("Activated!");
        // Instantiate laser
        GameObject laser = (GameObject)Instantiate(LaserPrefab, transform.position, transform.rotation);
        laser.transform.parent = transform;
        // Calculate breadth and length of laser and scale
        Vector2 scale = calculateLaserScale();
        laser.transform.localScale = scale;
        //Debug.Log("ScaleFactor = " + scale);
        // Calculate size of laser and position accordingly
        Vector2 sprSize = laser.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Vector2 size = new Vector2(scale.x * sprSize.x, scale.y * sprSize.y);
        //Debug.Log("LaserSize = " + size);
        laser.transform.position += laser.transform.up * 0.5f;
        // Save source structure
        laser.GetComponent<Laser>().SourceStructure = transform.parent.gameObject;
        laser.GetComponent<Laser>().SourceCore = Core;
        // Reset elapsedTime
        elapsedTime = 0;
    }

    Vector2 calculateLaserScale()
    {
        float normalizedElapsedTime = Mathf.Clamp(elapsedTime, 0, MaxChargeTime) / MaxChargeTime;
        float factorBreadth = Mathf.Lerp(MinLaserBreadth, MaxLaserBreadth, normalizedElapsedTime);
        float factorLength = Mathf.Lerp(MinLaserLength, MaxLaserLength, normalizedElapsedTime);
        return new Vector2(factorBreadth, factorLength);
    }
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(LaserGun), true)]
public class LaserGunEditor : ModuleEditor
{

    public override void OnInspectorGUI()
    {
        // Take the module inspector.
        base.OnInspectorGUI();

        LaserGun laserGun = (LaserGun)target;

        // Selector for projectile prefab

        laserGun.LaserPrefab = (GameObject)EditorGUILayout.ObjectField("Laser prefab", laserGun.LaserPrefab, typeof(GameObject), false);
        laserGun.Cooldown = EditorGUILayout.FloatField("Cooldown", laserGun.Cooldown);
        laserGun.MinChargeTime = EditorGUILayout.FloatField("Min charge time", laserGun.MinChargeTime);
        laserGun.MaxChargeTime = EditorGUILayout.FloatField("Max charge time", laserGun.MaxChargeTime);
        laserGun.MinLaserBreadth = EditorGUILayout.FloatField("Min laser breadth", laserGun.MinLaserBreadth);
        laserGun.MaxLaserBreadth = EditorGUILayout.FloatField("Max laser breadth", laserGun.MaxLaserBreadth);
        laserGun.MinLaserLength = EditorGUILayout.FloatField("Min laser length", laserGun.MinLaserLength);
        laserGun.MaxLaserLength = EditorGUILayout.FloatField("Max laser length", laserGun.MaxLaserLength);

    }
}