using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class path2 : MonoBehaviour
{
    public Transform player;               // ��ҵ�λ��
    private Transform target;              // Ŀ�����
    private LineRenderer lineRenderer;     // �����Ⱦ������
    private NavMeshPath navPath;           // NavMesh ·��

    public float updateFrequency = 0.5f;   // ����·�����l�� (��)
                                           // Start is called before the first frame update

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        navPath = new NavMeshPath();

        // �O�� LineRenderer �Č���
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.positionCount = 0;

        // ���Ӆf�̵ȴ�Ŀ���������
        StartCoroutine(WaitForTarget());
    }

    // �f�̣��ȴ�Ŀ��������ɁK�_ʼ����·��
    private IEnumerator WaitForTarget()
    {
        while (target == null)
        {
            GameObject targetObject = GameObject.FindWithTag("order_end");


            if (targetObject != null)
            {
                target = targetObject.transform;
                StartCoroutine(UpdatePathRoutine()); // �_ʼ����·��
            }
            else
            {

                yield return new WaitForSeconds(0.3f); // ÿ0.5�����}�z�y
            }
        }
    }

    // ʹ�Åf�̶��ڸ���·��
    private IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            UpdatePath();
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    // Ӌ��·���K���� LineRenderer
    private void UpdatePath()
    {
        if (target != null && NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, navPath))
        {
            // �O�� LineRenderer ���c��
            lineRenderer.positionCount = navPath.corners.Length;

            // ��ÿ��·���c�O�õ� LineRenderer
            for (int i = 0; i < navPath.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, navPath.corners[i]);
            }
        }
        else
        {
            // ����o��Ӌ��·�����t�[��������
            lineRenderer.positionCount = 0;
        }
    }
}
