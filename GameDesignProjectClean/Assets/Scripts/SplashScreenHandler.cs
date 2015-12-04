using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreenHandler : MonoBehaviour
{

    public Text PressStartText;
    public float TextFlashTime;

    private bool fadeIn;

    private void Start()
    {
        InvokeRepeating("ToggleFadeIn", 0, TextFlashTime);
    }

    private void ToggleFadeIn()
    {
        fadeIn = !fadeIn;
    }

    // Use this for initialization
    private void Update () {
        if (fadeIn)
        {
            PressStartText.GetComponent<Text>().CrossFadeAlpha(1, TextFlashTime / 2, false);
        }
        else
        {
            PressStartText.GetComponent<Text>().CrossFadeAlpha(0, TextFlashTime / 2, false);
        }
    }
}
