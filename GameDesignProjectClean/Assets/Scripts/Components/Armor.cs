using UnityEngine;
using System.Collections;
using GamepadInput;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
#if UNITY_EDITOR
/****************
* Editor tools.
****************/

[CustomEditor(typeof(Armor), true)]
public class ArmorEditor : ModuleEditor
{

}
#endif