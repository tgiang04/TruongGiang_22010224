using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Quan ly di chuyen cua doi tuong
public class NavAgent : MonoBehaviour
{
    public float speed = 1f;
    public bool move = true;
    public bool turn = true;
    // Vi tri dich
    [HideInInspector]
    public Vector2 destination;
    [HideInInspector]
    public Vector2 velocity;

    // Vi tri frame cuoi 
    private Vector2 prevPosition;

    void OnEnable()
    {
        prevPosition = transform.position;
    }

    void Update()
    {
        if (move == true)
        {
            // Di chuyen ve dich
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        Vector2 velocity = (Vector2)transform.position - prevPosition;
        velocity /= Time.deltaTime;
        if (turn == true)
        {
            SetSpriteDirection(destination - (Vector2)transform.position);
        }
        prevPosition = transform.position;
    }

    /// <summary>
    /// Đặt hướng sprite trên trục x.
    /// </summary>
    /// <param name="direction">Direction.</param>
    private void SetSpriteDirection(Vector2 direction)
    {
        if (direction.x > 0f && transform.localScale.x < 0f)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0f && transform.localScale.x > 0f) 
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
    public void LookAt(Vector2 direction)
    {
        SetSpriteDirection(direction);
    }
    public void LookAt(Transform target)
    {
        SetSpriteDirection(target.position - transform.position);
    }
}
