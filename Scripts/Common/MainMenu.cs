using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Menu chính
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Tải level
    /// </summary>
    public void NewGame()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    /// <summary>
    /// Đóng ứng dụng
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
