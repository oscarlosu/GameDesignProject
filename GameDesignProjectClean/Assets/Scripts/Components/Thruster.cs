using System;
using UnityEngine;
using GamepadInput;
using UnityEditor;
using Random = UnityEngine.Random;

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

            // Is ship's direction the direction we want?
            float dot = Vector3.Dot(new Vector3(leftStickValue.x, leftStickValue.y), Core.transform.up);
            float crossZ = Vector3.Cross(new Vector3(leftStickValue.x, leftStickValue.y), Core.transform.up).z;

            var shipRelative = Core.transform.InverseTransformPoint(transform.position);
            float dotThruster = Vector3.Dot(Core.transform.up, transform.up);
            float crossZThruster = Vector3.Cross(Core.transform.up, transform.up).z;

            if (dot > 1 - float.Epsilon) // If it is, power all forwards thrusters.
            {
                // If thruster is pointing backwards, fire.
                if (dotThruster < -0.01) // Thruster pointing backwards.
                {
                    powerX = ThrustPower;
                    //powerY = ThrustPower;
                }

            }
            else if (dot < -1 + float.Epsilon) // Go backwards (possibly turn as well?).
            {
                // If thruster is pointing forwards, fire.
                if (dotThruster > 0.01)
                {
                    powerX = ThrustPower;
                    //powerY = leftStickValue.y;
                }
            }
            else if (crossZ > 0) // Turn left.
            {
                // If moving forwards, fire thrusters pointing backwards on the right side and thrusters pointing right on the upper right side and pointing left on the lower left.
                if (dot > float.Epsilon)
                {
                    // Thrusters pointing backwards.
                    if (dotThruster < -0.01 && shipRelative.x > 0)
                    {
                        powerX = ThrustPower;
                        //powerY = ThrustPower;
                    }
                    // Thrusters pointing right in the upper right side.
                    else if (crossZThruster < 0 && shipRelative.x > 0 && shipRelative.y > 0)
                    {
                        powerX = ThrustPower;
                        //powerY = ThrustPower;
                    }
                    // Thrusters pointing left in the lower left side.
                    else if (crossZThruster > 0 && shipRelative.x < 0 && shipRelative.y < 0)
                    {
                        powerX = ThrustPower;
                        //powerY = ThrustPower;
                    }
                }
                // If moving backwards, fire thrusters pointing forwards on the right side and thrusters pointing right on the lower right side and pointing left on the upper left.
                else if (dot < float.Epsilon)
                {
                    // Thrusters pointing forwards.
                    if (dotThruster > 0.01 && shipRelative.x > 0)
                    {
                        powerX = ThrustPower;
                        //powerY = ThrustPower;
                    }
                    // Thrusters pointing right in the lower right side.
                    else if (crossZThruster < 0 && shipRelative.x > 0 && shipRelative.y < 0)
                    {
                        powerX = ThrustPower;
                        //powerY = ThrustPower;
                    }
                    // Thrusters pointing left in the upper left side.
                    else if (crossZThruster > 0 && shipRelative.x < 0 && shipRelative.y > 0)
                    {
                        powerX = ThrustPower;
                        //powerY = ThrustPower;
                    }
                }
                // If not moving forwards or backwards, but just rotating, fire thrusters pointing right on upper right side and thrusters pointing left on lower left side.
                else if (Mathf.Abs(dot) < float.Epsilon)
                {
                    // Thrusters pointing right in upper right corner.
                    if (crossZThruster < 0 && shipRelative.x > 0 && shipRelative.y > 0)
                    {
                        powerX = ThrustPower;
                        //powerY = ThrustPower;
                    }
                    else if (crossZThruster > 0 && shipRelative.x < 0 && shipRelative.y < 0)
                    {
                        powerX = ThrustPower;
                        //powerY = ThrustPower;
                    }
                }

            }
            else if (crossZ < 0) // Turn right. // TODO I'M RIGHT HERE IN THE SEARCH FOR BETTER MOVEMENT CONTROLS!
            {
                // If moving forwards, fire thrusters pointing backwards on the left side and thrusters pointing left on the upper left side and pointing right on the lower right.
                if (dot > float.Epsilon)
                {

                }
                // If moving backwards, fire thrusters pointing forwards on the left side and thrusters pointing left on the lower left side and pointing right on the upper right.
                else if (dot < float.Epsilon)
                {

                }
                // If not moving forwards or backwards, but just rotating, fire thrusters pointing left on upper left side and thrusters pointing right on lower right side..
                else if (Mathf.Abs(dot) < float.Epsilon)
                {

                }
            }

            var powerTotal = powerX + powerY;
            // DEBUG: Change sprite colour, when power is larger than 0.
            GetComponent<SpriteRenderer>().color = powerTotal > 0 ? Color.magenta : Color.white;
            Core.GetComponent<Rigidbody2D>().AddForceAtPosition((-transform.up) * ThrustPower * (powerTotal), transform.position);
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


            // If not, should ship turn left or right.

            // If turning right, Fire all forward thrusters in lower left quadrant
            // and all thrusters pointing left in upper left quadrant.



            /*float powerX = 0, powerY = 0;
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
            }*/
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