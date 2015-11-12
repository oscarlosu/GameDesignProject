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
        // If the ship hasn't been set yet, don't do ANYTHING!
        if (Core == null)
        {
            return;
        }
        // Handle normal input.
        if (ready)
        {
            switch (InputType)
            {
                case InputKeyType.Button:
                    if (GamePad.GetButton(ButtonKey, Core.GetComponent<Core>().ControllerIndex))
                    {
                        Activate();
                    }
                    break;
                case InputKeyType.Trigger:
                    var value = GamePad.GetTrigger(TriggerKey, Core.GetComponent<Core>().ControllerIndex);
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
        GameObject missile = (GameObject)Instantiate(RocketPrefab, transform.position + RocketLaunchPosOffset * transform.up, transform.rotation);
        missile.transform.parent = null;
        missile.GetComponent<Rigidbody2D>().velocity = Core.GetComponent<Rigidbody2D>().velocity + (Vector2)(transform.up * RocketLaunchSpeed);
    }
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(RocketLauncher), true)]
public class RocketLauncherEditor : ModuleEditor
{

    public override void OnInspectorGUI()
    {
        // Take the module inspector.
        base.OnInspectorGUI();

        RocketLauncher launcher = (RocketLauncher)target;

        // Selector for projectile prefab

        launcher.RocketPrefab = (GameObject)EditorGUILayout.ObjectField("Rocket prefab", launcher.RocketPrefab, typeof(GameObject), false);
        launcher.Cooldown = EditorGUILayout.FloatField("Cooldown", launcher.Cooldown);
        launcher.RocketLaunchSpeed = EditorGUILayout.FloatField("Rocket launch speed", launcher.RocketLaunchSpeed);
        launcher.RocketLaunchPosOffset = EditorGUILayout.FloatField("Rocket launch offset", launcher.RocketLaunchPosOffset);

    }
}