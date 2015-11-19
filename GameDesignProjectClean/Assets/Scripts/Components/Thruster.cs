﻿using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Thruster : Module
{
    private ParticleSystem[] childParticles;
    public float ThrustPower;

    public float DotMargin; // Default 0.01 .
    public float CrossMargin; // Default 0.01 .

    // Use this for initialization
    new void Start()
    {
        base.Start();
        childParticles = gameObject.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < childParticles.Length; i++)
        {
            if (childParticles[i].isPlaying)
                childParticles[i].Stop();
        }
    }

    public void Update()
    {
        // If velocity magnitude is larger than 0, play thruster sound.
        if (ShipCore.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
        {
            //Thruster sound depending on velocity. This should not be here, but couldn't get it to work in Activate().
            //It still needs some work, becuase right now, thrusters are playing all the time, not just when they are fired
            //Feel free to move to a better location.
            this.GetComponent<AudioSource>().pitch =
                Mathf.Clamp(ShipCore.GetComponent<Rigidbody2D>().velocity.magnitude, 0f, 2.5f) + Random.Range(0f, 0.5f);
            this.GetComponent<AudioSource>().volume = Mathf.Clamp(ShipCore.GetComponent<Rigidbody2D>().velocity.magnitude,
                0f, 0.2f);
        }

        // If the ship hasn't been set yet, don't do ANYTHING!
        if (ShipCore == null)
        {
            return;
        }
        // Handle normal input.
        switch (InputType)
        {
            case InputKeyType.Button:
                if (GamePad.GetButton(ButtonKey, ShipCore.GetComponent<Core>().ControllerIndex))
                {
                    Activate(1);
                    // DEBUG: Change sprite colour, when power is larger than 0.
                    GetComponent<SpriteRenderer>().color = Color.magenta;
                }
                else
                {
                    // DEBUG: Change sprite colour, when deactivating.
                    GetComponent<SpriteRenderer>().color = Color.white;
                    Deactivate();
                }

                break;
            case InputKeyType.Trigger:
                var value = GamePad.GetTrigger(TriggerKey, ShipCore.GetComponent<Core>().ControllerIndex);
                if (value > 0)
                {
                    Activate(value);
                    // DEBUG: Change sprite colour, when power is larger than 0.
                    GetComponent<SpriteRenderer>().color = Color.magenta;
                }
                else
                {
                    // DEBUG: Change sprite colour, when deactivating.
                    GetComponent<SpriteRenderer>().color = Color.white;
                    Deactivate();
                }
                break;
        }

        // Handle thumb stick input.
        Vector2 leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, ShipCore.GetComponent<Core>().ControllerIndex);
        if (leftStickValue.magnitude > 0)
        {
            var powerTotal = 0f;
            // Control ship depending on the selected control mode.
            switch (ShipCore.GetComponent<Core>().ShipControlMode)
            {
                case Core.ControlMode.RotationControlMode:
                    powerTotal = RotationStickControl(leftStickValue);
                    break;
                case Core.ControlMode.DirectionControlMode:
                    powerTotal = DirectionStickControl(leftStickValue);
                    break;
            }
                
            // DEBUG: Change sprite colour, when power is larger than 0.
            GetComponent<SpriteRenderer>().color = powerTotal > 0 ? Color.magenta : Color.white;
            ShipCore.GetComponent<Rigidbody2D>().AddForceAtPosition((-transform.up) * ThrustPower * (powerTotal), transform.position);
            if (childParticles.Length > 0)
            {
                if (powerTotal > 0 && !childParticles[0].isPlaying)
                {
                    for (int i = 0; i < childParticles.Length; i++)
                    {
                        childParticles[i].Play();
                    }
                }
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            Deactivate();
        }
    }

    public float RotationStickControl(Vector2 leftStickValue)
    {
        float powerX = 0, powerY = 0;
        var shipRelative = ShipCore.transform.InverseTransformPoint(transform.position);

        // Determines if thrusters point forwards, backwards or sideways.
        float dot = Vector3.Dot(ShipCore.transform.up, transform.up);
        float crossZ = Vector3.Cross(ShipCore.transform.up, transform.up).z;
        // dot == 0 && z < 0 : thruster pointing right

        // Check which quadrant the thrusters are in.
        if (shipRelative.x < 0 && shipRelative.y > 0) // Upper left quadrant.
        {
            if (dot > 0.01) // Thruster pointing forwards.
            {
                if ((leftStickValue.x >= 0 && leftStickValue.y < 0) ||
                    (leftStickValue.x < 0 && leftStickValue.y == 0))
                {
                    powerX = leftStickValue.x;
                    powerY = leftStickValue.y;
                }
            }
            else if (dot < -0.01) // Thruster pointing backwards.
            {
                if (leftStickValue.x >= 0 && leftStickValue.y > 0)
                {
                    powerX = leftStickValue.x;
                    powerY = leftStickValue.y;
                }
            }
            else
            {
                if (crossZ > 0) // Thrusters pointing left.
                {
                    if ((leftStickValue.x > 0 && leftStickValue.y >= 0) ||
                        (leftStickValue.x < 0 && leftStickValue.y < 0))
                    {
                        powerX = leftStickValue.x;
                        powerY = leftStickValue.y;
                    }
                }
                else if (crossZ < 0) // Thruster pointing right.
                {
                    if ((leftStickValue.x < 0 && leftStickValue.y >= 0) ||
                        (leftStickValue.x > 0 && leftStickValue.y < 0))
                    {
                        powerX = leftStickValue.x;
                        powerY = leftStickValue.y;
                    }
                }
            }
        }
        else if (shipRelative.x > 0 && shipRelative.y > 0) // Upper right quadrant.
        {
            if (dot > 0.01) // Thruster pointing forwards.
            {
                if ((leftStickValue.x <= 0 && leftStickValue.y < 0) ||
                    (leftStickValue.x > 0 && leftStickValue.y == 0))
                {
                    powerX = leftStickValue.x;
                    powerY = leftStickValue.y;
                }
            }
            else if (dot < -0.01) // Thruster pointing backwards.
            {
                if (leftStickValue.x <= 0 && leftStickValue.y > 0)
                {
                    powerX = leftStickValue.x;
                    powerY = leftStickValue.y;
                }
            }
            else
            {
                if (crossZ > 0) // Thrusters pointing left.
                {
                    if ((leftStickValue.x > 0 && leftStickValue.y >= 0) ||
                        (leftStickValue.x < 0 && leftStickValue.y < 0))
                    {
                        powerX = leftStickValue.x;
                        powerY = leftStickValue.y;
                    }
                }
                else if (crossZ < 0) // Thruster pointing right.
                {
                    if ((leftStickValue.x < 0 && leftStickValue.y >= 0) ||
                        (leftStickValue.x > 0 && leftStickValue.y < 0))
                    {
                        powerX = leftStickValue.x;
                        powerY = leftStickValue.y;
                    }
                }
            }
        }
        else if (shipRelative.x < 0 && shipRelative.y < 0) // Lower left quadrant.
        {
            if (dot > 0.01) // Thruster pointing forwards.
            {
                if (leftStickValue.x >= 0 && leftStickValue.y < 0)
                {
                    powerX = leftStickValue.x;
                    powerY = leftStickValue.y;
                }
            }
            else if (dot < -0.01) // Thruster pointing backwards.
            {
                if ((leftStickValue.x >= 0 && leftStickValue.y > 0) ||
                    (leftStickValue.x > 0 && leftStickValue.y == 0))
                {
                    powerX = leftStickValue.x;
                    powerY = leftStickValue.y;
                }
            }
            else
            {
                if (crossZ > 0) // Thrusters pointing left.
                {
                    if ((leftStickValue.x < 0 && leftStickValue.y >= 0) ||
                        (leftStickValue.x > 0 && leftStickValue.y < 0))
                    {
                        powerX = leftStickValue.x;
                        powerY = leftStickValue.y;
                    }
                }
                else if (crossZ < 0) // Thruster pointing right.
                {
                    if ((leftStickValue.x > 0 && leftStickValue.y >= 0) ||
                        (leftStickValue.x < 0 && leftStickValue.y < 0))
                    {
                        powerX = leftStickValue.x;
                        powerY = leftStickValue.y;
                    }
                }
            }
        }
        else if (shipRelative.x > 0 && shipRelative.y < 0) // Lower right quadrant.
        {
            if (dot > 0.01) // Thruster pointing forwards.
            {
                if (leftStickValue.x <= 0 && leftStickValue.y < 0)
                {
                    powerX = leftStickValue.x;
                    powerY = leftStickValue.y;
                }
            }
            else if (dot < -0.01) // Thruster pointing backwards.
            {
                if ((leftStickValue.x <= 0 && leftStickValue.y > 0) ||
                    (leftStickValue.x < 0 && leftStickValue.y == 0))
                {
                    powerX = leftStickValue.x;
                    powerY = leftStickValue.y;
                }
            }
            else
            {
                if (crossZ > 0) // Thrusters pointing left.
                {
                    if ((leftStickValue.x < 0 && leftStickValue.y >= 0) ||
                        (leftStickValue.x > 0 && leftStickValue.y < 0))
                    {
                        powerX = leftStickValue.x;
                        powerY = leftStickValue.y;
                    }
                }
                else if (crossZ < 0) // Thruster pointing right.
                {
                    if ((leftStickValue.x > 0 && leftStickValue.y >= 0) ||
                        (leftStickValue.x < 0 && leftStickValue.y < 0))
                    {
                        powerX = leftStickValue.x;
                        powerY = leftStickValue.y;
                    }
                }
            }
        }
        return Mathf.Abs(powerX) + Mathf.Abs(powerY);;
    }

    public float DirectionStickControl(Vector2 leftStickValue)
    {
        float powerX = 0, powerY = 0;

        // Is ship's direction the direction we want?
        float dot = Vector3.Dot(new Vector3(leftStickValue.x, leftStickValue.y).normalized, ShipCore.transform.up.normalized);
        float crossZ = Vector3.Cross(new Vector3(leftStickValue.x, leftStickValue.y).normalized, ShipCore.transform.up).normalized.z;

        var shipRelative = ShipCore.transform.InverseTransformPoint(transform.position);
        float dotThruster = Vector3.Dot(ShipCore.transform.up, transform.up);
        float crossZThruster = Vector3.Cross(ShipCore.transform.up, transform.up).z;

        //Debug.Log("Dot: " + dot + ", CrossZ: " + crossZ);

        if (dot > 1 - DotMargin) // If it is, power all forwards thrusters.
        {
            Debug.Log("Ship is facing same direction as the we point.");
            // If thruster is pointing backwards, fire.
            if (dotThruster < -1 + DotMargin) // Thruster pointing backwards.
            {
                powerX = ThrustPower;
                //powerY = ThrustPower;
            }

            // Have high angular drag to the ship, when it's pointing the right direction and its angular velocity is too high,
            // to stop it from spinning.
            var coreRigidBody = ShipCore.GetComponent<Rigidbody2D>();
            if (Mathf.Abs(coreRigidBody.angularVelocity) > ShipCore.GetComponent<Core>().MaxSpinBeforeAngularDrag)
            {
                coreRigidBody.angularDrag = ShipCore.GetComponent<Core>().AngularDragHigh;
            }
            else
            {
                coreRigidBody.angularDrag = ShipCore.GetComponent<Core>().DefaultAngularDrag;
            }

        }
        else if (dot < -1 + DotMargin) // Go backwards (possibly turn as well?).
        {
            // If thruster is pointing forwards, fire.
            if (dotThruster > 1 - DotMargin)
            {
                powerX = ThrustPower;
                //powerY = leftStickValue.y;
            }
        }
        else if (crossZ < -CrossMargin) // Turn left.
        {
            // If moving forwards, fire thrusters pointing backwards on the right side and thrusters pointing right on the upper right side and pointing left on the lower left.
            if (dot > DotMargin)
            {
                // Thrusters pointing backwards on the right side.
                if (dotThruster < -DotMargin && shipRelative.x > 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing right in the upper right side.
                else if (crossZThruster < -CrossMargin && shipRelative.x > 0 && shipRelative.y > 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing left in the lower left side.
                else if (crossZThruster > CrossMargin && shipRelative.x < 0 && shipRelative.y < 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
            }
            // If moving backwards, fire thrusters pointing forwards on the left side and thrusters pointing left in the lower left side and pointing right on the upper right.
            else if (dot < -DotMargin)
            {
                // Thrusters pointing forwards on the left side.
                if (dotThruster > DotMargin && shipRelative.x < 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing left in the lower left side.
                else if (crossZThruster > CrossMargin && shipRelative.x < 0 && shipRelative.y < 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing right in the upper right side.
                else if (crossZThruster < -CrossMargin && shipRelative.x > 0 && shipRelative.y > 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
            }
            // If not moving forwards or backwards, but just rotating, fire thrusters pointing right on upper right side and thrusters pointing left on lower left side.
            else if (Mathf.Abs(dot) < DotMargin)
            {
                // Thrusters pointing right in upper right corner.
                if (crossZThruster < -CrossMargin && shipRelative.x > 0 && shipRelative.y > 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing left in lower left corner.
                else if (crossZThruster > CrossMargin && shipRelative.x < 0 && shipRelative.y < 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
            }

        }
        else if (crossZ > CrossMargin) // Turn right.
        {
            // If moving forwards, fire thrusters pointing backwards on the left side and thrusters pointing left on the upper left side and pointing right on the lower right.
            if (dot > DotMargin)
            {
                // Thrusters pointing backwards on the left side.
                if (dotThruster < -DotMargin && shipRelative.x < 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing left on the upper left side.
                else if (crossZThruster > CrossMargin && shipRelative.x < 0 && shipRelative.y > 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing right on the lower right.
                else if (crossZThruster < -CrossMargin && shipRelative.x > 0 && shipRelative.y < 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
            }
            // If moving backwards, fire thrusters pointing forwards on the right side and thrusters pointing left on the upper left side and pointing right on the lower right.
            else if (dot < -DotMargin)
            {
                // Thrusters pointing forwards on the right side.
                if (dotThruster > DotMargin && shipRelative.x > 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing left on the upper left side.
                else if (crossZThruster > CrossMargin && shipRelative.x < 0 && shipRelative.y > 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing right on the lower right side.
                else if (crossZThruster < -CrossMargin && shipRelative.x > 0 && shipRelative.y < 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
            }
            // If not moving forwards or backwards, but just rotating, fire thrusters pointing left in upper left side and thrusters pointing right in lower right side.
            else if (Mathf.Abs(dot) < DotMargin)
            {
                // Thrusters pointing left in upper left side.
                if (crossZThruster > CrossMargin && shipRelative.x < 0 && shipRelative.y > 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
                // Thrusters pointing right in lower right side.
                else if (crossZThruster < -CrossMargin && shipRelative.x > 0 && shipRelative.y < 0)
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }
            }
        }
        return powerX + powerY;
    }

    public void Activate(float power)
    {
        ShipCore.GetComponent<Rigidbody2D>().AddForceAtPosition((-transform.up) * ThrustPower * power, transform.position);
        if (!childParticles[0].isPlaying)
        {
            Debug.Log("activate function");
            for (int i = 0; i < childParticles.Length; i++)
            {
                childParticles[i].Play();
            }
        }
    }

    void Deactivate()
    {
        bool input = false;
        switch (InputType)
        {
            case InputKeyType.Button:
                if (GamePad.GetButton(ButtonKey, ShipCore.GetComponent<Core>().ControllerIndex))
                    input = true;
                break;
            case InputKeyType.Trigger:
                var value = GamePad.GetTrigger(TriggerKey, ShipCore.GetComponent<Core>().ControllerIndex);
                if (value > 0)
                    input = true;
                break;
            default:
                Debug.LogError("AAAARRRGGGHHHH");
                break;
        }
        Vector2 leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, ShipCore.GetComponent<Core>().ControllerIndex);
        if (leftStickValue.magnitude > 0.2f)
        {
            input = true;
        }

        if (!input)
        {
            for (int i = 0; i < childParticles.Length; i++)
            {
                childParticles[i].Stop();
            }

        }
    }

	public IEnumerator Disable(float time)
	{
		enabled = false;
		yield return new WaitForSeconds(time);
		enabled = true;
	}
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Thruster), true)]
public class ThrusterEditor : ModuleEditor
{

    public new void DrawCustomInspector()
    {
        // Create heading.
        GUIStyle heading = new GUIStyle { fontSize = 14 };
        EditorGUILayout.LabelField("Thrusters settings", heading);

        // Get target and show/edit fields.
        Thruster t = (Thruster)target;
        t.ThrustPower = EditorGUILayout.FloatField("Power", t.ThrustPower);
        t.DotMargin = EditorGUILayout.FloatField("Dot margin", t.DotMargin);
        t.CrossMargin = EditorGUILayout.FloatField("Cross margin", t.CrossMargin);

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