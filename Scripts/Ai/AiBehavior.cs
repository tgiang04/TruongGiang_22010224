using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// scrpits chinh dieu khien toan bo trang thai AI
/// </summary>
public class AiBehavior : MonoBehaviour
{
    // Trang thai duoc kich hoat khi bat dau
    public string defaultState;

    // Danh sach chua tat ca trang thai cho AI
    private List<IAiState> aiStates = new List<IAiState>();
    private IAiState previousState;
    private IAiState currentState;
    void Start()
    {
        // Lay tat ca trang thai AI cua doi tuong 
        IAiState[] states = GetComponents<IAiState>();
        if (states.Length > 0) 
        {
            foreach (IAiState state in states)
            {
                aiStates.Add(state);
            }
            if (defaultState != null)
            {
                // Dat trang thai truoc do va trang thai hien tai la mac dinh
                previousState = currentState = GetComponent(defaultState) as IAiState;
                if (currentState != null)
                {
                    // Chuyen doi sang trang thai moi
                    ChangeState(currentState.GetType().ToString());
                }
                else
                {
                    Debug.LogError("Trang thai Ai ban dau sai " + defaultState);
                }
            }
            else
            {
                Debug.LogError("AI Khong co trang thai mac dinh");
            }
        } 
        else 
        {
            Debug.LogError ("Khong tim thay trang thai cua Ai");
        }
    }

    /// <summary>
    /// Ai dat ve trang thai mac dinh
    /// </summary>
    public void GoToDefaultState()
    {
        previousState = currentState;
        currentState = GetComponent(defaultState) as IAiState;
        NotifyOnStateExit();
        DisableAllStates();
        EnableNewState();
        NotifyOnStateEnter();
    }

    /// <summary>
    /// Chuyen doi trang thai Ai
    /// </summary>
    /// <param name="state">State.</param>
    public void ChangeState(string state)
    {
        if (state != "")
        {
            // Tim trang thai trong danh sach
            foreach (IAiState aiState in aiStates)
            {
                if (state == aiState.GetType().ToString())
                {
                    previousState = currentState;
                    currentState = aiState;
                    NotifyOnStateExit();
                    DisableAllStates();
                    EnableNewState();
                    NotifyOnStateEnter();
                    return;
                }
            }
            Debug.Log("Khong tim thay trang thai " + state);
            GoToDefaultState();
            Debug.Log("Tro ve trang thai mac dinh " + aiStates[0].GetType().ToString());
        }
    }

    /// <summary>
    /// Tat trang thai Ai
    /// </summary>
    private void DisableAllStates()
    {
        foreach (IAiState aiState in aiStates) 
        {
            MonoBehaviour state = GetComponent(aiState.GetType().ToString()) as MonoBehaviour;
            state.enabled = false;
        }
    }

    /// <summary>
    /// Bat trang thai Ai hien tai
    /// </summary>
    private void EnableNewState()
    {
        MonoBehaviour state = GetComponent(currentState.GetType().ToString()) as MonoBehaviour;
        state.enabled = true;
    }

    /// <summary>
    /// Gui thong bao den trang thai truoc do.
    /// </summary>
    private void NotifyOnStateExit()
    {
        string prev = previousState.GetType().ToString();
        string active = currentState.GetType().ToString();
        IAiState state = GetComponent(prev) as IAiState;
        state.OnStateExit(prev, active);
    }

    /// <summary>
    /// Gui thong bao den trang thai moi.
    /// </summary>
    private void NotifyOnStateEnter()
    {
        string prev = previousState.GetType().ToString();
        string active = currentState.GetType().ToString();
        IAiState state = GetComponent(active) as IAiState;
        state.OnStateEnter(prev, active);
    }

    /// <summary>
    /// Xu ly TriggerEnter2D.
    /// </summary>
    /// <param name="my">Collider cua doi tuong hien tai</param>
    /// <param name="other">Coliider cua doi tuong khac</param>
    public void TriggerEnter2D(Collider2D my, Collider2D other)
    {
        if (LevelManager.IsCollisionValid(gameObject.tag, other.gameObject.tag) == true)
        {
            currentState.TriggerEnter(my, other);
        }
    }

    /// <summary>
    /// Xu ly TriggerStay2D
    /// </summary>
    /// <param name="my">Collider cua doi tuong hien tai</param>
    /// <param name="other">Collider cua doi tuong khac</param>
    public void TriggerStay2D(Collider2D my, Collider2D other)
    {
        if (LevelManager.IsCollisionValid(gameObject.tag, other.gameObject.tag) == true)
        {
            currentState.TriggerStay(my, other);
        }
    }

    /// <summary>
    /// Xu ly TriggerExit2D
    /// </summary>
    public void TriggerExit2D(Collider2D my, Collider2D other)
    {
        if (LevelManager.IsCollisionValid(gameObject.tag, other.gameObject.tag) == true)
        {
            currentState.TriggerExit(my, other);
        }
    }
}
