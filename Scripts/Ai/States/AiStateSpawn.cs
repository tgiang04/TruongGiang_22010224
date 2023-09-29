using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tháp tạo ra đối tượng mới giứa các lần chờ  
/// </summary>
public class AiStateSpawn : MonoBehaviour, IAiState
{
    // Thời gian chờ giữa các lần tạo 
    public float cooldown = 10f;
    // Số lượng tối đa được tạo 
    public int maxNum = 2;
    // Các đối tượng được tạo sẽ sống sau khi AI bị hủy 
    public float delayAfterDestroy = 2f;
    // Tạo đối tượng prefabs 
    public GameObject prefab;
    // Vị trí để tạo đối tượng mới 
    public Transform spawnPoint;
    public string agressiveAiState;
    public string passiveAiState;
    private AiBehavior aiBehavior;
    // Điểm phòng thủ cho tháp 
    private DefendPoint defPoint;
    // Bộ đếm giữa các thời gian chờ 
    private float cooldownCounter;
    // Bộ đệm với các đối tượng được tạo
    private Dictionary<GameObject, Transform> defendersList = new Dictionary<GameObject, Transform>();
    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
    }
    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
    }
    void Awake ()
    {
        cooldownCounter = cooldown;
        aiBehavior = GetComponent<AiBehavior>();
        defPoint = transform.parent.GetComponentInChildren<DefendPoint>();
        Debug.Assert (aiBehavior && spawnPoint && defPoint, "Tham so khoi tao sai ");
    }
    public void OnStateEnter (string previousState, string newState)
    {

    }
    public void OnStateExit (string previousState, string newState)
    {

    }
    void Update ()
    {
        cooldownCounter += Time.deltaTime;
        if (cooldownCounter >= cooldown)
        {
            // Tạo đối tượng mới giữa cooldown 
            if (TryToSpawn() == true)
            {
                cooldownCounter = 0f;
            }
            else
            {
                cooldownCounter = cooldown;
            }
        }
    }

    // Lấy vị trí phòng thủ trống nếu có     
    private Transform GetFreeDefendPosition()
    {
        Transform res = null;
        List<Transform> points = defPoint.GetDefendPoints();
        foreach (Transform point in points)
        {
            // Nếu điểm chưa có gì 
            if (defendersList.ContainsValue(point) == false)
            {
                res = point;
                break;
            }
        }
        return res;
    }
    // Thử tạo ra đối tượng mới 
    private bool TryToSpawn()
    {
        bool res = false;
        // Nếu số lượng được tạo ít hơn đối tượng cho phép 
        if ((prefab != null) && (defendersList.Count < maxNum))
        {
            Transform position = GetFreeDefendPosition();
            // Còn vị trí trống 
            if (position != null)
            {
                // Tạo đối tượng
                GameObject obj = Instantiate<GameObject>(prefab, spawnPoint.position, spawnPoint.rotation);
                // Đặt vị trí đích 
                obj.GetComponent<AiStateMove>().destination = position;
                // Thêm vào bộ đệm 
                defendersList.Add(obj, position);
                res = true;
            }
        }
        return res;
    }

    //Gọi khi một đối tượng mất đi 
    private void UnitDie (GameObject obj, string param)
    {
        if (defendersList.ContainsKey(obj) == true)
        {
            // xóa khỏi bộ đệm
            defendersList.Remove(obj);
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

    void OnDestroy()
    {
        foreach (KeyValuePair<GameObject, Transform> defender in defendersList)
        {
            // Hủy toàn bộ đối tượng 
            Destroy(defender.Key, delayAfterDestroy);
        }
    }
}
