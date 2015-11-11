using UnityEngine;
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
        // If the ship hasn't been set yet, don't do ANYTHING!
        if (Ship == null)
        {
            return;
        }
        // Handle normal input.
        switch (InputType)
        {
            case InputKeyType.Button:
                if (GamePad.GetButton(ButtonKey, Ship.GetComponent<Ship>().ControllerIndex))
                {
                    Activate(1);
                    // DEBUG: Change sprite colour, when power is larger than 0.
                    GetComponent<SpriteRenderer>().color = Color.magenta;
                }
                else
                {
                    // DEBUG: Change sprite colour, when deactivating.
                    GetComponent<SpriteRenderer>().color = Color.white;
                }

                break;
            case InputKeyType.Trigger:
                var value = GamePad.GetTrigger(TriggerKey, Ship.GetComponent<Ship>().ControllerIndex);
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
                }
                break;
        }

        // Handle thumb stick input.
        Vector2 leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, Ship.GetComponent<Ship>().ControllerIndex);
        if (leftStickValue.magnitude > 0)
        {
            float powerX = 0, powerY = 0;
            var shipRelative = Ship.transform.InverseTransformPoint(transform.position);
            float dot = Vector3.Dot(Ship.transform.up, transform.up);
            if (dot < -0.01) // Thruster is backwards thruster.
            {
                if (leftStickValue.y < 0)
                {
                    if (leftStickValue.x < -float.Epsilon)
                    {
                        if (shipRelative.x < 0) // On the left side.
                        {
                            powerX = -leftStickValue.x;
                        }
                    }
                    else if (leftStickValue.x > float.Epsilon)
                    {
                        if (shipRelative.x > 0) // On the right side.
                        {
                            powerX = leftStickValue.x;
                        }
                    }
                    else // stick x practically in the middle.
                    {
                        powerY = -leftStickValue.y;
                    }
                }
                else if (leftStickValue.y > -float.Epsilon && leftStickValue.y < float.Epsilon)
                {
                    if (leftStickValue.x < 0 && shipRelative.x < 0) // On right side.
                    {
                        powerX = -leftStickValue.x;
                    }
                    else if (leftStickValue.x > 0 && shipRelative.x > 0) // On left side.
                    {
                        powerX = leftStickValue.x;
                    }
                }
            }
            else if (dot > 0.01) // Thruster is forwards thruster.
            {
                //Debug.Log("Forwards thruster");
                if (leftStickValue.y > 0)
                {
                    powerY = leftStickValue.y;

                    if (leftStickValue.x < 0 && shipRelative.x < 0) // On right side.
                    {
                        //Debug.Log("Forwards thruster on right side.");
                        powerX = leftStickValue.x;
                    }
                    else if (leftStickValue.x > 0 && shipRelative.x > 0) // On left side.
                    {
                        //Debug.Log("Forward thrusters on left side.");
                        powerX = -leftStickValue.x;
                    }
                }
                else if (leftStickValue.y < float.Epsilon && leftStickValue.y > -float.Epsilon)
                {
                    if (leftStickValue.x > 0 && shipRelative.x < 0)
                    {
                        powerX = leftStickValue.x;
                    }
                    if (leftStickValue.x < 0 && shipRelative.x > 0)
                    {
                        powerX = -leftStickValue.x;
                    }
                }

            }
            else // Thruster is sideways thruster.
            {
                //Debug.Log("Sideways thruster");
                // NOTE: Thumbstick left is negative z


                // Thruster above middle
                // - Thruster is on left side
                // - - Thruster is pointing left

                // Thruster below middle
                // - Thruster is on right side
                // - - Thruster is pointing right

                // Thruster below middle
                // - Thruster is on left side
                // - - Thruster is pointing down.

                // Thruster above middle
                // - Thruster is on right side
                // - - Thruster is pointing up.


                if (shipRelative.y >= 0) // Thruster above middle.
                {
                    if (shipRelative.x > 0) // Thruster on right side.
                    {
                        if (Vector3.Cross(Ship.transform.up, transform.up).z < 0) // Thruster pointing left.
                        {
                            Debug.Log("Above-right-left");
                            if (leftStickValue.x > 0 && leftStickValue.y >= 0)
                            {
                                powerX = leftStickValue.x;
                            }
                            else if (leftStickValue.x < 0 && leftStickValue.y < -float.Epsilon)
                            {
                                powerX = -leftStickValue.x;
                            }
                            else
                            {
                                powerX = 0;
                            }
                        }
                        else // Thruster pointing right.
                        {
                            Debug.Log("Above-right-right");
                            if (leftStickValue.x < 0 && leftStickValue.y >= 0)
                            {
                                powerX = -leftStickValue.x;
                            }
                            else if (leftStickValue.x > 0 && leftStickValue.y < -float.Epsilon)
                            {
                                powerX = leftStickValue.x;
                            }
                            else
                            {
                                powerX = 0;
                            }
                        }
                    }
                    else // Thruster on left side.
                    {
                        if (Vector3.Cross(Ship.transform.up, transform.up).z < 0) // Thruster pointing left.
                        {
                            Debug.Log("Above-left-left");
                            if (leftStickValue.x > 0 && leftStickValue.y >= 0)
                            {
                                powerX = leftStickValue.x;
                            }
                            else if (leftStickValue.x < 0 && leftStickValue.y < -float.Epsilon)
                            {
                                powerX = -leftStickValue.x;
                            }
                            else
                            {
                                powerX = 0;
                            }
                        }
                        else // Thruster pointing right.
                        {
                            Debug.Log("Above-left-right");
                            if (leftStickValue.x < 0 && leftStickValue.y >= 0)
                            {
                                powerX = -leftStickValue.x;
                            }
                            else if (leftStickValue.x > 0 && leftStickValue.y < -float.Epsilon)
                            {
                                powerX = leftStickValue.x;
                            }
                            else
                            {
                                powerX = 0;
                            }
                        }
                    }
                }
                else // Thruster below middle.
                {
                    if (shipRelative.x > 0) // Thruster on right side.
                    {
                        if (Vector3.Cross(Ship.transform.up, transform.up).z < 0) // Thruster pointing left.
                        {
                            Debug.Log("Below-right-left");
                            if (leftStickValue.x < 0 && leftStickValue.y >= 0)
                            {
                                powerX = -leftStickValue.x;
                            }
                            else if (leftStickValue.x > 0 && leftStickValue.y < 0)
                            {
                                powerX = leftStickValue.x;
                            }
                            else
                            {
                                powerX = 0;
                            }
                        }
                        else // Thruster pointing right.
                        {
                            Debug.Log("Below-right-right");
                            if (leftStickValue.x > 0 && leftStickValue.y >= 0)
                            {
                                powerX = leftStickValue.x;
                            }
                            else if (leftStickValue.x < 0 && leftStickValue.y < 0)
                            {
                                powerX = -leftStickValue.x;
                            }
                            else
                            {
                                powerX = 0;
                            }
                        }
                    }
                    else // Thruster on left side.
                    {
                        if (Vector3.Cross(Ship.transform.up, transform.up).z < 0) // Thruster pointing left.
                        {
                            Debug.Log("Below-left-left");
                            if (leftStickValue.x < 0 && leftStickValue.y >= 0)
                            {
                                powerX = -leftStickValue.x;
                            }
                            else if (leftStickValue.x > 0 && leftStickValue.y < 0)
                            {
                                powerX = leftStickValue.x;
                            }
                            else
                            {
                                powerX = 0;
                            }
                        }
                        else // Thruster pointing right.
                        {
                            Debug.Log("Below-left-right");
                            if (leftStickValue.x > 0 && leftStickValue.y >= 0)
                            {
                                powerX = leftStickValue.x;
                            }
                            else if (leftStickValue.x < 0 && leftStickValue.y < 0)
                            {
                                powerX = -leftStickValue.x;
                            }
                            else
                            {
                                powerX = 0;
                            }
                        }
                    }
                }
            }
            // DEBUG: Change sprite colour, when power is larger than 0.
            GetComponent<SpriteRenderer>().color = powerX + powerY > 0 ? Color.magenta : Color.white;
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