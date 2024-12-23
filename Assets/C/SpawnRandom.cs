using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRandom : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Ҫ���ɵ�������
    public float spawnInterval = 10f;  // �����g���r�g
    public float radius = 20f;         // NavMesh �S�Cλ�õİ돽
    public int maxRetries = 5;         // ÿ�����ɵ������ԇ�Δ�

    private float timer = 0f;
    private int failedSpawnAttempts = 0; // ӛ�����ʧ���ĴΔ�

    void Update()
    {
        timer += Time.deltaTime;

        // ÿ��ָ���r�g����һ���µ����
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        int retries = 0; // ��ԇ�Δ�Ӌ��

        while (retries < maxRetries)
        {
            // �� NavMesh ���ҵ�һ���S�Cλ��
            Vector3 randomPosition = GetRandomPointOnNavMesh();
            if (randomPosition != Vector3.zero)
            {
                // �S�C�x��һ�����
                GameObject prefabToSpawn = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

                // �����S�C�x�е����
                GameObject newObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);

                // �Lԇ��ʼ�� NavMeshAgent
                NavMeshAgent agent = newObject.GetComponent<NavMeshAgent>();
                if (agent != null && agent.isOnNavMesh)
                {
                    Vector3 randomDestination = GetRandomPointOnNavMesh();
                    if (randomDestination != Vector3.zero)
                    {
                        agent.SetDestination(randomDestination);

                        // �����N���z���_��
                        var destroyer = newObject.AddComponent<ObjectDestroyer>();
                        destroyer.Initialize(agent);
                    }
                    return; // �ɹ��������˳�
                }
                else
                {
                    Debug.LogWarning("Failed to initialize NavMeshAgent. Destroying object.");
                    Destroy(newObject); // �������o������� NavMesh���N����
                }
            }

            retries++;
        }

        // �����ԇ�_��������Ȼʧ��������ʧ��Ӌ��
        failedSpawnAttempts++;
        Debug.LogWarning($"Spawn failed! Total failed attempts: {failedSpawnAttempts}");
    }

    // �@ȡ NavMesh �ϵ��S�C�c
    Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomPoint = Random.insideUnitSphere * radius;
        randomPoint += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            // �z���c�Ƿ���򿿽� NavMesh ����Ч
            if (Vector3.Distance(hit.position, randomPoint) <= radius)
            {
                return hit.position;
            }
        }
        return Vector3.zero;  // ����]���ҵ���Ч�c������ Vector3.zero
    }
}

// ��������_�K�c���N���Լ�
public class ObjectDestroyer : MonoBehaviour
{
    private NavMeshAgent agent;

    public void Initialize(NavMeshAgent navAgent)
    {
        agent = navAgent;
    }

    void Update()
    {
        // ��������_�K�c����Ҿ��x���^ 20 �r�N��
        if (agent != null && agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Destroy(gameObject);
        }
    }
}
