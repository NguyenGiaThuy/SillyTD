using UnityEngine;

public class Mob : MonoBehaviour
{
    public static int Counter { get; private set; } = 0;

    public delegate void OnDeadHandler(Mob mob);
    public static OnDeadHandler OnDead;

    public enum ArmorType {
        Light,
        Medium,
        Heavy
    }

    [Header("Game Specifications", order = 0)]
    public ArmorType armor;
    public int killCredits;
    public int killResearchPoints;
    public int lifeDamage;
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float movementSpeed;

    [Header("Unity Specifications", order = 0)]
    [SerializeField]
    private GameObject hitEffectPrefab;

    private Transform[] wayPointsToMove;
    private int nextWayPointIndex;
    private Vector3 nextDirection;


    private void Awake()
    {
        Counter++;
    }

    private void FixedUpdate()
    {
        transform.Translate(nextDirection.normalized * movementSpeed * Time.fixedDeltaTime, Space.World);
        Quaternion lookRotation = Quaternion.LookRotation(nextDirection);
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime);

        // If destination is reached, find next destination 
        Transform nextWayPoint = wayPointsToMove[nextWayPointIndex];
        if (Vector3.Distance(nextWayPoint.position, transform.position) <= 0.1f)
            if (nextWayPointIndex + 1 < wayPointsToMove.Length)
            {
                nextWayPointIndex++;
                nextWayPoint = wayPointsToMove[nextWayPointIndex];
                nextDirection = nextWayPoint.position - transform.position;
            }
    }

    private void OnDestroy()
    {
        Counter--;
    }

    // Called from WaveSpawner
    public void SetPath(WayPoints wayPoints)
    {
        wayPointsToMove = wayPoints.GetWayPoints();
        nextWayPointIndex = 0;
        nextDirection = wayPointsToMove[nextWayPointIndex].position - transform.position;
        transform.rotation = Quaternion.LookRotation(nextDirection);
    }

    // Called from AttackTurret
    public float GetCurrentPathLength()
    {
        float pathLength = 0f;
        int tempNextWayPointIndex = nextWayPointIndex;
        Vector3 currentPosition = transform.position;
        Vector3 nextWayPointPosition = wayPointsToMove[tempNextWayPointIndex].position;

        // Calculate the total length of moving path
        while (tempNextWayPointIndex < wayPointsToMove.Length)
        {
            pathLength += Vector3.Distance(currentPosition, nextWayPointPosition);
            tempNextWayPointIndex++;

            if (tempNextWayPointIndex < wayPointsToMove.Length)
            {
                currentPosition = nextWayPointPosition;
                nextWayPointPosition = wayPointsToMove[tempNextWayPointIndex].position;
            }
        }

        return pathLength;
    }

    public void Hit(int damage) {
        healthPoints -= damage;
        hitEffectPrefab.GetComponent<ParticleSystem>().Play();

        if (healthPoints <= 0) {
            // Play animation
            // Play sound effect
            OnDead?.Invoke(this);
            Destroy(gameObject, 0.25f);
        }
    }
}