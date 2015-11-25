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
                    if (GamePad.GetButton(ButtonKey, ShipCore.GetComponent<Core>().ControllerIndex))
                    {
                        Activate();
						GetComponent<AudioSource>().Play();
                    }
                    break;
                case InputKeyType.Trigger:
                    var value = GamePad.GetTrigger(TriggerKey, ShipCore.GetComponent<Core>().ControllerIndex);
                    if (value > 0.5) // Activation threshold.
                    {
                        Activate();
						GetComponent<AudioSource>().Play();
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
		GameObject mine = pool.RequestPoolObject(ObjectPool.ObjectType.Mine, transform.position + transform.up * SpawnOffset, transform.rotation);
        //GameObject mine = (GameObject)Instantiate(MinePrefab, transform.position + transform.up * SpawnOffset, transform.rotation);
        //mine.transform.parent = null;

        // Set common projectile variables.
        var projectile = mine.GetComponent<Projectile>();
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

    public new void DrawCustomInspector()
    {
        MineSpawner launcher = (MineSpawner)target;

        // Selector for projectile prefab

        launcher.MinePrefab = (GameObject)EditorGUILayout.ObjectField("Mine prefab", launcher.MinePrefab, typeof(GameObject), false);
        launcher.SpawnOffset = EditorGUILayout.FloatField("Spawn offset", launcher.SpawnOffset);
        launcher.Cooldown = EditorGUILayout.FloatField("Cooldown", launcher.Cooldown);

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