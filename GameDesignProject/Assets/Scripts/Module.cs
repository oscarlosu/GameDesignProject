using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;

public abstract class Module : MonoBehaviour
{
    public float Weight;
    public GamePad.Button Button;
    public GamePad.Index Controller;
    public List<GameObject> Sockets;

    public abstract void Activate();

    public void Update()
    {
        if (GamePad.GetButtonDown(Button, Controller))
        {
            Activate();
        }
    }
}
