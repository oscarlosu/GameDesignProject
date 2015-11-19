using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
public class Module : ShipComponent
{

    // Enum that references key types.
    public enum InputKeyType
    {
        Button, Trigger
    }

    // The type of input (button or trigger) that is mapped to the module along with the actual key.
    public InputKeyType InputType;
    public GamepadInput.GamePad.Button ButtonKey;
    public GamepadInput.GamePad.Trigger TriggerKey;

    public bool CanSpriteRotate;
    public Sprite SpriteForward, SpriteSideways;
    public Direction SpriteDireciton;

    private SpriteRenderer spriteRenderer;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        UpdateSprite();
    }

    protected void OnDestroy()
    {
        if (ShipCore != null)
        {
            ShipCore.GetComponent<Rigidbody2D>().mass -= Mass;
        }
    }

    // Update the sprite.
    public void UpdateSprite()
    {
        // If rotation is possible, rotate sprite if necessary.
        if (CanSpriteRotate)
        {
            switch (SpriteDireciton)
            {
                case Direction.Forward:
                    spriteRenderer.sprite = SpriteForward;
                    break;
                case Direction.Sideway:
                    spriteRenderer.sprite = SpriteSideways;
                    break;
            }
        }
    }

    // Direction used for instance for selecting correct sprite.
    public enum Direction
    {
        Forward, Sideway
    }

}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Module), true)]
public class ModuleEditor : ShipComponentEditor
{

    public override void OnInspectorGUI()
    {

        // The component GUI.
        base.OnInspectorGUI();

        Module module = (Module)target;

        // Create a heading.
        GUIStyle heading = new GUIStyle { fontSize = 14 };
        EditorGUILayout.LabelField("Module settings", heading);

        // Show list of input types and display the list of keys for that input type.
        module.InputType = (Module.InputKeyType)EditorGUILayout.EnumPopup("Input type", module.InputType);
        switch (module.InputType)
        {
            case Module.InputKeyType.Button:
                module.ButtonKey = (GamepadInput.GamePad.Button)EditorGUILayout.EnumPopup("Input key", module.ButtonKey);
                break;
            case Module.InputKeyType.Trigger:
                module.TriggerKey =
                    (GamepadInput.GamePad.Trigger)EditorGUILayout.EnumPopup("Input key", module.TriggerKey);
                break;
        }
        // Select if the module can rotate (has more than one sprite direction).
        module.CanSpriteRotate = EditorGUILayout.Toggle("Can sprite rotate", module.CanSpriteRotate);
        // If it can rotate, also show sprite variables.
        if (module.CanSpriteRotate)
        {
            module.SpriteForward = (Sprite)EditorGUILayout.ObjectField("Sprite forward", module.SpriteForward, typeof (Sprite), false);
            module.SpriteSideways = (Sprite)EditorGUILayout.ObjectField("Sprite sideways", module.SpriteSideways, typeof (Sprite), false);
        }

        // Add a separator between the module settings and the settings of the actual module.
        EditorGUILayout.Separator();

        // If anything was changed, set the object to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}