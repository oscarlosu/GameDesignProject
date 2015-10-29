using UnityEngine;
using System.Collections;
using GamepadInput;

public class LaserGun : Module
{
    public GamePad.Button Button;
    public GamePad.Index Controller;
    public GameObject LaserPrefab;

    public float Cooldown;
    public float MinLaserBreadth, MaxLaserBreadth;
    public float MinLaserLength, MaxLaserLength;
    public float MinChargeTime, MaxChargeTime;


    private float elapsedTime;
    private bool ready;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        ready = true;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle chare up
        if (ready && GamePad.GetButtonDown(Button, Controller))
        {
            elapsedTime = 0;
        }
        else if (ready && GamePad.GetButton(Button, Controller))
        {
            elapsedTime += Time.deltaTime;
        }
        else if (ready && GamePad.GetButtonUp(Button, Controller))
        {
            elapsedTime += Time.deltaTime;
            //Debug.Log("ElapsedTime = " + elapsedTime);
            Activate();
        }

        // Handle cooldown
        if (!ready)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= Cooldown)
            {
                ready = true;
                //Debug.Log("Ready!");
                elapsedTime = 0;
            }
        }

    }

    public void Activate()
    {
        ready = false;
        //Debug.Log("Activated!");
        // Instantiate laser
        GameObject laser = (GameObject)Instantiate(LaserPrefab, transform.position, transform.rotation);
        // Calculate breadth and length of laser and scale
        Vector2 scale = calculateLaserScale();
        laser.transform.localScale = scale;
        //Debug.Log("ScaleFactor = " + scale);
        // Calculate size of laser and position accordingly
        Vector2 sprSize = laser.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Vector2 size = new Vector2(scale.x * sprSize.x, scale.y * sprSize.y);
        //Debug.Log("LaserSize = " + size);
        laser.transform.position += laser.transform.up * size.y / 2;
        // Save source structure
        laser.GetComponent<Laser>().SourceStructure = Sockets[0];
        // Reset elapsedTime
        elapsedTime = 0;
    }

    Vector2 calculateLaserScale()
    {
        float normalizedElapsedTime = Mathf.Clamp(elapsedTime, 0, MaxChargeTime) / MaxChargeTime;
        float factorBreadth = Mathf.Lerp(MinLaserBreadth, MaxLaserBreadth, normalizedElapsedTime);
        float factorLength = Mathf.Lerp(MinLaserLength, MaxLaserLength, normalizedElapsedTime);
        return new Vector2(factorBreadth, factorLength);
    }
}
