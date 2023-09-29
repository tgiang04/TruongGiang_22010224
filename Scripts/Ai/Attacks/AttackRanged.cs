using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tan cong bang vu khi tam xa
/// </summary>
public class AttackRanged : MonoBehaviour, IAttack
{
    public int damage = 1;
    public float cooldown = 1f;
    // Prefab cua mui ten, dan(Doi tuong duoc ban ra)
    public GameObject arrowPrefab;
    // Vi tri tu do mui ten se duoc ban di
    public Transform firePoint;

    // Bo dieu khien Animation cho doi tuong
    private Animation anim;
    private float cooldownCounter;

    /// <summary>
    /// Khoi tao doi tuong trong scene
    /// </summary>
    void Awake()
    {
        anim = GetComponentInParent<Animation>();
        cooldownCounter = cooldown;
        // Kiem tra xem arrowPrefab va firePoint co duoc dat chua
        Debug.Assert(arrowPrefab && firePoint, "Tham so khoi tao sai");
    }

    /// <summary>
    /// Update duoc goi trong moi frame.
    /// </summary>
    void Update()
    {
        if (cooldownCounter < cooldown)
        {
            cooldownCounter += Time.deltaTime;
        }
    }

    /// <summary>
    /// Tan cong muc tieu chi dinh
    /// </summary>
    /// <param name="target">Target.</param>
    public void Attack(Transform target)
    {
        //Kiem tra cooldownCounter da het cooldown chua
        if (cooldownCounter >= cooldown)
        {
            cooldownCounter = 0f;//Dat lai de bat dau dem lai cooldown
            Fire(target);//Thuc hien tan cong
        }
    }

    /// <summary>
    /// Thuc hien ban ra dan, mui ten
    /// </summary>
    /// <param name="target">Target.</param>
    private void Fire(Transform target)
    {
        if (target != null)
        {
            // Tao ra dan, mui ten tu prefab tai vi tri va huong cua firePoint
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            IBullet bullet = arrow.GetComponent<IBullet>();
            bullet.SetDamage(damage);//Khoi tao sat thuong
            bullet.Fire(target);//Ban dan, mui ten theo huong target
            if (anim != null)
            {
                anim.Play("AttackRanged");//Chay anim
            }
        }
    }
}
