using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (BoxCollider2D))]
[RequireComponent(typeof (AudioSource))]
public class HomingMissile : Projectile
{
    public float ThrusterActivateAt; // The thruser activates after this time period.
    public float ThrustPower;
    public float ThrusterDuration;
    public float TurnSpeed;
    public float GracePeriod;
    public float NewTargetTimer; // The time between each new target selection.
    public int TimesNotTargetingSource; // The number of times new target selection will not select the source.

    public GameObject ExplosionPrefab;

    private Rigidbody2D rb;
    private float elapsedTime;

    public GameObject Target;
    private int newTargetCount; // How many times a new target has been found.
    private float timeSinceLastTarget;

    private ParticleSystem[] childParticles;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
		InGrace = true;
        childParticles = gameObject.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < childParticles.Length; i++)
        {
            if (!childParticles[i].isPlaying)
                childParticles[i].Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
		// Update elapsed time until the mine is active
		if(elapsedTime <= ThrusterDuration)
		{
			elapsedTime += Time.deltaTime;
		}
        else
        {
            for (int i = 0; i < childParticles.Length; i++)
            {
                if (childParticles[i].isPlaying)
                    childParticles[i].Stop();
            }
        }
		// Handle grace period
		if(InGrace)
		{
			if(elapsedTime > GracePeriod)
			{
				InGrace = false;
			}
		}   
		timeSinceLastTarget += Time.deltaTime;
        // Handle thruster
        if (elapsedTime >= ThrusterActivateAt && elapsedTime <= ThrusterDuration + ThrusterActivateAt)
        {
            rb.AddForceAtPosition(transform.up*ThrustPower, transform.position);

            // Select target if no target present.
            if (Target == null)
            {
                SelectClosestTarget();
            }
            else
            {
                // Handle turning.
                Vector3 moveDirection = Target.transform.position - transform.position;
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x)*Mathf.Rad2Deg - 90;
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
        var ships = GameObject.FindGameObjectsWithTag(GlobalValues.ShipTag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject curShip in ships)
        {
            // If the ship is the source ship, don't target, if within the first couple of target selection rounds.
            if (TimesNotTargetingSource > newTargetCount && SourceCore.GetInstanceID() == curShip.GetInstanceID())
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

    void OnCollisionEnter2D(Collision2D other)
    {
		// With something that is not the source ship or after the grace period
		if (!InGrace || other.gameObject.GetInstanceID() != SourceStructure.GetInstanceID())
        {
            //Debug.Log("Homing missile detected collision with " + other.gameObject.name);
            Activate();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
	{
		// Missiles only activate with shield or laser triggers
		if ((other.gameObject.GetComponent<Shield>() != null && (!InGrace  || other.gameObject.GetComponent<Shield>().ShipCore.GetInstanceID() != SourceCore.GetInstanceID()))
		    || other.gameObject.GetComponent<Laser>() != null)
		{
            //Debug.Log("Homing missile trigger detected trigger with " + other.gameObject.name);
            Activate();
        }
    }

    void Activate()
    {
        // When destroyed, create an explosion, which damages objects around it.
        var explosion = GameObject.Instantiate(ExplosionPrefab);
        explosion.transform.position = transform.position;
        explosion.GetComponent<Explosion>().Damage = Damage;
        GameObject.Destroy(gameObject);
    }
}
