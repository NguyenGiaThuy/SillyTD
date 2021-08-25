using UnityEngine;

public class Mob : MonoBehaviour
{
    [Header("Specifications")]
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private int killCredit;
    [SerializeField]
    private int killResearchPoint;

    private Transform[] wayPointsToMove;
    private int nextWayPointIndex;
    private Vector3 nextDirection;

    private void FixedUpdate()
    {
        transform.Translate(nextDirection.normalized * movementSpeed * Time.deltaTime, Space.World);
        Quaternion lookRotation = Quaternion.LookRotation(nextDirection);
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // If destination is reached, find next destination 
        Transform nextWayPoint = wayPointsToMove[nextWayPointIndex];
        if (Vector3.Distance(nextWayPoint.position, transform.position) <= 0.1f)
        {
            if (nextWayPointIndex + 1 < wayPointsToMove.Length)
            {
                nextWayPointIndex++;
                nextWayPoint = wayPointsToMove[nextWayPointIndex];
                nextDirection = nextWayPoint.position - transform.position;
            }
        }
    }

    private void OnDestroy()
    {
        PlayerStats.credit += killCredit;
    }

    public void SetPath(WayPoints wayPoints)
    {
        wayPointsToMove = wayPoints.GetWayPoints();
        nextWayPointIndex = 0;
        nextDirection = wayPointsToMove[nextWayPointIndex].position - transform.position;
        transform.rotation = Quaternion.LookRotation(nextDirection);
    }

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
}
