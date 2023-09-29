using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Quản lí giao diện người dùng và sự kiện 
/// </summary>
public class UiManager : MonoBehaviour
{
    // Màn tiếp theo
    public string nextLevel;
    public GameObject pauseMenu;
    public GameObject defeatMenu;
    public GameObject victoryMenu;
    // Giao diện cấp độ
    public GameObject levelUI;
    public Text goldAmount;
    private bool paused;
    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
    }
    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
    }
    void Awake()
    {
        Debug.Assert(pauseMenu && defeatMenu && victoryMenu && levelUI && goldAmount, "Wrong initial parameters");
    }
    void Start()
    {
        GoToLevel();
    }
    void Update()
    {
        if (paused == false)
        {
            // Tạm dừng khi bấm nút 
            if (Input.GetButtonDown("Cancel") == true)
            {
                PauseGame(true);
                GoToPauseMenu();
            }
            // Người chơi bấm chuột 
            if (Input.GetMouseButtonDown(0) == true)
            {
                // Kiểm tra xem con trỏ có nằm trên các thành phần giao diện người dùng hay không
                GameObject hittedObj = null;
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);
                foreach (RaycastResult res in results)
                {
                    if (res.gameObject.CompareTag("ActionIcon"))
                    {
                        hittedObj = res.gameObject;
                        break;
                    }
                }
                if (results.Count <= 0) // Không có thành phần giao diện người dùng nào nằm trong con trỏ

                {
                    // Kiểm tra xem con trỏ có nằm trên vật thể va chạm hay không
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
                    foreach (RaycastHit2D hit in hits)
                    {
                        // Nếu là va chạm với tháp
                        if (hit.collider.gameObject.CompareTag("Tower"))
                        {
                            hittedObj = hit.collider.gameObject;
                            break;
                        }
                    }
                }
                // Gửi thông báo với dữ liệu nhấp chuột của người dùng
                EventManager.TriggerEvent("UserClick", hittedObj, null);
            }
        }
    }

    /// <summary>
    /// Dừng màn trước đó và chạy màn mới
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    private void LoadScene(string sceneName)
    {
        EventManager.TriggerEvent("SceneQuit", null, null);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void NewGame()
    {
        GoToLevel();
        PauseGame(false);
    }
    public void ResumeGame()
    {
        GoToLevel();
        PauseGame(false);
    }

    public void GoToMainMenu()
    {
        LoadScene("MainMenu");
    }
    private void CloseAllUI()
    {
        pauseMenu.SetActive(false);
        defeatMenu.SetActive(false);
        victoryMenu.SetActive(false);
        levelUI.SetActive(false);
    }
    private void PauseGame(bool pause)
    {
        paused = pause;
        // Stop the time on pause
        Time.timeScale = pause ? 0f : 1f;
        EventManager.TriggerEvent("GamePaused", null, pause.ToString());
    }
    private void GoToPauseMenu()
    {
        PauseGame(true);
        CloseAllUI();
        pauseMenu.SetActive(true);
    }

    private void GoToLevel()
    {
        CloseAllUI();
        levelUI.SetActive(true);
        PauseGame(false);
    }
    public void GoToDefeatMenu()
    {
        PauseGame(true);
        CloseAllUI();
        defeatMenu.SetActive(true);
    }
    public void GoToVictoryMenu()
    {
        PauseGame(true);
        CloseAllUI();
        victoryMenu.SetActive(true);
    }
    public void GoToNextLevel()
    {
        LoadScene(nextLevel);
    }

    public void RestartLevel()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        LoadScene(activeScene);
    }

    private int GetGold()
    {
        int gold;
        int.TryParse(goldAmount.text, out gold);
        return gold;
    }
    private void SetGold(int gold)
    {
        goldAmount.text = gold.ToString();
    }
    private void AddGold(int gold)
    {
        SetGold(GetGold() + gold);
    }
    public bool SpendGold(int cost)
    {
        bool res = false;
        int currentGold = GetGold();
        if (currentGold >= cost)
        {
            SetGold(currentGold - cost);
            res = true;
        }
        return res;
    }

    private void UnitDie(GameObject obj, string param)
    {
        if (obj.CompareTag("Enemy"))
        {
            Price price = obj.GetComponent<Price>();
            if (price != null)
            {
                AddGold(price.price);
            }
        }
    }
}
