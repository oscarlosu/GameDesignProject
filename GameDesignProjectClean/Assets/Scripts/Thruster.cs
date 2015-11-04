using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;

public class Thruster : Module
{

    public float ThrustPower;

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    public void Update()
    {
        // Handle normal input.
        switch (InputType)
        {
            case InputKeyType.Button:
                if (GamePad.GetButton(ButtonKey, Ship.GetComponent<Ship>().ControllerIndex))
                {
                    Activate(1);
                }
                break;
            case InputKeyType.Trigger:
                var value = GamePad.GetTrigger(TriggerKey, Ship.GetComponent<Ship>().ControllerIndex);
                if (value > 0)
                {
                    Activate(value);
                }
                break;
        }

        // Handle thumb stick input.
        Vector2 leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, Ship.GetComponent<Ship>().ControllerIndex);
        if (leftStickValue.magnitude > 0)
        {
            float powerX = 0, powerY = 0;
            float dot = Vector3.Dot(Ship.transform.up, transform.up);
            Debug.Log("DOT: " + dot);
            if (dot < - 0.01) // Thruster is backwards thruster.
            {
                //Debug.Log("Backwards thruster");
                if (leftStickValue.y < 0)
                {
                    powerY = Mathf.Abs(leftStickValue.y);
                }
            }
            else if (dot > 0.01) // Thruster is forwards thruster.
            {
                Debug.Log("Forwards thruster");
                if (leftStickValue.y > 0)
                {
                    powerY = leftStickValue.y;
                }
            }
            else // Thruster is sideways thruster.
            {
                Debug.Log("Sideways thruster");
                // NOTE: Thumbstick left is negative z
                if (Vector3.Cross(Ship.transform.up, transform.up).z < 0)
                {
                    Debug.Log("Cross.z < 0"); 
                    // LEFT!
                    if (leftStickValue.x > 0)
                    {
                        
                    }
                }
                else
                {
                    
                }
            }
            Ship.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * ThrustPower * (powerX + powerY), transform.position);
        }
    }

    public void Activate(float power)
    {
        Ship.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * ThrustPower * power, transform.position);

    }
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Thruster), true)]
public class ThrusterEditor : ModuleEditor
{

    public override void OnInspectorGUI()
    {
        // Display the module's settings.
        base.OnInspectorGUI();

        // Create heading.
        GUIStyle heading = new GUIStyle { fontSize = 14 };
        EditorGUILayout.LabelField("Thrusters settings", heading);

        // Get target and show/edit fields.
        Thruster t = (Thruster)target;
        t.ThrustPower = EditorGUILayout.FloatField("Power", t.ThrustPower);

        // If the target was changed, set the target to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}