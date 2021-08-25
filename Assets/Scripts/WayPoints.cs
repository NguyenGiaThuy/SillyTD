using UnityEngine;

public class WayPoints : MonoBehaviour
{
    private Transform[] wayPoints;

    private void Awake()
    {
        wayPoints = new Transform[transform.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = transform.GetChild(i);
        }
    }

    public Transform[] GetWayPoints()
    {
        return wayPoints;
    }
}
