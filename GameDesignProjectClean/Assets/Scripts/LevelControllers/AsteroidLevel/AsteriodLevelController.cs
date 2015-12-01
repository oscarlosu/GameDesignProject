using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteriodLevelController : MonoBehaviour, ILevelHandler
{
	public BezierSpline Path;

	public float Speed;
	public float AsteroidRespawnDist;


	public float GetPathLength()
	{
		return Vector3.Distance (Path.GetControlPoint(0), Path.GetControlPoint(Path.ControlPointCount - 1));
	}


    // Use this for initialization
    void Start ()
	{
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
            playerShips[0].transform.position = new Vector3(15, -15);
            playerShips[0].transform.eulerAngles = new Vector3(0, 0, 45);
            playerShips[0].SetActive(true);
        }
        if (playerShips[1] != null)
        {
            playerShips[1].transform.position = new Vector3(45, -15);
            playerShips[1].transform.eulerAngles = new Vector3(0, 0, 315);
            playerShips[1].SetActive(true);
        }
        if (playerShips[2] != null)
        {
            playerShips[2].transform.position = new Vector3(15, -30);
            playerShips[2].transform.eulerAngles = new Vector3(0, 0, 135);
            playerShips[2].SetActive(true);
        }
        if (playerShips[3] != null)
        {
            playerShips[3].transform.position = new Vector3(45, -30);
            playerShips[3].transform.eulerAngles = new Vector3(0, 0, 225);
            playerShips[3].SetActive(true);
        }
    }
}