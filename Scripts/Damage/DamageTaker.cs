using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Đối tượng có thể nhận sát thương
/// </summary>
public class DamageTaker : MonoBehaviour
{
    // Số điểm sát thương ban đầu
    public int hitpoints = 1;
    // Số điểm sát thương hiện tại
    public int currentHitpoints;
    // Thời gian hiển thị hiệu ứng va chạm
    public float hitDisplayTime = 0.2f;

    // Hình ảnh của đối tượng này
    private SpriteRenderer sprite;
    // Hiệu ứng hình ảnh khi bị va chạm
    private bool hitCoroutine;
    void Awake()
    {
        currentHitpoints = hitpoints;
        sprite = GetComponentInChildren<SpriteRenderer>();
        Debug.Assert(sprite, "Tham so khoi tao sai");
    }

    /// <summary>
    /// Nhận sát thương
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void TakeDamage(int damage)
    {
        if (currentHitpoints > damage)
        {
            // Mục tiêu vẫn còn ống
            currentHitpoints -= damage;
            // Nếu không có coroutine hiện tại
            if (hitCoroutine == false)
            {
                // Hiển thị hiệu ứng sát thương
                StartCoroutine(DisplayDamage());
            }
        }
        else
        {
            currentHitpoints = 0;
            Die();
        }
    }
    public void Die()
    {
        EventManager.TriggerEvent("UnitDie", gameObject, null);
        Destroy(gameObject);
    }

    /// <summary>
    /// Hiệu ứng hình ảnh khi nhận sát thương
    /// </summary>
    /// <returns>The damage.</returns>
    IEnumerator DisplayDamage()
    {
        hitCoroutine = true;
        Color originColor = sprite.color;
        float counter;
        for (counter = 0f; counter < hitDisplayTime; counter += Time.deltaTime)
        {
            sprite.color = Color.Lerp(originColor, Color.black, Mathf.PingPong(counter, hitDisplayTime));
            yield return new WaitForEndOfFrame();
        }
        sprite.color = originColor;
        hitCoroutine = false;
    }
}
