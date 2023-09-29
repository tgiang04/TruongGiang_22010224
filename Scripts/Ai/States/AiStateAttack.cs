using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cho phep AI tan cong muc tieu
/// </summary>
public class AiStateAttack : MonoBehaviour, IAiState
{
    // Tan cong muc tieu gan nhat
    public bool useTargetPriority = false;
    // Chuyen sang trang thai nay neu bi tan cong
    public string agressiveAiState;
    // Chuyen sang trang thai neu bi dong xay ra
    public string passiveAiState;

    private AiBehavior aiBehavior;
    // Muc tieu tan cong
    private GameObject target;
    // Danh sach muc tieu tim thay trong moi frame
    private List<GameObject> targetsList = new List<GameObject>();
    // Loai vu khi tan gan
    private IAttack meleeAttack;
    // Loai vu khi tam xa 
    private IAttack rangedAttack;
    // Loai tan cong cuoi cung da thuc hien
    private IAttack myLastAttack;
    // Đối tượng điều hướng 
    NavAgent nav;
    // Cho phép chờ đối tượng mới trong một frame trước khi kết thúc
    private bool targetless;

    void Awake ()
    {
        aiBehavior = GetComponent<AiBehavior> ();
        meleeAttack = GetComponentInChildren<AttackMelee>() as IAttack;
        rangedAttack = GetComponentInChildren<AttackRanged>() as IAttack;
        nav = GetComponent<NavAgent>();
        // Kiểm tra tham số đầu vào
        Debug.Assert ((aiBehavior != null) && ((meleeAttack != null) || (rangedAttack != null)), "Tham so khoi tao sai");
    }

    /// <summary>
    /// Gọi OnStateEnter khi vào trạng thái này
    /// </summary>
    /// <param name="previousState">Trạng thai trước đó.</param>
    /// <param name="newState">Trạng thái mới.</param>
    public void OnStateEnter (string previousState, string newState)
    {

    }

    //Thoát trạng thái
    public void OnStateExit (string previousState, string newState)
    {
        LoseTarget();
    }

    /// <summary>
    /// FixedUpdate cho đối tượng
    /// </summary>
    void FixedUpdate ()
    {
        // Tìm mục tiêu nếu không có mục tiêu có sẵn 
        if ((target == null) && (targetsList.Count > 0))
        {
            target = GetTopmostTarget();
            if ((target != null) && (nav != null))
            {
                // Xác định mục tiêu 
                nav.LookAt(target.transform);
            }
        }
        // Không tìm thấy mục tiêu 
        if (target == null)
        {
            if (targetless == false)
            {
                targetless = true;
            }
            else
            {
                // Thoát khỏi trạng thái nếu không tìm thấy đối tượng trong một frame
                aiBehavior.ChangeState(passiveAiState);
            }
        }
    }

    /// <summary>
    /// Lấy mục tiêu ưu tiên hàng đầu 
    /// </summary>
    /// <returns>Mục tiêu hàng đầu.</returns>
    private GameObject GetTopmostTarget()
    {
        GameObject res = null;
        if (useTargetPriority == true) // Lấy mục tiêu có khoảng cách tối thiểu
        {
            float minPathDistance = float.MaxValue;
            foreach (GameObject ai in targetsList)
            {
                if (ai != null)
                {
                    AiStatePatrol aiStatePatrol = ai.GetComponent<AiStatePatrol>();
                    float distance = aiStatePatrol.GetRemainingPath();
                    if (distance < minPathDistance)
                    {
                        minPathDistance = distance;
                        res = ai;
                    }
                }
            }
        }
        else // Lấy mục tiêu đầu tiên
        {
            res = targetsList[0];
        }
        // Xóa danh sách mục tiêu quan trọng
        targetsList.Clear();
        return res;
    }

    /// <summary>
    /// Mất mục tiêu
    /// </summary>
    private void LoseTarget()
    {
        target = null;
        targetless = false;
        myLastAttack = null;
    }

    /// <summary>
    /// Kích hoạt va chạm
    /// </summary>
    /// <param name="my">My.</param>
    /// <param name="other">Other.</param>
    public void TriggerEnter(Collider2D my, Collider2D other)
    {

    }
    // Tiếp tục va chạm 
     public void TriggerStay(Collider2D my, Collider2D other)
    {
        if (target == null) // Thêm mục tiêu mới vào danh sách
        {
            targetsList.Add(other.gameObject);
        }
        else // Tấn công mục tiêu hiện tại 
        {
            // Đang là mục tiêu hiện tại 
            if (target == other.gameObject)
            {
                if (my.name == "MeleeAttack") // Mục tiêu nằm trong phạm vi tấn công liền kề
                {
                    // Nếu có loại tấn công gần
                    if (meleeAttack != null)
                    {
                        // Ghi nhớ lần tấn công cuối cùng
                        myLastAttack = meleeAttack as IAttack;
                        // THực hiện tấn công
                        meleeAttack.Attack(other.transform);
                    }
                }
                else if (my.name == "RangedAttack") // Mục tiêu nằm trong phạm vi tấn công tầm xa
                {
                    if (rangedAttack != null)
                    {
                        if ((meleeAttack == null)
                            || ((meleeAttack != null) && (myLastAttack != meleeAttack)))
                        {
                            myLastAttack = rangedAttack as IAttack;
                            rangedAttack.Attack(other.transform);
                        }
                    }
                }
            }
        }
    }

    // Kết thúc va chạm  
    public void TriggerExit(Collider2D my, Collider2D other)
    {
        if (other.gameObject == target)
        {
            // Mục tiêu ra khỏi phạm vi tấn công
            LoseTarget();
        }
    }
}