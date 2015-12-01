using System.Linq;
using UnityEngine;
using GamepadInput;
using Debug = UnityEngine.Debug;

public class GameHandler : MonoBehaviour
{
    private static GameObject gameHandler;

    public Scene CurrentScene;
    public GamePad.Button BackButton;
    public GameObject WinScreenPrefab;
    public float SecondsToWinScreen;

    // Scene handlers.
    public PlayerSelectHandler PlayerSelectHandler;
    public LevelSelectHandler LevelSelectHandler;
    public BuilderHandler BuilderHandler;
    public ILevelHandler LevelHandler;

    // Game setup settings.
    private bool[] playersJoined;
    private string levelSelectedSceneName;
    private GameObject[] playerShips;
    private int playersLost = 0;
    private int[] playerLostPositions;
    private int[] originalNbOfModules = new int[4];
    private int[] modulesLeft = new int[4];
    private int[] modulesDestroyed = new int[4];

    // Other settings.
    private bool GameOver;

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
        playerLostPositions = new[] { 0, 0, 0, 0 };
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
                if (LevelSelectHandler != null && GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
                {
                    // Save the selected scene name.
                    levelSelectedSceneName = LevelSelectHandler.GetSelectedLevelSceneName();
                    Application.LoadLevel("Builder");
                    CurrentScene = Scene.BuilderScene;
                }
                return;
            case Scene.BuilderScene:
                // If all players have declared themselves ready.
                if (BuilderHandler != null && BuilderHandler.PlayersReady(out playerShips))
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
                    CurrentScene = Scene.GameScene;
                }
                return;
            case Scene.GameScene:
                // The winning condition should have been met.
                if (GameOver && GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
                {
                    // Destroy all the ships.
                    foreach (var ship in playerShips)
                    {
                        GameObject.Destroy(ship);
                    }
                    // Restart the game.
                    Application.LoadLevel("PlayerSelect");
                    CurrentScene = Scene.PlayerSelectScene;
                }
                return;
        }
    }

    public bool[] GetPlayersJoined()
    {
        return playersJoined;
    }

    public GameObject[] GetPlayerShips()
    {
        return playerShips;
    }

    public void PlayerLost(GamePad.Index controllerIndex)
    {
        var totalPlayersJoined = playersJoined.Count(t => t);
        // Save that the player lost.
        switch (controllerIndex)
        {
            case GamePad.Index.One:
                playerLostPositions[0] = (totalPlayersJoined - 1) - playersLost++;
                break;
            case GamePad.Index.Two:
                playerLostPositions[1] = (totalPlayersJoined - 1) - playersLost++;
                break;
            case GamePad.Index.Three:
                playerLostPositions[2] = (totalPlayersJoined - 1) - playersLost++;
                break;
            case GamePad.Index.Four:
                playerLostPositions[3] = (totalPlayersJoined - 1) - playersLost++;
                break;
        }

        Debug.Log("Total players: " + totalPlayersJoined + " players lost: " + playersLost);

        // If only one player is left, the game is over.
        if (playersLost == totalPlayersJoined - 1)
        {
            for (int i = 0; i < playersJoined.Length; i++)
            {
                if (!playersJoined[i])
                    continue;
                modulesLeft[i] = playerShips[i].GetComponent<Core>().NbOfModules;
                modulesDestroyed[i] = playerShips[i].GetComponent<Core>().ModulesDestroyed;
            }
            Invoke("DisplayWinScreen", SecondsToWinScreen);
        }
    }

    public void DisplayWinScreen()
    {
        WinScreen winScreen = Instantiate(WinScreenPrefab).GetComponent<WinScreen>();
        winScreen.SetupWinScreen(playersJoined, playerLostPositions, originalNbOfModules, modulesLeft, modulesDestroyed);
        GameOver = true;
    }

    public void NumberOfModules(GamePad.Index controllerIndex, int nbOfModules)
    {
        switch (controllerIndex)
        {
            case GamePad.Index.One:
                originalNbOfModules[0] = nbOfModules;
                break;               
            case GamePad.Index.Two:  
                originalNbOfModules[1] = nbOfModules;
                break;               
            case GamePad.Index.Three:
                originalNbOfModules[2] = nbOfModules;
                break;               
            case GamePad.Index.Four: 
                originalNbOfModules[3] = nbOfModules;
                break;
        }
    }
}
