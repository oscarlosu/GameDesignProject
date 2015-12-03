using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShipComponent : MonoBehaviour
{
    // For builder.
    public string ComponentName;
    public Sprite BuilderSprite;

    // For game.
    public GameObject ShipCore;
    public int Mass;
}

/****************
* Editor tools.
****************/
#if UNITY_EDITOR
[CustomEditor(typeof(ShipComponent), true)]
public class ShipComponentEditor : Editor
{
    protected bool DefaultInspectorOpen, CustomInspectorOpen;

    protected void DrawCustomInspector()
    {
        ShipComponent component = (ShipComponent)target;

        // Create a heading.
        GUIStyle heading = new GUIStyle { fontSize = 14 };
        EditorGUILayout.LabelField("Ship component settings", heading);

        component.ComponentName = EditorGUILayout.TextField("Component name", component.ComponentName);
        component.Mass = EditorGUILayout.IntField("Mass", component.Mass);
        component.BuilderSprite = (Sprite)EditorGUILayout.ObjectField("Builder sprite", component.BuilderSprite, typeof(Sprite), false);
    }

    public override void OnInspectorGUI()
    {

        DefaultInspectorOpen = EditorGUILayout.Foldout(DefaultInspectorOpen, "Default inspector (Hardcore mode)");
        if (DefaultInspectorOpen)
        {
            DrawDefaultInspector();
        }

        CustomInspectorOpen = EditorGUILayout.Foldout(CustomInspectorOpen, "Custom inspector (Easy mode)");
        if (CustomInspectorOpen)
        {
            DrawCustomInspector();
        }

        // If anything was changed, set the object to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif