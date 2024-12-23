using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // ܇�v�� Transform
    public float smoothSpeed = 0.125f; // ƽ���ٶ�

    private Vector3 initialOffset;     // �zӰ�C�c܇�v�ĳ�ʼ����λ��

    void Start()
    {
        // ����zӰ�C�c܇�v֮�g�ĳ�ʼ����λ��
        initialOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // ������ʼ����λ��Ӌ��zӰ�C��Ŀ��λ��
        Vector3 desiredPosition = target.position + initialOffset;

        // ʹ�� Lerp ��ƽ���Ƅӵ�Ŀ��λ��
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ���֔zӰ�C��ԭʼ���D�����M�����D
        // �����ϣ���zӰ�C���֮�ǰ�Ƕȣ��Ͳ��޸����D
    }
}
