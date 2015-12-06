using UnityEngine;
using System.Linq;

public class EmptyLevelHandler : MonoBehaviour, ILevelHandler {

    // Spawning.
    public GameObject CorePrefab;
    public float SpawnDistanceToCenter;

    // Use this for initialization
    void Start () {
        var gameHandler = GameObject.FindGameObjectWithTag("GameHandler");
        if (gameHandler != null)
        {
            gameHandler.GetComponent<GameHandler>().LevelHandler = this;
            StartLevel(gameHandler.GetComponent<GameHandler>().GetPlayerShips());
        }
        else
        {
            GameObject[] playerShips = new[]
            {Instantiate(CorePrefab), Instantiate(CorePrefab), Instantiate(CorePrefab), Instantiate(CorePrefab)};
            StartLevel(playerShips);
        }
    }

    public void StartLevel(GameObject[] playerShips)
    {
        int shipCount = playerShips.Count(s => s != null);
        int shipsPlaced = 0;

        if (playerShips[0] != null)
        {
            playerShips[0].transform.position = new Vector3(0, SpawnDistanceToCenter);
            playerShips[0].transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, (360f / shipCount) * shipsPlaced++ + 45);
            playerShips[0].SetActive(true);
        }
        if (playerShips[1] != null)
        {
            playerShips[1].transform.position = new Vector3(0, SpawnDistanceToCenter);
            playerShips[1].transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, (360f / shipCount) * shipsPlaced++ + 45);
            playerShips[1].SetActive(true);
        }
        if (playerShips[2] != null)
        {
            playerShips[2].transform.position = new Vector3(0, SpawnDistanceToCenter);
            playerShips[2].transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, (360f / shipCount) * shipsPlaced++ + 45);
            playerShips[2].SetActive(true);
        }
        if (playerShips[3] != null)
        {
            playerShips[3].transform.position = new Vector3(0, SpawnDistanceToCenter);
            playerShips[3].transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, (360f / shipCount) * shipsPlaced++ + 45);
            playerShips[3].SetActive(true);
        }
    }
}
