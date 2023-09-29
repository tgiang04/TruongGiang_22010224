using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scripts điều khiển cấp độ
/// </summary>
public class LevelManager : MonoBehaviour
{
    // Quản lí giao diện người dùng
    private UiManager uiManager;
    // Số lượng điểm xuất hiện địch trong màn này
    private int spawnNumbers;
    void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        spawnNumbers = FindObjectsOfType<SpawnPoint>().Length;
        if (spawnNumbers <= 0)
        {
            Debug.LogError("Khong co diem xuat hien");
        }
        Debug.Assert(uiManager, "Tham so khoi tao sai");
    }
    void OnEnable()
    {
        EventManager.StartListening("Captured", Captured);
        EventManager.StartListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }
    void OnDisable()
    {
        EventManager.StopListening("Captured", Captured);
        EventManager.StopListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    /// <summary>
    /// Determines if is collision valid for this scene.
    /// </summary>
    /// <returns><c>true</c> if is collision valid the specified myTag otherTag; otherwise, <c>false</c>.</returns>
    /// <param name="myTag">My tag.</param>
    /// <param name="otherTag">Other tag.</param>
    public static bool IsCollisionValid(string myTag, string otherTag)
    {
        bool res = false;
        switch (myTag)
        {
            case "Tower":
            case "Defender":
                switch (otherTag)
                {
                    case "Enemy":
                        res = true;
                        break;
                }
                break;
            case "Enemy":
                switch (otherTag)
                {
                    case "Defender":
                    case "Tower":
                        res = true;
                        break;
                }
                break;
            case "Bullet":
                switch (otherTag)
                {
                    case "Enemy":
                        res = true;
                        break;
                }
                break;
            case "CapturePoint":
                switch (otherTag)
                {
                    case "Enemy":
                        res = true;
                        break;
                }
                break;
            default:
                Debug.Log("Unknown collision tag => " + myTag + " - " + otherTag);
                break;
        }
        return res;
    }

    /// <summary>
    /// Kẻ địch đã đến điểm bắt giữ.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void Captured(GameObject obj, string param)
    {
        // Defeat
        uiManager.GoToDefeatMenu();
    }

    /// <summary>
    /// Tất cả kẻ địch bị tiêu diệt
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void AllEnemiesAreDead(GameObject obj, string param)
    {
        spawnNumbers--;
        if (spawnNumbers <= 0)
        {
            uiManager.GoToVictoryMenu();
        }
    }
}
