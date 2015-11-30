﻿using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    // Sprites.
    public Sprite[] CharacterWonSprites;
    public Sprite[] CharacterLostSprites;

    // Sprite and text objects.
    public Image FirstImage, SecondImage, ThirdImage, FourthImage;
    public Text WinText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupWinScreen(bool[] playersJoined, int[] playerPositions, int[] originalNbOfModules, int[] modulesLeft, int[] modulesDestroyed)
    {
        // Calculate points.
        int[] points = new int[4];
        points[0] = playersJoined[0] ? modulesLeft[0] + modulesDestroyed[0] : -1;
        points[1] = playersJoined[1] ? modulesLeft[1] + modulesDestroyed[1] : -1;
        points[2] = playersJoined[2] ? modulesLeft[2] + modulesDestroyed[2] : -1;
        points[3] = playersJoined[3] ? modulesLeft[3] + modulesDestroyed[3] : -1;

        // Calculate positions.
        int[] pointPos = new int[4];
        int currentPos = 0;
        while (true)
        {
            int largest = -1;
            int largestIndex = 0;
            for (int i = 0; i < pointPos.Length; i++)
            {
                if (points[i] > largest)
                {
                    largest = points[i];
                    largestIndex = i;
                }
            }
            if (largest == -1)
            {
                break;
            }
            pointPos[largestIndex] = currentPos++;
            points[largestIndex] = -1;
        }

        for (int i = 0; i < playersJoined.Length; i++)
        {
            Debug.Log("Player " + (i + 1) + " joined: " + playersJoined[i] + " pos: " + playerPositions[i] + "\n");
            if (!playersJoined[i]) continue;

            switch (pointPos[i])
            {
                case 3:
                    FourthImage.sprite = CharacterLostSprites[i];
                    FourthImage.gameObject.SetActive(true);
                    break;
                case 2:
                    ThirdImage.sprite = CharacterLostSprites[i];
                    ThirdImage.gameObject.SetActive(true);
                    break;
                case 1:
                    SecondImage.sprite = CharacterLostSprites[i];
                    SecondImage.gameObject.SetActive(true);
                    break;
                case 0:
                    FirstImage.sprite = CharacterWonSprites[i];
                    FirstImage.gameObject.SetActive(true);
                    WinText.text = "Player " + (i + 1) + " wins!";
                    break;
            }
        }
        gameObject.SetActive(true);
    }
}
