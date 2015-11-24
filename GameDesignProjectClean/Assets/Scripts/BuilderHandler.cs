using UnityEngine;
using System.Collections;

public class BuilderHandler : MonoBehaviour
{

    public delegate void PreviousScene();
    private PreviousScene prevScene;

    // Use this for initialization
    void Start()
    {
        // Set this as the GameHandler's LevelHandler.
        var gameHandler = GameObject.FindGameObjectWithTag("GameHandler");
        if (gameHandler != null)
        {
            gameHandler.GetComponent<GameHandler>().BuilderHandler = this;
            prevScene = gameHandler.GetComponent<GameHandler>().GotoPrevScene;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool PlayersReady(out GameObject[] playerShips)
    {
        playerShips = null;
        return true;
    }
}
