using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Cơ chế sinh kẻ địch
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Cơ chế làn sóng
    /// </summary>
    [System.Serializable]
    public class Wave
    {
        // Độ trễ trước khi vào đợt sóng
        public float timeBeforeWave;
        // Danh sách kẻ thù trong đợt sóng 
        public List<GameObject> enemies;
    }

    // Nếu kẻ thù không được đặt, thư mục này sẽ được sử dụng để lấy kẻ thù ngẫu nhiên
    public string enemiesResourceFolder = "Prefabs/Enemies";
    // Kẻ thù sẽ có tốc độ khác nhau trong khoảng thời gian nhất định
    public float speedRandomizer = 0.2f;
    // Độ trễ giữa kẻ thù xuất hiện trong đợt
    public float unitSpawnDelay = 0.8f;
    // Danh sách sinh sản 
    public List<Wave> waves;

    // Kẻ thù di chuyển theo Pathway
    private Pathway path;
    // Làn sóng gần nhất
    private Wave nextWave;
    // Bộ đếm độ trễ 
    private float counter;
    // Bắt đầu đợt sóng
    private bool waveInProgress;
    // Danh sách tạo kẻ địch ngẫu nhiên
    private List<GameObject> enemyPrefabs;
    // Bộ đệm với kẻ thù sinh ra đang hoạt động
    private List<GameObject> activeEnemies = new List<GameObject>();
    void Awake ()
    {
        path = GetComponentInParent<Pathway>();
        // Tải prefab kẻ thù từ thư mục chỉ định
        enemyPrefabs = Resources.LoadAll<GameObject>(enemiesResourceFolder).ToList();
        Debug.Assert((path != null) && (enemyPrefabs != null), "Tham số khởi tạo sai");
    }
    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
    }
    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
    }
    void Start()
    {
        if (waves.Count > 0)
        {
            // Bắt đầu từ đợt đầu tiên
            nextWave = waves[0];
        }
    }
    void Update()
    {
        // Chờ đến đợt tiếp theo
        if ((nextWave != null) && (waveInProgress == false))
        {
            counter += Time.deltaTime;
            if (counter >= nextWave.timeBeforeWave)
            {
                counter = 0f;
                // Bắt đầu đợt mới
                StartCoroutine(RunWave());
            }
        }
        // Nếu tất cả kẻ thù chết
        if ((nextWave == null) && (activeEnemies.Count <= 0))
        {
            EventManager.TriggerEvent("AllEnemiesAreDead", null, null);
            // Tắt cơ chế sinh kẻ thù
            enabled = false;
        }
    }
    private void GetNextWave()
    {
        int idx = waves.IndexOf(nextWave) + 1;
        if (idx < waves.Count)
        {
            nextWave = waves[idx];
        }
        else
        {
            nextWave = null;
        }
    }
    private IEnumerator RunWave()
    {
        waveInProgress = true;
        foreach (GameObject enemy in nextWave.enemies)
        {
            GameObject prefab = null;
            prefab = enemy;
            // Nếu prefab kẻ thù không địch chỉ định, lấy kẻ thù ngẫu nhiên
            if (prefab == null)
            {
                prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            }
            // Tạo kẻ địch
            GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
            // Cài đặt đường dẫn
            newEnemy.GetComponent<AiStatePatrol>().path = path;
            NavAgent agent = newEnemy.GetComponent<NavAgent>();
            // Cài đặt tốc độ offset
            agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
            // Thêm kẻ thù vào danh sách
            activeEnemies.Add(newEnemy);
            // Đợi độ trễ trước khi kẻ địch chạy tiếp
            yield return new WaitForSeconds(unitSpawnDelay);
        }
        GetNextWave();
        waveInProgress = false;
    }
    private void UnitDie(GameObject obj, string param)
    {
        // Nếu kẻ thù vẫn hoạt động
        if (activeEnemies.Contains(obj) == true)
        {
            // Xóa khỏi bộ đệm
            activeEnemies.Remove(obj);
        }
    }
}
