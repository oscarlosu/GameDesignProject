using UnityEngine;
using GamepadInput;

public class GameHandler : MonoBehaviour
{

    public Scene CurrentScene;
    public GamePad.Button BackButton; 

    public enum Scene
    {
        PlayerSelectScene, LevelSelectScene, BuilderScene, GameScene
    }

	// Use this for initialization
	void Start ()
	{
        DontDestroyOnLoad(this.gameObject); // Don't destroy this object, since it handles the game.
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GotoNextScene()
    {
        switch (CurrentScene)
        {
            case Scene.PlayerSelectScene:
                // Check if there are more than one player before going to the next scene.
                break;
            case Scene.LevelSelectScene:
                // Level should be selected before progressing to the next scene.
                break;
            case Scene.BuilderScene:
                // All players should have declared themselves ready.
                break;
            case Scene.GameScene:
                // The winning condition should have been met.
                break;
        }
    }
}
