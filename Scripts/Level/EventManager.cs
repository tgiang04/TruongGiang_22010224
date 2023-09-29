using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/// <summary>
//Loại sự kiện
/// </summary>
[System.Serializable]
public class MyEvent : UnityEvent<GameObject, string>
{

}

/// <summary>
/// Hệ thống tin nhắn
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                if (!eventManager)
                {
                    Debug.LogError("Can co mot scprits EventManager hoat dong ten mot GameObject.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    // Danh sách sự kiện
    private Dictionary<string, MyEvent> eventDictionary;
    // Quản lí sự kiện
    private static EventManager eventManager;

    /// <summary>
    /// Khởi tạo đối tượng
    /// </summary>
    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, MyEvent>();
        }
    }

    /// <summary>
    /// Bắt đầu hướng vào sự kiện cụ thể
    /// </summary>
    /// <param name="eventName">Event name.</param>
    /// <param name="listener">Listener.</param>
    public static void StartListening(string eventName, UnityAction<GameObject, string> listener)
    {
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new MyEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// Dừng lắng nghe sự kiện.
    /// </summary>
    /// <param name="eventName">Event name.</param>
    /// <param name="listener">Listener.</param>
    public static void StopListening(string eventName, UnityAction<GameObject, string> listener)
    {
        if (eventManager == null) return;
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    /// <summary>
    /// Kích hoạt sự kiện.
    /// </summary>
    /// <param name="eventName">Event name.</param>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    public static void TriggerEvent(string eventName,GameObject obj, string param)
    {
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(obj, param);
        }
    }
}
