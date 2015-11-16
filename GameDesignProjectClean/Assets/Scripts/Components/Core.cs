using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
public class Core : Structure
{
    public GamepadInput.GamePad.Index ControllerIndex;

    // Public methods

    // Use this for initialization
    public new void Start()
    {
        base.Start();
        // Add core and structure mass to rigidbody
        Core = this.gameObject;
        Assemble();
    }

    // Update is called once per frame
    public void Update()
    {

    }

    public void Assemble()
    {
        // Set initial mass to core mass.
        GetComponent<Rigidbody2D>().mass = Mass;
        // Retrieve all children
        List<GameObject> children = RetrieveChildren(gameObject);
        // Set itself as the ship of each component
        foreach (GameObject child in children)
        {
            if(child.GetComponent<ShipComponent>() != null)
            {
                Debug.Log("ShipComponend mass: " + child.GetComponent<ShipComponent>().Mass);
                child.GetComponent<ShipComponent>().Core = gameObject;
                GetComponent<Rigidbody2D>().mass += child.GetComponent<ShipComponent>().Mass;                
            }
        }            
    }

    // Private methods
    private List<GameObject> RetrieveChildren(GameObject go)
    {
        List<GameObject> myChildren = new List<GameObject>();
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            GameObject myChild = go.transform.GetChild(i).gameObject;
            // Add your own child
            myChildren.Add(myChild);
            // Add your child's children
            myChildren.AddRange(RetrieveChildren(myChild));
        }
        return myChildren;
    }
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Core), true)]
public class CoreEditor : StructureEditor
{

    public override void OnInspectorGUI()
    {
        // Display the module's settings.
        base.OnInspectorGUI();

        // Create heading.
        GUIStyle heading = new GUIStyle { fontSize = 14 };
        EditorGUILayout.LabelField("Core settings", heading);

        // Get target and show/edit fields.
        Core t = (Core)target;
        t.ControllerIndex = (GamepadInput.GamePad.Index)EditorGUILayout.EnumPopup("Controller", t.ControllerIndex);

        // If the target was changed, set the target to dirty, so Unity will save the values.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
