using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Quỹ đạo bay của đạn .
/// </summary>
public class BulletArrow : MonoBehaviour, IBullet
{
    // Sát thương gây ra 
    public int damage = 1;
    // Thời gian sống tối da 
    public float lifeTime = 3f;
    // Tốc độ ban đầu 
    public float speed = 3f;
    // Tăng tốc độ theo thời gian 
    public float speedUpOverTime = 0.5f;
    // Khoảng cách gần nhất để trúng mục tiêu 
    public float hitDistance = 0.2f;
    // Độ lêch quỹ đạo (Khoảng cách đến mục tiêu)
    public float ballisticOffset = 0.5f;
    // Không xoay đạn trong quá trình bay 
    public bool freezeRotation = false;

    // Vị trí xuất phát đạn 
    private Vector2 originPoint;
    // Mục tiêu đã nhắm
    private Transform target;
    // Vị trí mục tiêu cuối cùng 
    private Vector2 aimPoint;
    // Vị trí hiện tjai mà không tính lệch quỹ đjao 
    private Vector2 myVirtualPosition;
    // Vị trí trong frame trước 
    private Vector2 myPreviousPosition;
    // Biến đếm cho tính toán gia tốc 
    private float counter;
    // Hình ảnh của viên đạn
    private SpriteRenderer sprite;

    /// <summary>
    /// Cài đặt sát thương cho viên đạn 
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    /// <summary>
    /// Bắn viên đạn vào mục tiêu theo quỹ đạo đã được chọn .
    /// </summary>
    /// <param name="target">Target.</param>
    public void Fire(Transform target)
    {
        sprite = GetComponent<SpriteRenderer>();
        // Vô hiệu hóa sprite trong frame đầu vì chưa biết hướng bay
        sprite.enabled = false;
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        this.target = target;
        aimPoint = target.position;
        // Xóa viên đạn sau thời gian tồn tại 
        Destroy(gameObject, lifeTime);
    }
    void Update ()
    {
        counter += Time.deltaTime;
        // Thêm gia tốc 
        speed += Time.deltaTime * speedUpOverTime;
        if (target != null)
        {
            aimPoint = target.position;
        }
        // Tính khoảng cách từ điểm bắn đến điểm nhắm 
        Vector2 originDistance = aimPoint - originPoint;
        // Tính toán đoạn còn lại 
        Vector2 distanceToAim = aimPoint - (Vector2)myVirtualPosition;
        // Tiến tới mục tiêu
        myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * speed / originDistance.magnitude);
        // Thêm phần bù đạn đạo vào quỹ đạo
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);
        // Xoay viên đạn theo quỹ đạo 
        LookAtDirection2D((Vector2)transform.position - myPreviousPosition);
        myPreviousPosition = transform.position;
        sprite.enabled = true;
        // Đủ gần để tấn công
        if (distanceToAim.magnitude <= hitDistance)
        {
            if (target != null)
            {
                // Mục tiêu có thể nhận sát thương
                DamageTaker damageTaker = target.GetComponent<DamageTaker>();
                if (damageTaker != null)
                {
                    damageTaker.TakeDamage(damage);
                }
            }
            // Hủy viên đạn
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Nhìn vào hướng 2d 
    /// </summary>
    /// <param name="direction">Direction.</param>
    private void LookAtDirection2D(Vector2 direction)
    {
        if (freezeRotation == false)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    /// <summary>
    /// Thêm độ lệch đạn vào quỹ đạo 
    /// </summary>
    /// <returns>The ballistic offset.</returns>
    /// <param name="originDistance">Origin distance.</param>
    /// <param name="distanceToAim">Distance to aim.</param>
    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset > 0f)
        {
            // Tính toán độ lệch
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            // Thêm offset vào quỹ đạo 
            return (Vector2)myVirtualPosition + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            return myVirtualPosition;
        }
    }
}
