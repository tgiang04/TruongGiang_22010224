using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cây xây dựng
/// </summary>
public class BuildingTree : MonoBehaviour
{
    /// <summary>
    /// Tháp mở trong cây xây dựng 
    /// </summary>
    [HideInInspector]
    public Tower myTower;
    void Start()
    {
        Debug.Assert(myTower, "Tham so khoi tao sai");
    }
    public void Build(GameObject prefab)
    {
        myTower.BuildTower(prefab);
    }
}
