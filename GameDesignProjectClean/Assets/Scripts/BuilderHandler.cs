using UnityEngine;
using System.Collections;
using System.Linq;

public class BuilderHandler : MonoBehaviour
{
    public GameObject[] PlayerAreas;

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
        ships[playerIndex] = null;
    }
}
