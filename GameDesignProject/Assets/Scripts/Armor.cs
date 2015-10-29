using UnityEngine;
using System.Collections;

public class Armor : Module {

    public void Activate()
    {
        GameObject.Destroy(this.gameObject);
    }

}
