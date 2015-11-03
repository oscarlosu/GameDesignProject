using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;

public class Thruster : Module
{
    [SerializeField] public float ThrustPower;


    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    public void Update()
    {
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
        Thruster t = (Thruster) target;
        t.ThrustPower = EditorGUILayout.FloatField("Power", t.ThrustPower);

        // If the target was changed, set the target to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}