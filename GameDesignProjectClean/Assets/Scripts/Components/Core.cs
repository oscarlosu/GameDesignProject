﻿using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
public class Core : Structure
{
    public GamepadInput.GamePad.Index ControllerIndex;

    public ControlMode ShipControlMode;
    public float AngularDragHigh; // The drag set, when the ship is pointing in the right direction (if the control scheme is direction based).
    public float DefaultAngularDrag { get; private set; } // Getter for the rigid body's default/initial angular drag.
    public float MaxSpinBeforeAngularDrag; // How many degrees per second the ship should at least spin, before applying high angular drag.

    public bool InBuildMode;

    // Public methods

    // Use this for initialization
    public new void Start()
    {
        base.Start();
        // Add core and structure mass to rigidbody
        ShipCore = this.gameObject;
        //Assemble();
        DefaultAngularDrag = GetComponent<Rigidbody2D>().angularDrag;
    }

    // Update is called once per frame
    public void Update()
    {

    }

    public void Assemble()
    {
        // Set initial mass to core mass.
        GetComponent<Rigidbody2D>().mass = Mass;
        // Retrieve all children
        List<GameObject> children = RetrieveChildren(gameObject);
        // Set itself as the ship of each component
        foreach (GameObject child in children)
        {
            if(child.GetComponent<ShipComponent>() != null)
            {
                child.GetComponent<ShipComponent>().ShipCore = gameObject;
                GetComponent<Rigidbody2D>().mass += child.GetComponent<ShipComponent>().Mass;         
                // If this component is a structure, find all shields attached.
                if (child.GetComponent<Structure>() != null)
                {
                    child.GetComponent<Structure>().FindNearbyShipShields();
                }       
            }
        }            
    }

    // Private methods
    private List<GameObject> RetrieveChildren(GameObject go)
    {
        List<GameObject> myChildren = new List<GameObject>();
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            GameObject myChild = go.transform.GetChild(i).gameObject;
            // Add your own child
            myChildren.Add(myChild);
            // Add your child's children
            myChildren.AddRange(RetrieveChildren(myChild));
        }
        return myChildren;
    }

    /****************
    * Control modes.
    ****************/
    public enum ControlMode
    {
        RotationControlMode, DirectionControlMode
    }
}


/****************
* Editor tools.
****************/

[CustomEditor(typeof(Core), true)]
public class CoreEditor : StructureEditor
{

    protected new void DrawCustomInspector()
    {
        // Create heading.
        GUIStyle heading = new GUIStyle { fontSize = 14 };
        EditorGUILayout.LabelField("Core settings", heading);

        // Get target and show/edit fields.
        Core t = (Core)target;
        t.ControllerIndex = (GamepadInput.GamePad.Index)EditorGUILayout.EnumPopup("Controller", t.ControllerIndex);
        t.ShipControlMode = (Core.ControlMode) EditorGUILayout.EnumPopup("Control mode", t.ShipControlMode);
        // Only show settings relevant for the direction control mode, when that mode is selected.
        if (t.ShipControlMode == Core.ControlMode.DirectionControlMode)
        {
            t.AngularDragHigh = EditorGUILayout.FloatField("Angular drag high", t.AngularDragHigh);
            t.MaxSpinBeforeAngularDrag = EditorGUILayout.FloatField("Max spin before angular drag",
                t.MaxSpinBeforeAngularDrag);
            EditorGUILayout.HelpBox(
            "The 'Angular drag high' value will be set as the angular drag of the ship, if it is pointing the correct direction," +
            "but it is spinning more degrees per second than stated in the 'Max Spin Before Angular Drag' value.", MessageType.Info);
        }

        // If the target was changed, set the target to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (CustomInspectorOpen)
        {
            DrawCustomInspector();
        }
    }
}
