using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cho phep thuc hien di chuyen den diem dich
/// </summary>
public class AiStateMove : MonoBehaviour, IAiState
{
    // Khởi tạo điểm kết thúc 
    public Transform destination;
    public string agressiveAiState;
    public string passiveAiState;

    // Bộ điều khiển animation 
    private Animation anim;
    private AiBehavior aiBehavior;
    // Điều hướng của đối tượng 
    NavAgent navAgent;
    void Awake ()
    {
        aiBehavior = GetComponent<AiBehavior>();
        navAgent = GetComponent<NavAgent>();
        anim = GetComponentInParent<Animation>();
        Debug.Assert (aiBehavior && navAgent, "Tham so khoi tạo sai ");
    }
    public void OnStateEnter (string previousState, string newState)
    {
        // Đặt điểm đích 
        navAgent.destination = destination.position;
        if (anim != null)
        {
            // Di chuyển
            navAgent.move = true;
            // Chạy animation 
            anim.Play("Move");
        }
    }

    public void OnStateExit (string previousState, string newState)
    {
        if (anim != null)
        {
            // Dừng di chuyển
            navAgent.move = false;
            // Dừng animation
            anim.Stop();
        }
    }
    void FixedUpdate ()
    {
        // Đến điểm đích 
        if ((Vector2)transform.position == (Vector2)destination.position)
        {
            // Xác định điểm quan trọng 
            navAgent.LookAt(destination.right);
            // Chuyển trạng thái 
            aiBehavior.ChangeState(passiveAiState);
        }
    }
    public void TriggerEnter(Collider2D my, Collider2D other)
    {

    }
    public void TriggerStay(Collider2D my, Collider2D other)
    {

    }
    public void TriggerExit(Collider2D my, Collider2D other)
    {

    }
}
