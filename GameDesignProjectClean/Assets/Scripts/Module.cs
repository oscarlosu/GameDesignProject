using UnityEngine;
using System.Collections;
using UnityEditor;

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

}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Module), true)]
public class ModuleEditor : Editor
{
    private Module module;
    private bool toggleInput = false;

    private void Awake()
    {
        module = (Module)target;
    }

    public override void OnInspectorGUI()
    {

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
        // Add a separator between the module settings and the settings of the actual module.
        EditorGUILayout.Separator();

        // If anything was changed, set the object to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}