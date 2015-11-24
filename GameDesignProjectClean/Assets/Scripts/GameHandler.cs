using System.Linq;
using UnityEngine;
using GamepadInput;

public class GameHandler : MonoBehaviour
{
    private static GameObject gameHandler;

    public Scene CurrentScene;
    public GamePad.Button BackButton;

    // Scene handlers.
    public PlayerSelectHandler PlayerSelectHandler;
    public LevelHandler LevelHandler;
    public BuilderHandler BuilderHandler;

    // Game setup settings.
    private bool[] playersJoined;
    private string levelSelectedSceneName;
    private GameObject[] playerShips;

    public enum Scene
    {
        PlayerSelectScene, LevelSelectScene, BuilderScene, GameScene
    }

    private void Awake()
    {
        if (gameHandler != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            gameHandler = this.gameObject;
        }
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject); // Don't destroy this object, since it handles the game.
    }

    // Update is called once per frame
    void Update()
    {
        GotoPrevScene(); // Check if going back.
        GotoNextScene(); // Check if going forward.
    }

    public void GotoPrevScene()
    {
        switch (CurrentScene)
        {
            case Scene.PlayerSelectScene:
                // Can't go further back... Don't do anything.
                return;
            case Scene.LevelSelectScene:
                // Go to PlayerSelectScene and reset level select values.
                if (GamePad.GetButtonDown(BackButton, GamePad.Index.Any))
                {
                    Application.LoadLevel("PlayerSelect");
                    CurrentScene = Scene.PlayerSelectScene;
                }
                return;
            case Scene.BuilderScene:
                // Go to LevelSelectScene and reset builder values.
                return;
            case Scene.GameScene:
                // Go back to the PlayerSelectScene and reset all values.
                return;
        }
    }

    public void GotoNextScene()
    {
        switch (CurrentScene)
        {
            case Scene.PlayerSelectScene:
                // Check if there are more than one player before going to the next scene.
                if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
                {
                    if (PlayerSelectHandler.PlayersJoined(out playersJoined))
                    {
                        Application.LoadLevel("LevelSelect");
                        CurrentScene = Scene.LevelSelectScene;
                    }
                    else
                    {
                        Debug.LogError("At least two players are needed...");
                    }
                }
                return;
            case Scene.LevelSelectScene:
                // Level should be selected before progressing to the next scene.
                if (LevelHandler != null && GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
                {
                    // Save the selected scene name.
                    levelSelectedSceneName = LevelHandler.GetSelectedLevelSceneName();
                    Application.LoadLevel("Builder");
                    CurrentScene = Scene.BuilderScene;
                }
                return;
            case Scene.BuilderScene:
                // All players should have declared themselves ready.
                if (BuilderHandler != null && GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
                {
                    // If all players have declared themselves ready.
                    if (BuilderHandler.PlayersReady(out playerShips))
                    {
                        // All the ships build need to become persistant, so they can move to the level.
                        // They should also be disabled, so they don't hit anything upon entering the next level.
                        foreach (var ship in playerShips.Where(ship => ship != null))
                        {
                            //DontDestroyOnLoad(ship);
                            ship.SetActive(false);
                        }

                        // Load the selected level.
                        Application.LoadLevel(levelSelectedSceneName);

                        // DEBUG. // TODO Change positions via a specific level handler later.
                        if (playerShips[0] != null)
                        {
                            playerShips[0].transform.position = new Vector3(-10, 10);
                            playerShips[0].SetActive(true);
                        }
                        if (playerShips[1] != null)
                        {
                            playerShips[1].transform.position = new Vector3(10, 10);
                            playerShips[1].SetActive(true);
                        }
                        if (playerShips[2] != null)
                        {
                            playerShips[2].transform.position = new Vector3(-10, -10);
                            playerShips[2].SetActive(true);
                        }
                        if (playerShips[3] != null)
                        {
                            playerShips[3].transform.position = new Vector3(10, -10);
                            playerShips[3].SetActive(true);
                        }
                    }
                    else
                    {
                        Debug.LogError("Not all players are ready yet...");
                    }
                }
                return;
            case Scene.GameScene:
                // The winning condition should have been met.
                return;
        }
    }

    public bool[] GetPlayersJoined()
    {
        return playersJoined;
    }
}
