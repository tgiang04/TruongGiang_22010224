using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Khu vực có hiệu lực sát thương khi tiêu diệt.
/// </summary>
public class AOE : MonoBehaviour
{
    // Bán kính của khu vực 
    public float radius = 0.3f;
    // Lượng sát thương 
    public int damage = 3;
    // Prefab nổ 
    public GameObject explosion;
    // Thời gian hiển thị hiệu ứng nổ 
    public float explosionDuration = 1f;

    // Đóng cảnh, không tạo đối tượng mới khi bị phá hủy 
    private bool isQuitting;

    /// <summary>
    /// Gọi khi OnEnable kích hoạt 
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("SceneQuit", SceneQuit);
    }

    /// <summary>
    /// Gọi khi OnDisable kích hoạt 
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("SceneQuit", SceneQuit);
    }

    /// <summary>
    /// Gọi khi thoát ứng dụng 
    /// </summary>
    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    /// <summary>
    /// Gọi khi bị hủy 
    /// </summary>
    void OnDestroy()
    {
        // Nếu đang trong một tiến trình 
        if (isQuitting == false)
        {
            // Tìm các collider trong bán kính cụ thể 
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D col in cols)
            {
                // Nếu va chạm được cho phép 
                if (LevelManager.IsCollisionValid(gameObject.tag, col.gameObject.tag) == true)
                {
                    // Nếu mục tiêu có thể nhận sát thương 
                    DamageTaker damageTaker = col.gameObject.GetComponent<DamageTaker>();
                    if (damageTaker != null)
                    {
                        damageTaker.TakeDamage(damage);
                    }
                }
            }
            if (explosion != null)
            {
                // Tạo hiệu ứng hình ảnh nổ
                Destroy(Instantiate<GameObject>(explosion, transform.position, transform.rotation), explosionDuration);
            }
        }
    }

    /// <summary>
    /// Gọi khi thoát cảnh 
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void SceneQuit(GameObject obj, string param)
    {
        isQuitting = true;
    }
}
