using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;     // NPC ģ�����
    public GameObject navMeshObject;   // ���� NavMesh �����
    public Transform parentFolder;     // ĸ�Y�ϊA����춽M�����ɵ� NPC
    public float spawnRadius = 20f;    // ���ɹ����돽
    public float spawnInterval = 10f;  // ÿ�����ɵ��g���r�g

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // ÿ��ָ���r�g���� NPC
        if (timer >= spawnInterval)
        {
            SpawnNPC();
            timer = 0f;
        }
    }

    void SpawnNPC()
    {
        // �S�C�x��һ�� NPC ģ��
        GameObject selectedNPC = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

        // �@ȡ NavMesh �ϵ��S�C�c
        Vector3 spawnPosition = GetRandomPointOnNavMesh();

        if (spawnPosition != Vector3.zero)
        {
            // �� NavMesh ������ NPC���K�O�Þ�ĸ�Y�ϊA�������
            GameObject newNPC = Instantiate(selectedNPC, spawnPosition, Quaternion.identity, parentFolder);

            // �z���Ƿ��� NavMeshAgent �K����
            NavMeshAgent agent = newNPC.GetComponent<NavMeshAgent>();
            if (agent != null && agent.isOnNavMesh)
            {
                Debug.Log($"NPC '{newNPC.name}' spawned at {spawnPosition}");
            }
            else
            {
                Debug.LogWarning($"Failed to place NPC '{newNPC.name}' on NavMesh. Destroying it.");
                Destroy(newNPC); // �������ʧ�����N�����
            }
        }
        else
        {
            Debug.LogWarning("Failed to find a valid point on NavMesh.");
        }
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        // ��ָ���������xȡһ���S�C�c
        Vector3 randomPoint = Random.insideUnitSphere * spawnRadius;
        randomPoint += navMeshObject.transform.position; // ���S�C�c����� NavMesh ���������

        // �z��ԓ�c�Ƿ��� NavMesh ��
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero; // ����]���ҵ���Ч�c������ Vector3.zero
    }
}
