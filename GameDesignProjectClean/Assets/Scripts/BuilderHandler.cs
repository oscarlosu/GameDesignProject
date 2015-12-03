using UnityEngine;
using System.Collections;
using System.Linq;
using GamepadInput;

public class BuilderHandler : MonoBehaviour
{
    public GameObject[] PlayerAreas;
    public GameObject OverlayCanvas;

    public delegate void PreviousScene();
    private PreviousScene prevScene;
    private GameObject[] ships = new GameObject[4];
    private bool[] playersJoined;

    // Use this for initialization
    void Start()
    {
        // Set this as the GameHandler's LevelSelectHandler.
        var gameHandler = GameObject.FindGameObjectWithTag("GameHandler");
        if (gameHandler != null)
        {
            gameHandler.GetComponent<GameHandler>().BuilderHandler = this;
            prevScene = gameHandler.GetComponent<GameHandler>().GotoPrevScene;
            playersJoined = gameHandler.GetComponent<GameHandler>().GetPlayersJoined();
            ActivatePlayerAreas(playersJoined);
        }
        else
        {
            ActivatePlayerAreas(new [] {true, true, true, true});
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (OverlayCanvas.activeSelf && AnyButtonPressed())
        {
            OverlayCanvas.SetActive(false);
        }
    }

    // Activating the player areas for the players, who joined.
    private void ActivatePlayerAreas(bool[] players)
    {
        for (var i = 0; i < players.Length; i++)
        {
            if (players[i])
            {
                PlayerAreas[i].SetActive(true);
            }
        }
    }

    public bool PlayersReady(out GameObject[] playerShips)
    {
        // If the number of players is equal to the number of ships marked ready. Everyone are ready.
        var nbPlayers = playersJoined.Count(player => player);
        var nbShips = ships.Count(ship => ship != null);
        if (nbPlayers == nbShips)
        {
            playerShips = ships;
            return true;
        }
        playerShips = null;
        return false;
    }

    public void SetPlayerShip(int playerIndex, GameObject ship)
    {
        ship.transform.parent = null;
        DontDestroyOnLoad(ship);
        ships[playerIndex] = ship;
        Debug.Log("Player: " + (playerIndex + 1) + " finished: " + ship);
    }

    public void SetPlayerNotReady(int playerIndex)
    {
        if (ships[playerIndex] != null)
        {
            Destroy(ships[playerIndex]);
            ships[playerIndex] = null;
        }
    }

    private bool AnyButtonPressed()
    {
        GamePad.Button? button;
        GamePad.Trigger? trigger;
        return AnyButtonPressed(out button, out trigger);
    }

    private bool AnyButtonPressed(out GamePad.Button? button, out GamePad.Trigger? trigger)
    {
        // Trigger pressed threshold.
        const float triggerThreshold = .5f;
        // Defaults.
        button = null;
        trigger = null;
        // Check for buttons.
        if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
        {
            button = GamePad.Button.A;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
        {
            button = GamePad.Button.B;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Any))
        {
            button = GamePad.Button.X;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Any))
        {
            button = GamePad.Button.Y;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.Back, GamePad.Index.Any))
        {
            button = GamePad.Button.Back;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
        {
            button = GamePad.Button.Start;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder, GamePad.Index.Any))
        {
            button = GamePad.Button.LeftShoulder;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, GamePad.Index.Any))
        {
            button = GamePad.Button.RightShoulder;
            return true;
        }

        // Check for triggers. // TODO Right now, if a trigger is held down before this method. It will think it was pressed.
        if (GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.Any) > triggerThreshold)
        {
            trigger = GamePad.Trigger.LeftTrigger;
            return true;
        }
        if (GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.Any) > triggerThreshold)
        {
            trigger = GamePad.Trigger.RightTrigger;
            return true;
        }

        // No button pressed.
        return false;
    }
}
