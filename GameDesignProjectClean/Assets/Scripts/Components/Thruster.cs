using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Thruster : Module
{
    private ParticleSystem[] childParticles;
    public float ThrustPower;
    public bool IsActivated { get; private set; }

    public float DotMargin; // Default 0.01 .
    public float CrossMargin; // Default 0.01 .

    // Use this for initialization
    new void Start()
    {
        base.Start();
        childParticles = gameObject.GetComponentsInChildren<ParticleSystem>();
    }
    public void Update()
    {
        // If in build mode, don't do anything.
        if (ShipCore.GetComponent<Core>().InBuildMode)
        {
            return;
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
                    IsActivated = true;
                    Activate(1);
                }
                else
                {
                    IsActivated = false;
                }

                break;
            case InputKeyType.Trigger:
                var value = GamePad.GetTrigger(TriggerKey, ShipCore.GetComponent<Core>().ControllerIndex);
                if (value > 0)
                {
                    IsActivated = true;
                    Activate(value);
                }
                else
                {
                    IsActivated = false;
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

            // If the power total is larger than 0, the thrusters are active.
            if (powerTotal > 0)
            {
                IsActivated = true;
            }

            ShipCore.GetComponent<Rigidbody2D>().AddForceAtPosition((-transform.up) * ThrustPower * (powerTotal), transform.position);
        }

        // Handle thruster state depending on if it is active or not.
        HandleThrusterState();
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
        return Mathf.Abs(powerX) + Mathf.Abs(powerY); ;
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
    }

    public IEnumerator Disable(float time)
    {
        enabled = false;
        yield return new WaitForSeconds(time);
        enabled = true;
    }

    public void HandleThrusterState()
    {
        if (IsActivated)
        {
            // Activate particles.
            for (int i = 0; i < childParticles.Length; i++)
            {
                if (!childParticles[i].isPlaying)
                    childParticles[i].Play();
            }

            // Activate sound.
            PlayThrusters();

            // Activate colouring.
            GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        else
        {
            // Deactivate particles.
            for (int i = 0; i < childParticles.Length; i++)
            {
                if (childParticles[i].isPlaying)
                    childParticles[i].Stop();
            }

            // Deactivate sounds.
            ThrusterFalloff();
            // Deactivate colouring.
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
	public void PlayThrusters()
	{
		//Duration it takes to have "full" thrust power
		float Duration = 1f;
		
		if (!this.GetComponent<AudioSource> ().isPlaying) {
			this.GetComponent<AudioSource> ().Play ();
			this.GetComponent<AudioSource>().volume = 1;
		} else {
            this.GetComponent<AudioSource>().volume = 1;
            this.GetComponent<AudioSource>().pitch = Mathf.MoveTowards(this.GetComponent<AudioSource>().pitch,2f,Time.deltaTime/Duration);
		}
	}
	
	public void ThrusterFalloff()
	{
		float Duration = 0.25f;
        float VolumeFalloff = 0.25f;
		
		if (!this.GetComponent<AudioSource> ().isPlaying) {
			this.GetComponent<AudioSource> ().Play ();
		} else {
            this.GetComponent<AudioSource>().volume = Mathf.MoveTowards(this.GetComponent<AudioSource>().volume, 0, Time.deltaTime / VolumeFalloff);
			this.GetComponent<AudioSource>().pitch = Mathf.MoveTowards(this.GetComponent<AudioSource>().pitch,0f,Time.deltaTime/Duration);
		}
		
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