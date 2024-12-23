using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRandom : MonoBehaviour
{
    public GameObject[] objectPrefabs; // 要生成的物件陣列
    public float spawnInterval = 10f;  // 生成間隔時間
    public float radius = 20f;         // NavMesh 隨機位置的半徑
    public int maxRetries = 5;         // 每次生成的最大重試次數

    private float timer = 0f;
    private int failedSpawnAttempts = 0; // 記錄生成失敗的次數

    void Update()
    {
        timer += Time.deltaTime;

        // 每隔指定時間生成一個新的物件
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        int retries = 0; // 重試次數計數

        while (retries < maxRetries)
        {
            // 在 NavMesh 上找到一個隨機位置
            Vector3 randomPosition = GetRandomPointOnNavMesh();
            if (randomPosition != Vector3.zero)
            {
                // 隨機選擇一個物件
                GameObject prefabToSpawn = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

                // 生成隨機選中的物件
                GameObject newObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);

                // 嘗試初始化 NavMeshAgent
                NavMeshAgent agent = newObject.GetComponent<NavMeshAgent>();
                if (agent != null && agent.isOnNavMesh)
                {
                    Vector3 randomDestination = GetRandomPointOnNavMesh();
                    if (randomDestination != Vector3.zero)
                    {
                        agent.SetDestination(randomDestination);

                        // 附加銷毀檢查腳本
                        var destroyer = newObject.AddComponent<ObjectDestroyer>();
                        destroyer.Initialize(agent);
                    }
                    return; // 成功生成後退出
                }
                else
                {
                    Debug.LogWarning("Failed to initialize NavMeshAgent. Destroying object.");
                    Destroy(newObject); // 如果物件無法放置於 NavMesh，銷毀它
                }
            }

            retries++;
        }

        // 如果重試達到上限仍然失敗，增加失敗計數
        failedSpawnAttempts++;
        Debug.LogWarning($"Spawn failed! Total failed attempts: {failedSpawnAttempts}");
    }

    // 獲取 NavMesh 上的隨機點
    Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomPoint = Random.insideUnitSphere * radius;
        randomPoint += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            // 檢查點是否足夠靠近 NavMesh 且有效
            if (Vector3.Distance(hit.position, randomPoint) <= radius)
            {
                return hit.position;
            }
        }
        return Vector3.zero;  // 如果沒有找到有效點，返回 Vector3.zero
    }
}

// 當物件到達終點後銷毀自己
public class ObjectDestroyer : MonoBehaviour
{
    private NavMeshAgent agent;

    public void Initialize(NavMeshAgent navAgent)
    {
        agent = navAgent;
    }

    void Update()
    {
        // 當物件到達終點且玩家距離超過 20 時銷毀
        if (agent != null && agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Destroy(gameObject);
        }
    }
}
