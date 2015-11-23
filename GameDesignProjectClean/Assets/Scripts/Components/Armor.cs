using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEditor;

public class Armor : Module
{
	public int ExtraHp;
    // Use this for initialization
    new void Start()
    {
        base.Start();
		//Activate();
    }

    // Update is called once per frame
    void Update()
    {
        // If in build mode, don't do anything.
        if (ShipCore.GetComponent<Core>().InBuildMode)
        {
            return;
        }
    }

    public void Activate()
    {
		// Armor increases the maximum hp of the structure its attached, 
		// effectively making every module attached to that same structure more resistant
		transform.parent.GetComponent<Structure>().IncreaseMaxHp(ExtraHp);
    }
}

/****************
* Editor tools.
****************/

[CustomEditor(typeof(Armor), true)]
public class ArmorEditor : ModuleEditor
{

}