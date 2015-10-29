using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Asteroid))]
public class AsteroidInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Asteroid asteroid = (Asteroid)target;
        int size = asteroid.Size;
        size = (int)EditorGUILayout.Slider("Size", size, asteroid.SizeMin, asteroid.SizeMax);
        if (size != asteroid.Size)
        {
            asteroid.Size = size;
            asteroid.Initialise();
        }
    }

}
