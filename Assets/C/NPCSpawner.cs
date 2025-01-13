using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;     // NPC 模型陣列
    public GameObject navMeshObject;   // 包含 NavMesh 的物件
    public Transform parentFolder;     // 母資料夾，用於組織生成的 NPC
    public float spawnRadius = 20f;    // 生成範圍半徑
    public float spawnInterval = 10f;  // 每次生成的間隔時間

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // 每隔指定時間生成 NPC
        if (timer >= spawnInterval)
        {
            SpawnNPC();
            timer = 0f;
        }
    }

    void SpawnNPC()
    {
        // 隨機選擇一個 NPC 模型
        GameObject selectedNPC = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

        // 獲取 NavMesh 上的隨機點
        Vector3 spawnPosition = GetRandomPointOnNavMesh();

        if (spawnPosition != Vector3.zero)
        {
            // 在 NavMesh 上生成 NPC，並設置為母資料夾的子物件
            GameObject newNPC = Instantiate(selectedNPC, spawnPosition, Quaternion.identity, parentFolder);

            // 檢查是否有 NavMeshAgent 並啟用
            NavMeshAgent agent = newNPC.GetComponent<NavMeshAgent>();
            if (agent != null && agent.isOnNavMesh)
            {
                Debug.Log($"NPC '{newNPC.name}' spawned at {spawnPosition}");
            }
            else
            {
                Debug.LogWarning($"Failed to place NPC '{newNPC.name}' on NavMesh. Destroying it.");
                Destroy(newNPC); // 如果生成失敗，銷毀物件
            }
        }
        else
        {
            Debug.LogWarning("Failed to find a valid point on NavMesh.");
        }
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        // 在指定範圍內選取一個隨機點
        Vector3 randomPoint = Random.insideUnitSphere * spawnRadius;
        randomPoint += navMeshObject.transform.position; // 將隨機點相對於 NavMesh 物件的中心

        // 檢查該點是否在 NavMesh 上
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero; // 如果沒有找到有效點，返回 Vector3.zero
    }
}
