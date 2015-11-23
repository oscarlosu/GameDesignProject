using UnityEngine;
using GamepadInput;

public class GameHandler : MonoBehaviour
{
    private static GameObject gameHandler;

    public Scene CurrentScene;
    public GamePad.Button BackButton;

    // Scene handlers.
    public LevelHandler LevelHandler;
    public PlayerSelectHandler PlayerSelectHandler;

    // Game setup settings.
    private bool[] playersJoined;
    private string levelSelectedSceneName;

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
	void Start ()
	{
        DontDestroyOnLoad(this.gameObject); // Don't destroy this object, since it handles the game.
	}
	
	// Update is called once per frame
	void Update ()
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
                        Debug.Log(playersJoined);
                        Application.LoadLevel("LevelSelect");
                        CurrentScene = Scene.LevelSelectScene;
                    }
                    else
                    {
                        Debug.LogError("At least two players are needed.");
                    }
                }
                return;
            case Scene.LevelSelectScene:
                // Level should be selected before progressing to the next scene.
                if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any))
                {
                    if (LevelHandler != null)
                    {
                        // Save the selected scene name.
                        levelSelectedSceneName = LevelHandler.GetSelectedLevelSceneName();
                        Application.LoadLevel("Builder");
                        CurrentScene = Scene.BuilderScene;
                    }
                }
                return;
            case Scene.BuilderScene:
                // All players should have declared themselves ready.
                return;
            case Scene.GameScene:
                // The winning condition should have been met.
                return;
        }
    }
}
