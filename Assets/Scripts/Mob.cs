using UnityEngine;

public class Mob : MonoBehaviour
{
    public static int Counter { get; private set; } = 0;

    public enum ArmorType {
        Light,
        Medium,
        Heavy
    }

    [Header("Game Specifications", order = 0)]
    public ArmorType armor;
    public int healthPoints;
    public float movementSpeed;
    public int killCredits;
    public int killResearchPoints;
    public int lifeDamage;

    [Header("Unity Specifications", order = 0)]
    public Transform[] wayPointsToMove;
    public int nextWayPointIndex;
    public Vector3 nextDirection;
    
    private void Awake()
    {
        Counter++;
    }

    private void FixedUpdate()
    {
        transform.Translate(nextDirection.normalized * movementSpeed * Time.deltaTime, Space.World);
        //Quaternion lookRotation = Quaternion.LookRotation(nextDirection);
        //transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

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

        if (healthPoints <= 0) {
            // Play animation
            // Play sound effect
            Destroy(gameObject);
        }
    }
}
