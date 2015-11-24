using UnityEngine;
using System.Collections;
using System.Linq;
using GamepadInput;
using UnityEngine.UI;

public class PlayerSelectHandler : MonoBehaviour
{

    public GamePad.Button JoinButton, ReadyButton;
    public GameObject PlayerOnePanel, PlayerTwoPanel, PlayerThreePanel, PlayerFourPanel;
    public Sprite PlayerOneSprite, PlayerTwoSprite, PlayerThreeSprite, PlayerFourSprite;

    private bool[] playersJoined = new bool[4];

	// Use this for initialization
	void Start () {
        // Set this as the GameHandler's PlayerSelectHandler.
        GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().PlayerSelectHandler = this;
    }
	
	// Update is called once per frame
	void Update () {
	    if (PlayerJoined(GamePad.Index.One))
	    {
            playersJoined[0] = true;
	        PlayerOnePanel.transform.GetChild(0).GetComponent<Image>().sprite = PlayerOneSprite;
	        PlayerOnePanel.transform.GetChild(1).gameObject.SetActive(false);
	    }
        if (PlayerJoined(GamePad.Index.Two))
        {
            playersJoined[1] = true;
            PlayerTwoPanel.transform.GetChild(0).GetComponent<Image>().sprite = PlayerTwoSprite;
            PlayerTwoPanel.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (PlayerJoined(GamePad.Index.Three))
        {
            playersJoined[2] = true;
            PlayerThreePanel.transform.GetChild(0).GetComponent<Image>().sprite = PlayerThreeSprite;
            PlayerThreePanel.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (PlayerJoined(GamePad.Index.Four))
        {
            playersJoined[3] = true;
            PlayerFourPanel.transform.GetChild(0).GetComponent<Image>().sprite = PlayerFourSprite;
            PlayerFourPanel.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private bool PlayerJoined(GamePad.Index controllerIndex)
    {
        if (GamePad.GetButtonDown(JoinButton, controllerIndex))
        {
            return true;
        }
        return false;
    }

    public bool PlayersJoined(out bool[] playersJoined)
    {
        playersJoined = this.playersJoined;
        var nbOfPlayersJoined = playersJoined.Count(pJoined => pJoined);
        // The minimum number of players is two, so only return true, if there are more than two players.
        return nbOfPlayersJoined >= 2;
    }

}
