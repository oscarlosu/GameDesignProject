using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    // Sprites.
    public Sprite[] CharacterWonSprites;
    public Sprite[] CharacterLostSprites;

    // Sprite and text objects.
    public Image FirstImage, SecondImage, ThirdImage, FourthImage;
    public Text FirstScore, SecondScore, ThirdScore, FourthScore;
    public Text WinText;
    public float TextFlashTime;

    private bool fadeIn;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("ToggleFadeIn", 0, TextFlashTime);
    }

    private void ToggleFadeIn()
    {
        fadeIn = !fadeIn;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            WinText.GetComponent<Text>().CrossFadeAlpha(1, TextFlashTime / 2, false);
        }
        else
        {
            WinText.GetComponent<Text>().CrossFadeAlpha(0, TextFlashTime / 2, false);
        }
    }

    public void SetupWinScreen(bool[] playersJoined, int[] playerPositions, int[] originalNbOfModules, int[] modulesLeft, int[] modulesDestroyed, bool[] selfDestruct)
    {
        // Calculate points.
        int nbPlayersJoined = playersJoined.Count(t => t);
        int[] points = new int[4];
        points[0] = playersJoined[0] ? (selfDestruct[0] && modulesLeft[0] == originalNbOfModules[0] ? 0 : modulesLeft[0]) + modulesDestroyed[0] : -1;
        points[1] = playersJoined[1] ? (selfDestruct[1] && modulesLeft[1] == originalNbOfModules[1] ? 0 : modulesLeft[1]) + modulesDestroyed[1] : -1;
        points[2] = playersJoined[2] ? (selfDestruct[2] && modulesLeft[2] == originalNbOfModules[2] ? 0 : modulesLeft[2]) + modulesDestroyed[2] : -1;
        points[3] = playersJoined[3] ? (selfDestruct[3] && modulesLeft[3] == originalNbOfModules[3] ? 0 : modulesLeft[3]) + modulesDestroyed[3] : -1;

        // Calculate positions.
        int[] pointsForPos = new int[4];
        points.CopyTo(pointsForPos,0);
        int[] pointPos = new int[4];
        int currentPos = 0;
        for (int p = 0; p < nbPlayersJoined; p++)
        {
            int largest = -1;
            int largestIndex = 0;
            for (int i = 0; i < pointPos.Length; i++)
            {
                if (pointsForPos[i] > largest)
                {
                    largest = pointsForPos[i];
                    largestIndex = i;
                }
            }
            
            pointPos[largestIndex] = currentPos++;
            pointsForPos[largestIndex] = -1;
        }

        for (int i = 0; i < playersJoined.Length; i++)
        {

            Debug.Log("Player " + (i + 1) + " rank: " + pointPos[i] + "\nself destruct: " + selfDestruct[i] + "\nmodules left: " + modulesLeft[i] + "\nmodules destroyed: " + modulesDestroyed[i]);
            if (!playersJoined[i]) continue;

            switch (pointPos[i])
            {
                case 3:
                    FourthImage.sprite = CharacterLostSprites[i];
                    FourthScore.text = "Score " + points[i];
                    FourthImage.gameObject.SetActive(true);
                    break;
                case 2:
                    ThirdImage.sprite = CharacterLostSprites[i];
                    ThirdScore.text = "Score " + points[i];
                    ThirdImage.gameObject.SetActive(true);
                    break;
                case 1:
                    SecondImage.sprite = CharacterLostSprites[i];
                    SecondScore.text = "Score " + points[i];
                    SecondImage.gameObject.SetActive(true);
                    break;
                case 0:
                    FirstImage.sprite = CharacterWonSprites[i];
                    FirstScore.text = "Score " + points[i];
                    FirstImage.gameObject.SetActive(true);
                    WinText.text = "Player " + (i + 1) + " wins!\nPress start to continue";
                    break;
            }
        }
        gameObject.SetActive(true);
    }
}
