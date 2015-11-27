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
    public Sprite SpriteForward, SpriteLeft, SpriteRight;
    public ModuleDirection SpriteDirection;
    public Direction FacingDirection { get; set; }
    public Direction ParentDirection { get; set; }

    private SpriteRenderer spriteRenderer;
	protected ObjectPool pool;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        UpdateSprite();
    }

	protected void OnEnable()
	{
		pool = GameObject.FindGameObjectWithTag(GlobalValues.ObjectPoolTag).GetComponent<ObjectPool>();
	}

    protected void OnDestroy()
    {
        if (ShipCore != null)
        {
            ShipCore.GetComponent<Rigidbody2D>().mass -= Mass;
            ShipCore.GetComponent<Core>().NbOfModules--;
        }
    }

    public void RotateModule()
    {
        switch (FacingDirection)
        {
            case Direction.Up:
                RotateModuleTo(ParentDirection != Direction.Right ? Direction.Right : Direction.Down);
                break;
            case Direction.Right:
                RotateModuleTo(ParentDirection != Direction.Down ? Direction.Down : Direction.Left);
                break;
            case Direction.Down:
                RotateModuleTo(ParentDirection != Direction.Left ? Direction.Left : Direction.Up);
                break;
            case Direction.Left:
                RotateModuleTo(ParentDirection != Direction.Up ? Direction.Up : Direction.Right);
                break;
            
        }
    }

    public void RotateModuleTo(Direction direction)
    {
        Debug.Log("Rotate from: " + FacingDirection + ", rotate to: " + direction);
        // Can't rotate the module to point directly at its parent structure.
        if (direction != ParentDirection)
        {
            switch (ParentDirection)
            {
                case Direction.Up:
                    switch (direction)
                    {
                        case Direction.Down:
                            SpriteDirection = ModuleDirection.Forward;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 180);
                            break;                                        
                        case Direction.Right:                             
                            SpriteDirection = ModuleDirection.Left;       
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 270);
                            break;                                        
                        case Direction.Left:                              
                            SpriteDirection = ModuleDirection.Right;      
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 90);
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (direction)
                    {
                        case Direction.Up:
                            SpriteDirection = ModuleDirection.Forward;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 0);
                            break;
                        case Direction.Right:
                            SpriteDirection = ModuleDirection.Right;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 270);
                            break;
                        case Direction.Left:
                            SpriteDirection = ModuleDirection.Left;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 90);
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (direction)
                    {
                        case Direction.Down:
                            SpriteDirection = ModuleDirection.Right;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 180);
                            break;
                        case Direction.Right:
                            SpriteDirection = ModuleDirection.Forward;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 270);
                            break;
                        case Direction.Up:
                            SpriteDirection = ModuleDirection.Left;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 0);
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (direction)
                    {
                        case Direction.Down:
                            SpriteDirection = ModuleDirection.Left;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 180);
                            break;
                        case Direction.Left:
                            SpriteDirection = ModuleDirection.Forward;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 90);
                            break;
                        case Direction.Up:
                            SpriteDirection = ModuleDirection.Right;
                            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 0);
                            break;
                    }
                    break;
            }
            UpdateSprite();
            FacingDirection = direction;
        }
    }

    // Update the sprite.
    public void UpdateSprite()
    {
        // If rotation is possible, rotate sprite if necessary.
        if (CanSpriteRotate)
        {
            switch (SpriteDirection)
            {
                case ModuleDirection.Forward:
                    spriteRenderer.sprite = SpriteForward;
                    break;
                case ModuleDirection.Left:
                    spriteRenderer.sprite = SpriteLeft;
                    break;
                case ModuleDirection.Right:
                    spriteRenderer.sprite = SpriteRight;
                    break;
            }
        }
    }

    // Direction used for selecting correct sprite.
    public enum ModuleDirection
    {
        Forward, Left, Right
    }

    // Direction used for selecting correct sprite.
    public enum Direction
    {
        Up, Down, Left, Right
    }

}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Module), true)]
public class ModuleEditor : ShipComponentEditor
{

    protected new void DrawCustomInspector()
    {
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
        EditorGUILayout.HelpBox("Can sprite rotate should be true, if the modules needs extra sprites for the rotation.", MessageType.Info);
        // If it can rotate, also show sprite variables.
        if (module.CanSpriteRotate)
        {
            module.SpriteForward = (Sprite)EditorGUILayout.ObjectField("Sprite forward", module.SpriteForward, typeof (Sprite), false);
            module.SpriteLeft = (Sprite)EditorGUILayout.ObjectField("Sprite left", module.SpriteLeft, typeof (Sprite), false);
            module.SpriteRight = (Sprite)EditorGUILayout.ObjectField("Sprite right", module.SpriteRight, typeof (Sprite), false);
        }

        // Add a separator between the module settings and the settings of the actual module.
        EditorGUILayout.Separator();

        // If anything was changed, set the object to dirty, so Unity will save the values.
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