using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Điều khiển camera
/// </summary>
public class CameraControl : MonoBehaviour
{
    // Chiều rộng khung hình ban đầu
    private int defaultWidth;
    // Chiều rộng lần cuối cùng đã cập nhật
    private int lastUpdatedWidth;
    // Kích thước camera ban đầu
    private float defaultCameraSize;
    void Start()
    {
        defaultWidth = lastUpdatedWidth = Camera.main.pixelWidth;
        defaultCameraSize = Camera.main.orthographicSize;
        ChangeCameraSize();
    }
    void Update()
    {
        if (lastUpdatedWidth != Camera.main.pixelWidth)
        {
            lastUpdatedWidth = Camera.main.pixelWidth;
            ChangeCameraSize();
        }
    }

    /// <summary>
    /// Thay đổi kích thước camera để duy trì chiều rộng hiển thị không đổi.
    /// </summary>
    private void ChangeCameraSize()
    {
        Camera.main.orthographicSize = defaultCameraSize * ((float)defaultWidth / (float)lastUpdatedWidth);
    }
}
