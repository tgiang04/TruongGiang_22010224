using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Thực hiện trạng thái chờ 
/// </summary>
public class AiStateIdle : MonoBehaviour, IAiState
{
    // Chuyển trạng thái nếu có tấn công
    public string agressiveAiState;
    public string passiveAiState;
    private AiBehavior aiBehavior;
    void Awake ()
    {
        aiBehavior = GetComponent<AiBehavior> ();
        Debug.Assert (aiBehavior, "Tham so khoi tao sai ");
    }
    public void OnStateEnter (string previousState, string newState)
    {

    }
    public void OnStateExit (string previousState, string newState)
    {

    }
    public void TriggerEnter(Collider2D my, Collider2D other)
    {

    }
    public void TriggerStay(Collider2D my, Collider2D other)
    {
        aiBehavior.ChangeState(agressiveAiState);
    }

    public void TriggerExit(Collider2D my, Collider2D other)
    {

    }
}
