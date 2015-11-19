﻿using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class Thruster : Module
{
    private ParticleSystem[] childParticles;
    public float ThrustPower;

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
        if (Core.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
        {
            //Thruster sound depending on velocity. This should not be here, but couldn't get it to work in Activate().
            //It still needs some work, becuase right now, thrusters are playing all the time, not just when they are fired
            //Feel free to move to a better location.
            this.GetComponent<AudioSource>().pitch =
                Mathf.Clamp(Core.GetComponent<Rigidbody2D>().velocity.magnitude, 0f, 2.5f) + Random.Range(0f, 0.5f);
            this.GetComponent<AudioSource>().volume = Mathf.Clamp(Core.GetComponent<Rigidbody2D>().velocity.magnitude,
                0f, 0.2f);
        }

        // If the ship hasn't been set yet, don't do ANYTHING!
        if (Core == null)
        {
            return;
        }
        // Handle normal input.
        switch (InputType)
        {
            case InputKeyType.Button:
                if (GamePad.GetButton(ButtonKey, Core.GetComponent<Core>().ControllerIndex))
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
                var value = GamePad.GetTrigger(TriggerKey, Core.GetComponent<Core>().ControllerIndex);
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
        Vector2 leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, Core.GetComponent<Core>().ControllerIndex);
        if (leftStickValue.magnitude > 0)
        {
            float powerX = 0, powerY = 0;
            var shipRelative = Core.transform.InverseTransformPoint(transform.position);

            // Determines if thrusters point forwards, backwards or sideways.
            float dot = Vector3.Dot(Core.transform.up, transform.up);
            float crossZ = Vector3.Cross(Core.transform.up, transform.up).z;
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

            var powerTotal = Mathf.Abs(powerX) + Mathf.Abs(powerY);
            // DEBUG: Change sprite colour, when power is larger than 0.
            GetComponent<SpriteRenderer>().color = powerTotal > 0 ? Color.magenta : Color.white;
            Core.GetComponent<Rigidbody2D>().AddForceAtPosition((-transform.up) * ThrustPower * (powerTotal), transform.position);
            if (childParticles.Length > 0)
            {
                if (powerTotal > 0 && !childParticles[0].isPlaying)
                {
                    Debug.Log("analoge stick");
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

    public void Activate(float power)
    {
        Core.GetComponent<Rigidbody2D>().AddForceAtPosition((-transform.up) * ThrustPower * power, transform.position);
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
                if (GamePad.GetButton(ButtonKey, Core.GetComponent<Core>().ControllerIndex))
                    input = true;
                break;
            case InputKeyType.Trigger:
                var value = GamePad.GetTrigger(TriggerKey, Core.GetComponent<Core>().ControllerIndex);
                if (value > 0)
                    input = true;
                break;
            default:
                Debug.LogError("AAAARRRGGGHHHH");
                break;
        }
        Vector2 leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, Core.GetComponent<Core>().ControllerIndex);
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