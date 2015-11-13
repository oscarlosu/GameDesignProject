using UnityEngine;
using UnityEditor;

public class ShipComponent : MonoBehaviour
{
    // For builder.
    public string ComponentName;
    public Sprite BuilderSprite;

    // For game.
    public GameObject Core;
    public int Mass;
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(ShipComponent), true)]
public class ShipComponentEditor : Editor
{

    public override void OnInspectorGUI()
    {
        ShipComponent component = (ShipComponent)target;

        // Create a heading.
        GUIStyle heading = new GUIStyle { fontSize = 14 };
        EditorGUILayout.LabelField("Ship component settings", heading);

        component.ComponentName = EditorGUILayout.TextField("Component name", component.ComponentName);
        component.BuilderSprite = (Sprite)EditorGUILayout.ObjectField("Builder sprite", component.BuilderSprite, typeof(Sprite), false);

        // Add a separator between the component settings and the settings of the actual component.
        EditorGUILayout.Separator();

        // If anything was changed, set the object to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}