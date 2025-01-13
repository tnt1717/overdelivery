using UnityEngine;
using UnityEngine.AI;

public class NPCVehicle : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Transform player;
    private float maxDistanceFromPlayer;
    private float patrolTime;
    private float patrolTimer;

    private float rotationSpeed = 200f; // 旋轉速度
    private float moveSpeed = 3f; // 移動速度
    private int roadAreaMask; // 指定的 NavMesh 圖層

    public void Initialize(Transform playerTransform, int areaMask, float distance)
    {
        player = playerTransform;
        maxDistanceFromPlayer = distance;
        roadAreaMask = areaMask; // 設定路徑圖層遮罩

        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent 未配置於車輛物件上！");
            return;
        }

        // 設定 NavMeshAgent 的相關參數
        navAgent.enabled = true;
        navAgent.areaMask = roadAreaMask; // 指定 NavMesh 圖層
        navAgent.speed = moveSpeed;
        navAgent.angularSpeed = rotationSpeed;

        // 隨機巡邏時間範圍
        patrolTime = Random.Range(5f, 10f);
        patrolTimer = 0f;

        // 放置車輛在有效的 NavMesh 上
        PlaceOnNavMesh();

        // 設定初始巡邏目的地
        SetNewDestination();
    }

    private void Update()
    {
        if (!navAgent.isOnNavMesh)
        {
            Debug.LogWarning("車輛還未放置到 NavMesh 上，無法開始巡邏！");
            return;
        }

        patrolTimer += Time.deltaTime;

        // 如果巡邏時間已過，設置新的巡邏目的地
        if (patrolTimer >= patrolTime)
        {
            patrolTimer = 0f;
            SetNewDestination();
        }

        // 檢查是否超過最大距離，超過則銷毀車輛
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > maxDistanceFromPlayer)
        {
            Destroy(gameObject);
        }

        // 確保車輛朝向行駛方向
        MoveAndRotate();
    }

    private void SetNewDestination()
    {
        // 檢查是否在有效的 NavMesh 上
        if (!navAgent.isOnNavMesh)
        {
            //Debug.LogWarning("車輛未在有效的 NavMesh 上，無法設置目的地！");
            return;
        }

        // 設置新目的地
        NavMeshHit hit;
        Vector3 randomDirection = Random.insideUnitSphere * 10f + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, roadAreaMask))
        {
            navAgent.SetDestination(hit.position);
            //Debug.Log($"設置新的目的地：{hit.position}");
        }
        else
        {
            Debug.LogWarning("未找到有效的 NavMesh 位置！");
        }
    }

    private void PlaceOnNavMesh()
    {
        NavMeshHit hit;
        Vector3 spawnPosition = transform.position;

        // 嘗試最多 10 次來找到有效的 NavMesh 位置
        int attempts = 0;
        while (attempts < 10)
        {
            if (NavMesh.SamplePosition(spawnPosition, out hit, 10f, roadAreaMask))
            {
                transform.position = hit.position; // 將車輛放置到最近的有效 NavMesh 點
                navAgent.Warp(hit.position); // 確保 agent 立即更新位置
                return;
            }
            else
            {
                // 隨機改變生成位置，直到找到有效位置
                spawnPosition = Random.insideUnitSphere * 10f + transform.position;
            }
            attempts++;
        }

        // 如果 10 次嘗試都沒找到有效位置，報錯
        Debug.LogError("未能找到車輛的有效 NavMesh 點，請檢查生成位置！");
    }

    private void MoveAndRotate()
    {
        // 讓車輛朝著其目的地方向移動
        if (navAgent.velocity.sqrMagnitude > 0.1f)
        {
            // 計算車輛應該面對的方向
            Quaternion targetRotation = Quaternion.LookRotation(navAgent.velocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
