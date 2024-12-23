using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayerFollw : MonoBehaviour
{
    public Transform target;           // ��ҽ�ɫ�� Transform
    public float distance = 5.0f;      // �R�^�c��ɫ�ľ��x
    public float height = 2.0f;        // �R�^�c��ɫ�ĸ߶Ȳ�
    public float smoothSpeed = 0.1f;   // ƽ�����S�ٶ�
    public float rotationSpeed = 5.0f; // �R�^���D���ٶ�

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        // �@ȡ��ɫ���᷽λ��
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        // ƽ���Ƅӵ�Ŀ��λ��
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // �����R�^λ��
        transform.position = smoothedPosition;

        // ׌�R�^�������R��ɫ�����D
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
