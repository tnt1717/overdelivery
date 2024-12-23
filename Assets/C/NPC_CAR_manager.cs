using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC_CAR_manager : MonoBehaviour
{
    public GameObject[] npcPrefabs;         // 9��NPC܇�v��ģ�����
    public Transform player;                // ���λ��
    public float spawnInterval = 7f;        // ÿ��������һ��
    public float spawnDistance = 30f;       // NPC ���x��ҳ��^30�r���ɻ��N��
    public int roadAreaMask;                // ָ���� NavMesh �D��


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

                // �O�� NPC ܇�v���K�� roadAreaMask ���f�o��
                npc.AddComponent<NPCVehicle>().Initialize(player, roadAreaMask, spawnDistance);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // �S�C��������� spawnDistance ���x���λ��
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
