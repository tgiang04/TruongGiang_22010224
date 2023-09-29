using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vị trí cho người bảo vệ
/// </summary>
public class DefendPoint : MonoBehaviour
{
    /// <summary>
    /// Lấy danh sách điểm phòng thủ.
    /// </summary>
    /// <returns>Các điểm bảo vệ.</returns>
    public List<Transform> GetDefendPoints()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
        return children;
    }
}
