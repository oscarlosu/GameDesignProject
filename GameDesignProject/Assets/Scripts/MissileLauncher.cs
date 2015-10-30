using UnityEngine;
using System.Collections;
using GamepadInput;

public class MissileLauncher : Module
{
    public GamePad.Button Button;
    public GamePad.Index Controller;
    public GameObject MissilePrefab;

    public float Cooldown;
    public float MissileLaunchSpeed;
    public float MissileLaunchPosOffset;

    private float elapsedTime;
    private bool ready;
    private SpriteRenderer rend;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        ready = true;
        elapsedTime = 0;
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ready && GamePad.GetButtonDown(Button, Controller))
        {
            Activate();
        }
        // Handle cooldown
        if(!ready)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= Cooldown)
            {
                ready = true;
                //rend.enabled = true;
                elapsedTime = 0;
            }
        }
        
    }

    public void Activate()
    {
        ready = false;
        //rend.enabled = false;
        GameObject missile = (GameObject)Instantiate(MissilePrefab, transform.position + MissileLaunchPosOffset * transform.up, transform.rotation);
        missile.transform.parent = null;
        missile.GetComponent<Rigidbody2D>().velocity = rb.velocity + (Vector2)(transform.up * MissileLaunchSpeed);
        missile.GetComponent<Projectile>().SourceStructure = Sockets[0];
        missile.GetComponent<Projectile>().SourceShip = Ship;
    }
}
