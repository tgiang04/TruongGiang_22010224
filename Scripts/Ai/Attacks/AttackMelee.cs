using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Co che tan cong ban vu khi gan
/// </summary>
public class AttackMelee : MonoBehaviour, IAttack
{
    public int damage = 1;
    // Thoi gian cho giua cac lan tan cong
    public float cooldown = 1f;

    // Bo dieu khien Animation
    private Animation anim;
    // Bien dem tinh toan thoi gian cho
    private float cooldownCounter;

    /// <summary>
    /// Khoi tao doi tuong
    /// </summary>
    void Awake()
    {
        anim = GetComponentInParent<Animation>();
        cooldownCounter = cooldown;
    }

    /// <summary>
    /// Cap nhat doi tuong
    /// </summary>
    void Update()
    {
        if (cooldownCounter < cooldown)
        {
            cooldownCounter += Time.deltaTime;
        }
    }

    /// <summary>
    /// Bat dau tan cong neu thoi gian cho da ket thuc
    /// </summary>
    /// <param name="target">Target.</param>
    public void Attack(Transform target)
    {
        if (cooldownCounter >= cooldown)
        {
            cooldownCounter = 0f;
            Smash(target);
        }
    }

    /// <summary>
    /// Thuc hien tan cong
    /// </summary>
    /// <param name="target">Target.</param>
    private void Smash(Transform target)
    {
        if (target != null)
        {
            // Neu muc tieu co the nhan sat thuong
            DamageTaker damageTaker = target.GetComponent<DamageTaker>();
            if (damageTaker != null)
            {
                damageTaker.TakeDamage(damage);
            }
            if (anim != null)
            {
                anim.Play("AttackMelee");
            }
        }
    }
}
