using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEngine.UI;

public class LevelSelectHandler : MonoBehaviour
{

    public GameObject LevelSelectPanel;
    public Text LevelNameText, LevelDescriptionText;
    public Image LevelImage;
    public Sprite[] LevelImages;
    public string[] LevelNames;
    [TextArea(3, 10)]
    public string[] LevelDescriptions;
    public int NumberOfLevels;
    public string[] LevelSceneNames; // The name of the scene file (as would be used in the Application.Load method (no extension)).
    public Color SelectedLevelColour;
    public float MovePauseTime;

    private float elapsedTime;
    private float elapsedMoveTime; // The time elapsed since last move (used to restrict how fast the player can move the selection).
    private int selectedLevel = 0;

    // Use this for initialization
    void Start()
    {
        var gameHandler = GameObject.FindGameObjectWithTag("GameHandler");
        if (gameHandler != null)
        {
            // Set this as the GameHandler's LevelSelectHandler.
            GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().LevelSelectHandler = this;
            // Set colour of default selected level.
            LevelSelectPanel.transform.GetChild(selectedLevel).GetComponent<Image>().color = SelectedLevelColour;
        }
        UpdateSelectedLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // Animated image.
        elapsedTime += Time.deltaTime;

        // Movements.
        var leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        if (leftStickValue.magnitude > 0.2)
        {
            elapsedMoveTime += Time.deltaTime; // Add to the time elapsed since last move.
            if (elapsedMoveTime >= MovePauseTime)
            {
                if (leftStickValue.x > 0)
                {
                    NextLevel();
					GetComponent<AudioSource>().Play ();
                }
                else if (leftStickValue.x < 0)
                {
                    PrevLevel();
					GetComponent<AudioSource>().Play ();
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
        UpdateSelectedLevel();
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
        UpdateSelectedLevel();
    }

    private void UpdateSelectedLevel()
    {
        // Set selected level colour.
        LevelSelectPanel.transform.GetChild(selectedLevel).GetComponent<Image>().color = SelectedLevelColour;
        // Change level image.
        LevelImage.sprite = LevelImages[selectedLevel];
        // Change level title.
        LevelNameText.text = LevelNames[selectedLevel];
        // Change level description.
        LevelDescriptionText.text = LevelDescriptions[selectedLevel];
    }
}
