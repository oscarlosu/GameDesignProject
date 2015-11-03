using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;

[CustomEditor(typeof(Ship))]
public class ShipInspector : Editor
{

    private GamepadInput.GamePad.Index controller;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("Remember to structure the hierarchy correctly before linking all parts!", MessageType.Info);
        controller = (GamepadInput.GamePad.Index)EditorGUILayout.EnumPopup("Controller", controller);
        if (GUILayout.Button("Link parts with ship"))
        {
            // Get target ship.
            Ship ship = (Ship)target;

            // Get core module.
            var core = ship.CoreStructureModule;

            // Clear all links.
            ClearAllLinks(core.gameObject);

            // Create new links.
            core.Ship = ship.gameObject;
            core.rb = ship.GetComponent<Rigidbody2D>();
            LinkWithChildren(ship.gameObject, core.gameObject);
        }

        if (GUILayout.Button("Clear links"))
        {
            // Get target ship.
            Ship ship = (Ship)target;

            // Get core module.
            var core = ship.CoreStructureModule;

            // Clear all links.
            ClearAllLinks(core.gameObject);
        }
    }

    private void ClearAllLinks(GameObject currentObj)
    {
        currentObj.GetComponent<Module>().Sockets.Clear();
        currentObj.GetComponent<Module>().Ship = null;
        currentObj.GetComponent<Module>().rb = null;
        currentObj.GetComponent<Module>().Controller = GamepadInput.GamePad.Index.Any;
        // Find all modules attached to the core module.
        for (var childId = 0; childId < currentObj.transform.childCount; childId++)
        {
            var module = currentObj.transform.GetChild(childId); // Get the module.
            ClearAllLinks(module.gameObject);
        }
    }

    private void LinkWithChildren(GameObject ship, GameObject currentObj)
    {
        // Find all modules attached to the core module.
        for (var childId = 0; childId < currentObj.transform.childCount; childId++)
        {
            var module = currentObj.transform.GetChild(childId); // Get the module.
            // Link core module to all those module and visa versa.
            currentObj.GetComponent<Module>().Sockets.Add(module.gameObject); // Add the module to the socket of the current object.
            module.GetComponent<Module>().Sockets.Add(currentObj.gameObject); // Add current object to the socket of the module.
            module.GetComponent<Module>().Ship = ship; // Add the ship reference to the module.
            module.GetComponent<Module>().rb = ship.GetComponent<Rigidbody2D>(); // Add the ship's rigid body to the module.
            // Set the controller of the module.
            module.GetComponent<Module>().Controller = controller;
            // Go into all structure modules attached to the core module.
            if (module.GetComponent<Structure>() != null)
            {
                LinkWithChildren(ship, module.gameObject);
            }
        }
    }
}
