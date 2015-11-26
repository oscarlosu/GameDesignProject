using UnityEngine;
using System.Collections;

public class MoonLevelController : MonoBehaviour, ILevelHandler
{
    public Transform moonRotator;
    public float speed = 0.0f;
	// Use this for initialization
	void Start () {
        moonRotator.eulerAngles = new Vector3(0, 0, Random.Range(30, 330));
        var gameHandler = GameObject.FindGameObjectWithTag("GameHandler");
        if (gameHandler != null)
        {
            gameHandler.GetComponent<GameHandler>().LevelHandler = this;
            StartLevel(gameHandler.GetComponent<GameHandler>().GetPlayerShips());
        }
    }

    public void StartLevel(GameObject[] playerShips)
    {
        if (playerShips[0] != null)
        {
            playerShips[0].transform.position = new Vector3(-100, 20);
            playerShips[0].transform.eulerAngles = new Vector3(0, 0, 45);
            playerShips[0].SetActive(true);
        }
        if (playerShips[1] != null)
        {
            playerShips[1].transform.position = new Vector3(-70, 20);
            playerShips[1].transform.eulerAngles = new Vector3(0, 0, 315);
            playerShips[1].SetActive(true);
        }
        if (playerShips[2] != null)
        {
            playerShips[2].transform.position = new Vector3(-100, -20);
            playerShips[2].transform.eulerAngles = new Vector3(0, 0, 135);
            playerShips[2].SetActive(true);
        }
        if (playerShips[3] != null)
        {
            playerShips[3].transform.position = new Vector3(-70, -20);
            playerShips[3].transform.eulerAngles =  new Vector3(0, 0, 225);
            playerShips[3].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
        moonRotator.Rotate(0, 0, speed * Time.deltaTime);
	}
}