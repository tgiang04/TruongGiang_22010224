using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Di chuyển theo đường dẫn
/// </summary>
public class AiStatePatrol : MonoBehaviour, IAiState
{
    // Đường dẫn đã chỉ định 
    public Pathway path;
    public bool loop = false;
    public string agressiveAiState;
    public string passiveAiState;
    private Animation anim;
    private AiBehavior aiBehavior;
    NavAgent navAgent;
    private Waypoint destination;
    void Awake ()
    {
        aiBehavior = GetComponent<AiBehavior>();
        navAgent = GetComponent<NavAgent>();
        anim = GetComponent<Animation>();
        Debug.Assert (aiBehavior && navAgent, "Tham so khoi tao khong dung ");
    }
    public void OnStateEnter (string previousState, string newState)
    {
        if (path == null)
        {
            // Tìm đường dẫn 
            path = FindObjectOfType<Pathway>();
            Debug.Assert (path, "Have no path");
        }
        if (destination == null)
        {
            // Lấy điểm đến từ đường dẫn
            destination = path.GetNearestWaypoint (transform.position);
        }
        // Đặt đích đến cho điều hướng 
        navAgent.destination = destination.transform.position;
        if (anim != null)
        {
            navAgent.move = true;
            anim.Play("Move");
        }
    }
    public void OnStateExit (string previousState, string newState)
    {
        if (anim != null)
        {
            navAgent.move = false;
            anim.Stop();
        }
    }
    void FixedUpdate ()
    {
        if (destination != null)
        {
            // Đến đích 
            if ((Vector2)destination.transform.position == (Vector2)transform.position)
            {
                // Lấy điểm tiếp theo từ đường dẫn 
                destination = path.GetNextWaypoint (destination, loop);
                if (destination != null)
                {
                    // Đăt điểm đích 
                    navAgent.destination = destination.transform.position;
                }
            }
        }
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
    //Tính toán khoảng cách còn lại trên đường dẫn 
    public float GetRemainingPath()
    {
        Vector2 distance = destination.transform.position - transform.position;
        return (distance.magnitude + path.GetPathDistance(destination));
    }
}
