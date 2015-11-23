using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{

    public GameObject LevelSelectPanel;
    public int NumberOfLevels;
    public string[] LevelSceneNames; // The name of the scene file (as would be used in the Application.Load method (no extension)).
    public Color SelectedLevelColour;
    public float MovePauseTime;

    private float elapsedMoveTime; // The time elapsed since last move (used to restrict how fast the player can move the selection).
    private int selectedLevel = 0;

    // Use this for initialization
    void Start()
    {
        // Set this as the GameHandler's LevelHandler.
        GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().LevelHandler = this;
        // Set colour of default selected level.
        LevelSelectPanel.transform.GetChild(selectedLevel).GetComponent<Image>().color = SelectedLevelColour;
    }

    // Update is called once per frame
    void Update()
    {

        var leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        if (leftStickValue.magnitude > 0.1)
        {
            elapsedMoveTime += Time.deltaTime; // Add to the time elapsed since last move.
            if (elapsedMoveTime >= MovePauseTime)
            {
                if (leftStickValue.x > 0)
                {
                    NextLevel();
                }
                else if (leftStickValue.x < 0)
                {
                    PrevLevel();
                }
                elapsedMoveTime = 0; // Reset the timer after move.
            }
        }

    }

    public string GetSelectedLevelSceneName()
    {
        if (selectedLevel >= 0 && selectedLevel < NumberOfLevels)
        {
            return LevelSceneNames[selectedLevel];
        }
        return null;
    }

    private void PrevLevel()
    {
        LevelSelectPanel.transform.GetChild(selectedLevel).GetComponent<Image>().color = Color.white;
        if (selectedLevel > 0)
        {
            selectedLevel--;
        }
        else
        {
            selectedLevel = NumberOfLevels - 1;
        }
        LevelSelectPanel.transform.GetChild(selectedLevel).GetComponent<Image>().color = SelectedLevelColour;
    }

    private void NextLevel()
    {
        LevelSelectPanel.transform.GetChild(selectedLevel).GetComponent<Image>().color = Color.white;
        if (selectedLevel < NumberOfLevels - 1)
        {
            selectedLevel++;
        }
        else
        {
            selectedLevel = 0;
        }
        LevelSelectPanel.transform.GetChild(selectedLevel).GetComponent<Image>().color = SelectedLevelColour;
    }
}
