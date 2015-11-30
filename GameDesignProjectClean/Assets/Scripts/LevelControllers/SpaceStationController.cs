﻿using UnityEngine;
using System.Collections;

public class SpaceStationController : MonoBehaviour, ILevelHandler
{
    public Vector2 laserSizeStation;
    public Transform[] ssParts;

    public Transform Charge1;
    public Transform Charge2;

    private ParticleSystem[] charge1Systems;
    private ParticleSystem[] charge2Systems;

    public float rotModifier;
    public float chargeTime;
    private float waitTime = 0.0f;
    private float count = 0.0f;
    private bool charge1 = false;


    public void StartLevel(GameObject[] playerShips)
    {
        if (playerShips[0] != null)
        {
            playerShips[0].transform.position = new Vector3(-35, 35);
            playerShips[0].transform.eulerAngles = new Vector3(0, 0, 45);
            playerShips[0].SetActive(true);
        }
        if (playerShips[1] != null)
        {
            playerShips[1].transform.position = new Vector3(35, 35);
            playerShips[1].transform.eulerAngles = new Vector3(0, 0, 315);
            playerShips[1].SetActive(true);
        }
        if (playerShips[2] != null)
        {
            playerShips[2].transform.position = new Vector3(-35, -35);
            playerShips[2].transform.eulerAngles = new Vector3(0, 0, 135);
            playerShips[2].SetActive(true);
        }
        if (playerShips[3] != null)
        {
            playerShips[3].transform.position = new Vector3(35, -35);
            playerShips[3].transform.eulerAngles = new Vector3(0, 0, 225);
            playerShips[3].SetActive(true);
        }
    }

    // Use this for initialization
    void Start () {
        waitTime = Random.Range(15, 25);
        charge1Systems = Charge1.GetComponentsInChildren<ParticleSystem>();
        charge2Systems = Charge2.GetComponentsInChildren<ParticleSystem>();
        for(int i = 0; i < charge1Systems.Length; i++)
        {
            charge1Systems[i].Stop();
            charge2Systems[i].Stop();
        }
    }
	
	// Update is called once per frame
	void Update () {
        rotModifier = Mathf.Abs(rotModifier);
        for(int i = 0; i < ssParts.Length; i++)
        {
            int j = 6 - i;
            ssParts[i].Rotate(new Vector3(0, 0, Mathf.Sin(Time.realtimeSinceStartup * (j * rotModifier))));
            rotModifier = -rotModifier;
        }

        if (count >= waitTime)
        {
            waitTime = Random.Range(15, 25) + chargeTime;
            count = 0.0f;
            StartCoroutine(FireLaser());
        }
        count += Time.deltaTime;
	}

    IEnumerator FireLaser()
    {  
        if(charge1)
        {
            for (int i = 0; i < charge1Systems.Length; i++)
            {
                charge1Systems[i].Play();
            }
            charge1 = false;
        }
        else
        {
            for (int i = 0; i < charge2Systems.Length; i++)
            {
                charge2Systems[i].Play();
            }
            charge1 = true;
        }

        yield return new WaitForSeconds(chargeTime);
        
        for (int i = 0; i < charge1Systems.Length; i++)
        {
            charge1Systems[i].Stop();
            charge2Systems[i].Stop();
        }
        BroadcastMessage("FireLaserGun", laserSizeStation);
    }
}