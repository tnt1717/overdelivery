using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC_CAR_manager : MonoBehaviour
{
    public GameObject[] npcPrefabs;         // 9個NPC車輛的模型陣列
    public Transform player;                // 玩家位置
    public float spawnInterval = 7f;        // 每七秒生成一次
    public float spawnDistance = 30f;       // NPC 距離玩家超過30時生成或銷毀
    public int roadAreaMask;                // 指定的 NavMesh 圖層


    private void Start()
    {
        StartCoroutine(SpawnNPC());
        int roadAreaMask = NavMesh.GetAreaFromName("road");
        Debug.Log("Road area mask: " + roadAreaMask);

    }

    private IEnumerator SpawnNPC()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPosition = GetSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                GameObject npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
                GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);

                // 設定 NPC 車輛，並將 roadAreaMask 傳遞給它
                npc.AddComponent<NPCVehicle>().Initialize(player, roadAreaMask, spawnDistance);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // 隨機生成在玩家 spawnDistance 距離外的位置
        Vector3 randomDirection = Random.insideUnitSphere * spawnDistance;
        randomDirection += player.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnDistance, roadAreaMask))
        {
            return hit.position;
        }

        return Vector3.zero;
    }
}
