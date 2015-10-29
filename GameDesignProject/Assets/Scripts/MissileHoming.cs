using UnityEngine;
using System.Collections;

public class MissileHoming : Projectile
{
    public float ThrusterActivateAt; // The thruser activates after this time period.
    public float ThrustPower;
    public float ThrusterDuration;
    public float TurnSpeed;
    public float ExplosionSpeed;
    public float ExplosionRadius;
    public float GracePeriod;
    public float NewTargetTimer; // The time between each new target selection.
    public int TimesNotTargetingSource; // The number of times new target selection will not select the source.

    private Rigidbody2D rb;
    private float elapsedTime;

    public GameObject Target;
    private int newTargetCount; // How many times a new target has been found.
    private float timeSinceLastTarget;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeSinceLastTarget += Time.deltaTime;
        // Handle thruster
        if (elapsedTime >= ThrusterActivateAt && elapsedTime <= ThrusterDuration + ThrusterActivateAt)
        {
            rb.AddForceAtPosition(transform.up * ThrustPower, transform.position);

            // Select target if no target present.
            if (Target == null)
            {
                SelectClosestTarget();
            }
            else
            {
                // Handle turning.
                Vector3 moveDirection = Target.transform.position - transform.position;
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
                var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, TurnSpeed);
            }
            // If there is still fuel in this basterd, select new target, when the new target timer runs out.
            if (timeSinceLastTarget > NewTargetTimer)
            {
                SelectClosestTarget();
                timeSinceLastTarget = 0;
            }
        }


    }

    private void SelectClosestTarget()
    {
        var ships = GameObject.FindGameObjectsWithTag("Ship");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject curShip in ships)
        {
            // If the ship is the source ship, don't target, if within the first couple of target selection rounds.
            if (TimesNotTargetingSource > newTargetCount && SourceShip.GetInstanceID() == curShip.GetInstanceID())
            {
                newTargetCount++;
                continue;
            }
            Vector3 diff = curShip.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = curShip;
                distance = curDistance;
            }
        }
        Target = closest;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Structure")
        {
            // Ignore source ship for a short period of time after the missile is fired
            if (other.gameObject != SourceStructure || elapsedTime >= GracePeriod)
            {
                // Make ship lose a module
                Structure str = other.gameObject.GetComponent<Structure>();
                str.LoseModule();
                // Destroy Missile
                GameObject.Destroy(this.gameObject);
            }
        }
        else if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Asteroid>().Breakdown();
            // Destroy Missile
            GameObject.Destroy(this.gameObject);
        }
    }
}
