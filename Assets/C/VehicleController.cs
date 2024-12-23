using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float speed = 5f; // ܇�v���Ƅ��ٶ�
    public float turnSpeed = 10f; // ܇�v���D���ٶ�
    private Transform[] corners; // �L���w���Ă�����
    private int currentCornerIndex; // ��ǰĿ�˽��������
    public int CornerVisitCount { get; private set; } = 0; // ��Ѳ߉�Ľ��䔵��

    public void Initialize(Transform[] cornerPoints, int startingIndex)
    {
        corners = cornerPoints;
        currentCornerIndex = (startingIndex + 1) % corners.Length; // �O����ʼĿ�˽���
        transform.position = corners[startingIndex].position; // ��܇�vλ���O�Þ���ʼ����
        CornerVisitCount = 1; // ��ʼѲ߉ӛ�
    }

    void Update()
    {
        MoveTowardsCorner();
    }

    void MoveTowardsCorner()
    {
        if (corners == null || corners.Length == 0) return;

        // �@ȡĿ�˽����λ��
        Vector3 targetPosition = corners[currentCornerIndex].position;

        // Ӌ�㷽���c���D
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // �Ƅ�܇�v
        transform.position += direction * speed * Time.deltaTime;

        // ƽ���D��
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // �z�y�Ƿ��_Ŀ�˽���
        if (Vector3.Distance(transform.position, targetPosition) <0.9f)
        {
            // ���µ���һ������
            currentCornerIndex = (currentCornerIndex + 1) % corners.Length;

            // ����Ѳ߉ӛ�
            CornerVisitCount++;
        }
    }
}
