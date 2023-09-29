using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Điều khiển thứ tự hiển thị của sprite dựa trên vị trí y
/// </summary>
public class SpriteSorting : MonoBehaviour
{
    // Cài đặt này là tĩnh sẽ không thay đổi thứ tự trong quá trình cập nhật, chỉ thay đổi khi bắt đầu
    public bool isStatic;
    // Hệ số tăng độ chính xác
    public float rangeFactor = 100f;

    // Danh sách sprite cho đối tượng
    private Dictionary<SpriteRenderer, int> sprites = new Dictionary<SpriteRenderer, int>();
    void Awake()
    {
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprites.Add(sprite, sprite.sortingOrder);
        }
    }
    void Start()
    {
        UpdateSortingOrder();
    }
    void Update()
    {
        if (isStatic == false)
        {
            UpdateSortingOrder();
        }
    }

    /// <summary>
    /// Cập nhật thứ tự hiển thị của sprites
    /// </summary>
    private void UpdateSortingOrder()
    {
        foreach (KeyValuePair<SpriteRenderer, int> sprite in sprites)
        {
            sprite.Key.sortingOrder = sprite.Value - (int)(transform.position.y * rangeFactor);
        }
    }
}
