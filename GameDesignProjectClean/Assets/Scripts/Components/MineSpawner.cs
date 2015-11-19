using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;

public class MineSpawner : Module
{
    public GameObject MinePrefab;

    public float SpawnOffset;
    public float Cooldown;

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
                    if (GamePad.GetButton(ButtonKey, ShipCore.GetComponent<Core>().ControllerIndex))
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
        GameObject rocket = (GameObject)Instantiate(MinePrefab, transform.position + transform.up * SpawnOffset, transform.rotation);
        rocket.transform.parent = null;

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

[CustomEditor(typeof(MineSpawner), true)]
public class MineSpawnerEditor : ModuleEditor
{

    public override void OnInspectorGUI()
    {
        // Take the module inspector.
        base.OnInspectorGUI();

        MineSpawner launcher = (MineSpawner)target;

        // Selector for projectile prefab

        launcher.MinePrefab = (GameObject)EditorGUILayout.ObjectField("Mine prefab", launcher.MinePrefab, typeof(GameObject), false);
        launcher.SpawnOffset = EditorGUILayout.FloatField("Spawn offset", launcher.SpawnOffset);
        launcher.Cooldown = EditorGUILayout.FloatField("Cooldown", launcher.Cooldown);

    }
}