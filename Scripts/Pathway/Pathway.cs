using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Đường dẫn cho kẻ địch di chuyển
/// </summary>
[ExecuteInEditMode]
public class Pathway : MonoBehaviour
{
    void Update()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        if (waypoints.Length > 1)
        {
            int idx;
            for (idx = 1; idx < waypoints.Length; idx++)
            {
                // Vẽ các đường màu xanh ở chế độ edit
                Debug.DrawLine(waypoints[idx - 1].transform.position, waypoints[idx].transform.position, Color.blue);
            }
        }
    }

    /// <summary>
    /// Lấy điểm tham chiếu gần nhất cho vị trí được chỉ định
    /// </summary>
    /// <returns>Điểm đến gần nhất.</returns>
    /// <param name="position">Position.</param>
    public Waypoint GetNearestWaypoint(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Waypoint nearestWaypoint = null;
        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            if (waypoint.GetHashCode() != GetHashCode())
            {
                // Tính toán khoảng cách đến điểm tham chiếu 
                Vector3 vect = position - waypoint.transform.position;
                float distance = vect.magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestWaypoint = waypoint;
                }
            }
        }
        return nearestWaypoint;
    }

    /// <summary>
    /// Nhận điểm tham chiếu tiếp theo đến con đường này 
    /// </summary>
    /// <returns>Điểm tiếp theo.</returns>
    /// <param name="currentWaypoint">Điểm gần đó.</param>
    /// <param name="loop">If set to <c>true</c> loop.</param>
    public Waypoint GetNextWaypoint(Waypoint currentWaypoint, bool loop)
    {
        Waypoint res = null;
        int idx = currentWaypoint.transform.GetSiblingIndex();
        if (idx < (transform.childCount - 1))
        {
            idx += 1;
        }
        else
        {
            idx = 0;
        }
        if (!(loop == false && idx == 0))
        {
            res = transform.GetChild(idx).GetComponent<Waypoint>(); 
        }
        return res;
    }

    public float GetPathDistance(Waypoint fromWaypoint)
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        bool hitted = false;
        float pathDistance = 0f;
        int idx;
        for (idx = 0; idx < waypoints.Length; ++idx)
        {
            if (hitted == true)
            {
                Vector2 distance = waypoints[idx].transform.position - waypoints[idx - 1].transform.position;
                pathDistance += distance.magnitude;
            }
            if (waypoints[idx] == fromWaypoint)
            {
                hitted = true;
            }
        }
        return pathDistance;
    }
}
