using UnityEngine;
using System.Collections;
using GamepadInput;

public enum GamePadKey
{
    // Buttons
    A = 1,
    B = 2,
    X = 3,
    Y = 4,
    LeftShoulder = 5,
    RightShoulder = 6,
    LeftStickButton = 7,
    RightStickButton = 8,
    Back = 9,
    Start = 10,
    // Triggers
    LeftTrigger = 11,
    RightTrigger = 12,
    // Axis
    DPad = 13,
    LeftStick = 14,
    RightStick = 15
}
public class GamePadInputWrapper
{
    public static GamePad.Button TranslateButton(GamePadKey key)
    {
        switch(key)
        {
            case GamePadKey.A:
                return GamePad.Button.A;                
            case GamePadKey.B:
                return GamePad.Button.B;                
            case GamePadKey.X:
                return GamePad.Button.X;                
            case GamePadKey.Y:
                return GamePad.Button.Y;                
            case GamePadKey.LeftShoulder:
                return GamePad.Button.LeftShoulder;
            case GamePadKey.RightShoulder:
                return GamePad.Button.RightShoulder;                
            case GamePadKey.LeftStickButton:
                return GamePad.Button.LeftStick;                
            case GamePadKey.RightStickButton:
                return GamePad.Button.RightStick;                
            case GamePadKey.Back:
                return GamePad.Button.Back;
            case GamePadKey.Start:
                return GamePad.Button.Start;
            default:
                throw new System.Exception("Invalid GamePadKey code.");
        }
    }

    public static GamePad.Trigger TranslateTrigger(GamePadKey key)
    {
        switch (key)
        {
            case GamePadKey.LeftTrigger:
                return GamePad.Trigger.LeftTrigger;                
            case GamePadKey.RightTrigger:
                return GamePad.Trigger.RightTrigger;
            default:
                throw new System.Exception("Invalid GamePadKey code.");
        }
    }

    public static GamePad.Axis TranslateAxis(GamePadKey key)
    {
        switch (key)
        {
            case GamePadKey.DPad:
                return GamePad.Axis.Dpad;                
            case GamePadKey.LeftStick:
                return GamePad.Axis.LeftStick;                
            case GamePadKey.RightStick:
                return GamePad.Axis.RightStick;
            default:
                throw new System.Exception("Invalid GamePadKey code.");
        }
    }

    

    public static bool GetButtonDown(GamePadKey key, GamePad.Index controller)
    {
        // Button
        if((int)key <= 10 && (int)key >= 1)
        {
            return GamePad.GetButtonDown(TranslateButton(key), controller);
        }
        // Anything else is not a button and should not be passed as an argument to this method
        else
        {
            return false;
        }      
    }
    public static bool GetButtonUp(GamePadKey key, GamePad.Index controller)
    {
        // Button
        if ((int)key <= 10 && (int)key >= 1)
        {
            return GamePad.GetButtonUp(TranslateButton(key), controller);
        }
        // Anything else is not a button and should not be passed as an argument to this method
        else
        {
            return false;
        }
    }
    public static bool GetButton(GamePadKey key, GamePad.Index controller)
    {
        // Button
        if ((int)key >= 1 && (int)key <= 10)
        {
            return GamePad.GetButton(TranslateButton(key), controller);
        }
        // Anything else is not a button and should not be passed as an argument to this method
        else
        {
            return false;
        }
    }

    public static float GetTrigger(GamePadKey key, GamePad.Index controller)
    {
        // Button
        if ((int)key >= 11 && (int)key < 13)
        {
            return GamePad.GetTrigger(TranslateTrigger(key), controller);
        }
        // Anything else is not a trigger and should not be passed as an argument to this method
        else
        {
            return 0.0f;
        }
    }

    public static Vector2 GetAxis(GamePadKey key, GamePad.Index controller)
    {
        // Button
        if ((int)key >= 13 && (int)key <= 15)
        {
            return GamePad.GetAxis(TranslateAxis(key), controller);
        }
        // Anything else is not an axis and should not be passed as an argument to this method
        else
        {
            return Vector2.zero;
        }
    }
}
