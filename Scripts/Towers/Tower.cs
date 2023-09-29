using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Xây dựng và vận hành tháp.
/// </summary>
public class Tower : MonoBehaviour
{
    // Prefab cho cây xây dựng
    public GameObject buildingTreePrefab;
    // Phạm vi tấn công của tháp
    public GameObject range;

    // Quản lý giao diện người dùng
    private UiManager uiManager;
    // Canvas UI cấp độ để hiển thị cây xây dựng
    private Canvas canvas;
    // Collider của tháp
    private Collider2D bodyCollider;
    // Cây xây dựng đang hiển thị
    private BuildingTree activeBuildingTree;
    void OnEnable()
    {
        EventManager.StartListening("GamePaused", GamePaused);
        EventManager.StartListening("UserClick", UserClick);
    }
    void OnDisable()
    {
        EventManager.StopListening("GamePaused", GamePaused);
        EventManager.StopListening("UserClick", UserClick);
    }
    void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canv in canvases)
        {
            if (canv.CompareTag("LevelUI"))
            {
                canvas = canv;
                break;
            }
        }
        bodyCollider = GetComponent<Collider2D>();
        Debug.Assert(uiManager && canvas && bodyCollider, "Tham so khoi tao sai");
    }
    private void OpenBuildingTree()
    {
        if (buildingTreePrefab != null)
        {
            // Tạo cây xây dhuwnjg
            activeBuildingTree = Instantiate<GameObject>(buildingTreePrefab, canvas.transform).GetComponent<BuildingTree>();
            // Đặt qua tháp
            activeBuildingTree.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            activeBuildingTree.myTower = this;
            // Tắt raycast của tháp
            bodyCollider.enabled = false;
        }
    }

    /// <summary>
    /// Đóng cây xây dựng
    /// </summary>
    private void CloseBuildingTree()
    {
        if (activeBuildingTree != null)
        {
            Destroy(activeBuildingTree.gameObject);
            // Bật raycast của tháp
            bodyCollider.enabled = true;
        }
    }

    /// <summary>
    /// Xây dựng tháp.
    /// </summary>
    /// <param name="towerPrefab">Tower prefab.</param>
    public void BuildTower(GameObject towerPrefab)
    {
        // Đóng cây xây dựng đang hoạt động
        CloseBuildingTree();
        Price price = towerPrefab.GetComponent<Price>();
        // Đủ tiền
        if (uiManager.SpendGold(price.price) == true)
        {
            // Tạo tháp mới và đặt nó ở cùng vị trí
            GameObject newTower = Instantiate<GameObject>(towerPrefab, transform.parent);
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
            // Hủy tháp cũ
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Tắt raycast của tháp và đóng cây xây dựng khi trò chơi tạm dừng.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void GamePaused(GameObject obj, string param)
    {
        if (param == bool.TrueString) // Paused
        {
            CloseBuildingTree();
            bodyCollider.enabled = false;
        }
        else // Tiếp tục chơi
        {
            bodyCollider.enabled = true;
        }
    }
    private void UserClick(GameObject obj, string param)
    {
        if (obj == gameObject) 
        {
            // Hiển thị phạm vi tấn công
            ShowRange(true);
            if (activeBuildingTree == null)
            {
                // Mở cây xây dựng nếu nó chưa được mở
                OpenBuildingTree();
            }
        }
        else 
        {
            // Ẩn phạm vi tấn công
            ShowRange(false);
            // Đóng cây xây dựng đang hoạt động
            CloseBuildingTree();
        }
    }
    private void ShowRange(bool condition)
    {
        if (range != null)
        {
            range.SetActive(condition);
        }
    }
}
