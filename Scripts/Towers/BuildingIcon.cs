using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Biểu tượng UI trong cây xây dựng tháp.
/// </summary>
public class BuildingIcon : MonoBehaviour
{
    // Prefab tháp cho biểu tượng này
    public GameObject towerPrefab;

    // Trường text cho giá tháp
    private Text price;
    // Cây xây dựng cha
    private BuildingTree myTree;

    /// <summary>
    /// Khởi chạy sự kiện khi kích hoạt.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("UserClick", UserClick);
    }

    /// <summary>
    /// Kết thúc sự kiện khi vô hiệu hóa.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("UserClick", UserClick);
    }
    void Awake()
    {
        // Lấy cây xây dựng từ đối tượng cha
        myTree = transform.GetComponentInParent<BuildingTree>();
        price = GetComponentInChildren<Text>();
        Debug.Assert(price && myTree, "Tham số đầu vào sai");
        if (towerPrefab == null)
        {
            // Nếu biểu tượng này không có prefab tháp - ẩn biểu tượng
            gameObject.SetActive(false);
        }
        else
        {
            // Hiển thị giá tháp
            price.text = towerPrefab.GetComponent<Price>().price.ToString();
        }
    }
    private void UserClick(GameObject obj, string param)
    {
        // Nếu nhấp chuột vào biểu tượng này
        if (obj == gameObject)
        {
            // Xây dựng tháp 
            myTree.Build(towerPrefab);
        }
    }
}
