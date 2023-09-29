using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBold : MonoBehaviour, IBullet
{
    // Sát thương
    public int damage = 1;
    // Thời gian tồn tại 
    public float lifeTime = 3f;
    // Tốc độ ban đầu
    public float speed = 3f;
    // Tăng tốc độ không đổi 
    public float speedUpOverTime = 0.5f;
    // Nếu mục tiêu nằm trong phạm vi thì tấn công 
    public float hitDistance = 0.2f;
    // Độ lệch đường đi theo offset (Tính bằng khoảng cách đến mục tiêu)
    public float ballisticOffset = 0.1f;
    // Đạn sẽ đi qua mục tiêu (Theo tỉ lệ so với khoảng cách gốc đến mục tiêu)
    public float penetrationRatio = 0.3f;

    // Từ vị trí này đạn đã được bắn
    private Vector2 originPoint;
    // Mục tiêu đã nhắm
    private Transform target;
    // Vị trí cuối của mục tiêu 
    private Vector2 aimPoint;
    // Vị trí hiện tại mà không tính độ lệch đường đi 
    private Vector2 myVirtualPosition;
    // Vị trí trong frame trước 
    private Vector2 myPreviousPosition;
    // Bộ đếm cho việc tính toán gia tốc 
    private float counter;
    // Hình ảnh của viên đạn này 
    private SpriteRenderer sprite;

    /// <summary>
    /// Đặt số lượng sat thương cho viên đạn này 
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    /// <summary>
    /// Bắn viên đạn vào mục tiêu cụ thể 
    /// </summary>
    /// <param name="target">Target.</param>
    public void Fire(Transform target)
    {
        sprite = GetComponent<SpriteRenderer>();
        //  Tắt hình ảnh vào frame đầu tiên vì chúng ta chưa biết hướng bay
        sprite.enabled = false;
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        this.target = target;
        aimPoint = GetPenetrationPoint(target.position);
        // Hủy viên đạn sau thời gian tồn tại 
        Destroy(gameObject, lifeTime);
    }
    void Update()
    {
        counter += Time.deltaTime;
        // Thêm gia tốc
        speed += Time.deltaTime * speedUpOverTime;
        if (target != null)
        {
            aimPoint = GetPenetrationPoint(target.position);
        }
        // Tính khoảng cách từ vị trí bắn đến mục tiêu
        Vector2 originDistance = aimPoint - originPoint;
        // Tính khoảng cách còn lại 
        Vector2 distanceToAim = aimPoint - (Vector2)myVirtualPosition;
        // Di chuyển đến mục tiêu
        myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * speed / originDistance.magnitude);
        // Thêm độ lệch đường đi
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);
        // Xoay viên đạn theo hướng đường đi
        LookAtDirection2D((Vector2)transform.position - myPreviousPosition);
        myPreviousPosition = transform.position;
        sprite.enabled = true;
        // Đủ gần để trúng
        if (distanceToAim.magnitude <= hitDistance)
        {
            // Hủy viên đạn
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Xoay đối tượng theo hướng 2d
    /// </summary>
    /// <param name="direction">Direction.</param>
    private void LookAtDirection2D(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// Tính toán vị trí nhắm với độ lệch đường đi
    /// </summary>
    /// <returns>Điểm nhắm có độ lệch.</returns>
    /// <param name="targetPosition">Target position.</param>
    private Vector2 GetPenetrationPoint(Vector2 targetPosition)
    {
        Vector2 penetrationVector = targetPosition - originPoint;
        return originPoint + penetrationVector * (1f + penetrationRatio);
    }

    /// <summary>
    /// Thêm độ lệch đường đi
    /// </summary>
    /// <returns>Độ lệch đường đi</returns>
    /// <param name="originDistance">Origin distance.</param>
    /// <param name="distanceToAim">Distance to aim.</param>
    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset > 0f)
        {
            // Tính độ lệch sin
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            // AThêm độ lệch vào đường đi
            return (Vector2)myVirtualPosition + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            return myVirtualPosition;
        }
    }

    /// <summary>
    /// Gây sát thương cho mục tiêu khi va chạm
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu có xảy ra va chạm
        if (LevelManager.IsCollisionValid(gameObject.tag, other.gameObject.tag) == true)
        {
            // Nếu mục tiêu nhận sát thương
            DamageTaker damageTaker = other.GetComponent<DamageTaker> ();
            if (damageTaker != null)
            {
                damageTaker.TakeDamage(damage);
            }
        }
    }
}
