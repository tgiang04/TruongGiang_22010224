using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Xu li va cham
public class AiColliderTrigger : MonoBehaviour
{
    // Phat hien va cham
    public List<string> tags = new List<string>();
    private Collider2D col;
    // Thanh phan AI behavior nam trong doi tuong cha
    private AiBehavior aiBehavior;
    void Awake()
    {
        col = GetComponent<Collider2D>();
        aiBehavior = GetComponentInParent<AiBehavior>();
        Debug.Assert(col && aiBehavior, "Tham so khoi tao sai");
    }

    /// <summary>
    /// Xac dinh xem doi tuong có cho phep trong va cham khong
    /// </summary>
    /// <returns><c>true</c> Neu duoc cho phep tag cu the trong va cham; nguoc lai, <c>false</c>.</returns>
    /// <param name="tag">Tag.</param>
    private bool IsTagAllowed(string tag)
    {
        bool res = false;
        if (tags.Count > 0)
        {
            foreach (string str in tags)
            {
                if (str == tag)
                {
                    res = true;
                    break;
                }
            }
        }
        else
        {
            res = true;
        }
        return res;
    }

    /// <summary>
    /// Goi khi va cham bat dau
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            // Thong bao cho AI behavior
            aiBehavior.TriggerEnter2D(col, other);
        }
    }

    /// <summary>
    /// Goi khi tiep tuc va cham
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            aiBehavior.TriggerStay2D(col, other);
        }
    }

    /// <summary>
    /// Goi khi ket thuc va cham.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            aiBehavior.TriggerExit2D(col, other);
        }
    }
}
