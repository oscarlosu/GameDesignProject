using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;

public class RocketLauncher : Module
{
    public GameObject RocketPrefab;

    public float Cooldown;
    public float RocketLaunchSpeed;
    public float RocketLaunchPosOffset;

    private float elapsedTime;
    private bool ready;

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
        if (ready)
        {
            switch (InputType)
            {
                case InputKeyType.Button:
                    if (GamePad.GetButtonDown(ButtonKey, ShipCore.GetComponent<Core>().ControllerIndex))
                    {
                        Activate();
                    }
                    break;
                case InputKeyType.Trigger:
                    var value = GamePad.GetTrigger(TriggerKey, ShipCore.GetComponent<Core>().ControllerIndex);
                    if (value > 0.5) // Activation threshold.
                    {
                        Activate();
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
                elapsedTime = 0;
            }
        }

    }

    public void Activate()
    {
        ready = false;
        GameObject rocket = (GameObject)Instantiate(RocketPrefab, transform.position + RocketLaunchPosOffset * transform.up, transform.rotation);
        rocket.transform.parent = null;
        rocket.GetComponent<Rigidbody2D>().velocity = ShipCore.GetComponent<Rigidbody2D>().velocity + (Vector2)(transform.up * RocketLaunchSpeed);

        // Set common projectile variables.
        var projectile = rocket.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SourceCore = ShipCore;
            projectile.SourceStructure = transform.parent.gameObject;
        }
    }
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(RocketLauncher), true)]
public class RocketLauncherEditor : ModuleEditor
{

    public new void DrawCustomInspector()
    {
        RocketLauncher launcher = (RocketLauncher)target;

        // Selector for projectile prefab

        launcher.RocketPrefab = (GameObject)EditorGUILayout.ObjectField("Rocket prefab", launcher.RocketPrefab, typeof(GameObject), false);
        launcher.Cooldown = EditorGUILayout.FloatField("Cooldown", launcher.Cooldown);
        launcher.RocketLaunchSpeed = EditorGUILayout.FloatField("Rocket launch speed", launcher.RocketLaunchSpeed);
        launcher.RocketLaunchPosOffset = EditorGUILayout.FloatField("Rocket launch offset", launcher.RocketLaunchPosOffset);

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