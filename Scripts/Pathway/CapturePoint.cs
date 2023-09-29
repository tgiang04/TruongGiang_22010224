using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nếu kẻ địch đến điểm này, trò chơi kết thúc
/// </summary>
public class CapturePoint : MonoBehaviour
{
    // Kẻ địch đến điểm bắt giữ
    private bool alreadyCaptured;
    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu cho phép va chạm 
        if (LevelManager.IsCollisionValid(gameObject.tag, other.gameObject.tag) == true)
        {
            if (alreadyCaptured == false)
            {
                alreadyCaptured = true;
                EventManager.TriggerEvent("Captured", other.gameObject, null);
            }
        }
    }
}
