using UnityEngine;
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
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetupWinScreen(bool[] playersJoined, int[] playerPositions)
    {
        for (int i = 0; i < playersJoined.Length; i++)
        {
            Debug.Log("Player " + (i+1) + " joined: " + playersJoined[i] + " pos: " + playerPositions[i] + "\n");
            if (!playersJoined[i]) continue;
            switch (playerPositions[i])
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
                    WinText.text = "Player " + (i+1) + " wins!";
                    break;
            }
        }
        gameObject.SetActive(true);
    }
}
