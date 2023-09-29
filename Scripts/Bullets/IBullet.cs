using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Giao diện chính cho đạn
/// </summary>
public interface IBullet
{
    void SetDamage(int damage);
    void Fire(Transform target);
}
