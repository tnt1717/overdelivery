using UnityEngine;
using UnityEngine.AI;

public class NPCVehicle : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Transform player;
    private float maxDistanceFromPlayer;
    private float patrolTime;
    private float patrolTimer;

    private float rotationSpeed = 200f; // ���D�ٶ�
    private float moveSpeed = 3f; // �Ƅ��ٶ�
    private int roadAreaMask; // ָ���� NavMesh �D��

    public void Initialize(Transform playerTransform, int areaMask, float distance)
    {
        player = playerTransform;
        maxDistanceFromPlayer = distance;
        roadAreaMask = areaMask; // �O��·���D������

        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent δ�����܇�v����ϣ�");
            return;
        }

        // �O�� NavMeshAgent �����P����
        navAgent.enabled = true;
        navAgent.areaMask = roadAreaMask; // ָ�� NavMesh �D��
        navAgent.speed = moveSpeed;
        navAgent.angularSpeed = rotationSpeed;

        // �S�CѲ߉�r�g����
        patrolTime = Random.Range(5f, 10f);
        patrolTimer = 0f;

        // ����܇�v����Ч�� NavMesh ��
        PlaceOnNavMesh();

        // �O����ʼѲ߉Ŀ�ĵ�
        SetNewDestination();
    }

    private void Update()
    {
        if (!navAgent.isOnNavMesh)
        {
            Debug.LogWarning("܇�v߀δ���õ� NavMesh �ϣ��o���_ʼѲ߉��");
            return;
        }

        patrolTimer += Time.deltaTime;

        // ���Ѳ߉�r�g���^���O���µ�Ѳ߉Ŀ�ĵ�
        if (patrolTimer >= patrolTime)
        {
            patrolTimer = 0f;
            SetNewDestination();
        }

        // �z���Ƿ��^�����x�����^�t�N��܇�v
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > maxDistanceFromPlayer)
        {
            Destroy(gameObject);
        }

        // �_��܇�v������񂷽��
        MoveAndRotate();
    }

    private void SetNewDestination()
    {
        // �z���Ƿ�����Ч�� NavMesh ��
        if (!navAgent.isOnNavMesh)
        {
            //Debug.LogWarning("܇�vδ����Ч�� NavMesh �ϣ��o���O��Ŀ�ĵأ�");
            return;
        }

        // �O����Ŀ�ĵ�
        NavMeshHit hit;
        Vector3 randomDirection = Random.insideUnitSphere * 10f + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, roadAreaMask))
        {
            navAgent.SetDestination(hit.position);
            //Debug.Log($"�O���µ�Ŀ�ĵأ�{hit.position}");
        }
        else
        {
            Debug.LogWarning("δ�ҵ���Ч�� NavMesh λ�ã�");
        }
    }

    private void PlaceOnNavMesh()
    {
        NavMeshHit hit;
        Vector3 spawnPosition = transform.position;

        // �Lԇ��� 10 �΁��ҵ���Ч�� NavMesh λ��
        int attempts = 0;
        while (attempts < 10)
        {
            if (NavMesh.SamplePosition(spawnPosition, out hit, 10f, roadAreaMask))
            {
                transform.position = hit.position; // ��܇�v���õ��������Ч NavMesh �c
                navAgent.Warp(hit.position); // �_�� agent ��������λ��
                return;
            }
            else
            {
                // �S�C��׃����λ�ã�ֱ���ҵ���Чλ��
                spawnPosition = Random.insideUnitSphere * 10f + transform.position;
            }
            attempts++;
        }

        // ��� 10 �·Lԇ���]�ҵ���Чλ�ã����e
        Debug.LogError("δ���ҵ�܇�v����Ч NavMesh �c��Ո�z������λ�ã�");
    }

    private void MoveAndRotate()
    {
        // ׌܇�v������Ŀ�ĵط����Ƅ�
        if (navAgent.velocity.sqrMagnitude > 0.1f)
        {
            // Ӌ��܇�v��ԓ�挦�ķ���
            Quaternion targetRotation = Quaternion.LookRotation(navAgent.velocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
