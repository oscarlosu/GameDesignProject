﻿using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;

public class Armor : Module
{

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {

    }
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Armor), true)]
public class ArmorEditor : ModuleEditor
{

    public override void OnInspectorGUI()
    {
        // Take the module inspector.
        base.OnInspectorGUI();

        Armor armor = (Armor)target;

        // Selector for projectile prefab

    }
}