using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10f;       // ܇�v���Ƅ��ٶ�
    public float turnSpeed = 70f;       // ܇�v���D���ٶ�

    public float rightingSpeed = 1f; // �����ٶ�
    public float maxTiltAngle = 20f; // �|�l�����ĽǶ��ֵ
    public GameObject helperCollider; // �������������w��ײ��
    public float disableDelay = 2f; // �������w��ײ������t�r�g

    private bool isRighting = false; // �Ƿ����ڻ���
    private float disableTimer = 0f; // ��ײ����õ�Ӌ�r��
    void Start()
    {
        if (helperCollider != null)
        {
            helperCollider.SetActive(false); // ��ʼ�r�������w��ײ��
        }
    }
    void Update()
    {
        // ��ֱ�������܇�v��ǰ�M�����ˣ�W/S �I��
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // ˮƽ�������܇�v�������D��A/D �I��
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // �Ƅ�܇�v������ǰǰ�M����
        transform.Translate(Vector3.forward * move);

        // ���D܇�v�����D��
        transform.Rotate(Vector3.up * turn);
        if (helperCollider != null)
        {
            helperCollider.SetActive(false); // ��ʼ�r�������w��ײ��
        }
        CheckAndRightCar();

        // ̎�����w��ײ����õ�Ӌ�r
        if (isRighting && helperCollider != null && disableTimer > 0)
        {
            disableTimer -= Time.deltaTime;
            if (disableTimer <= 0)
            {
                helperCollider.SetActive(false); // �������w��ײ��
                isRighting = false;
            }
        }
    }
    private void CheckAndRightCar()
    {
        // �@ȡ��ǰ�� Z �S���D�Ƕȣ��_���� -180 �� 180 ������
        float zRotation = NormalizeAngle(transform.eulerAngles.z);

        // ��� Z �S�Ƕȳ��^�ֵ�����ӻ���
        if (Mathf.Abs(zRotation) > maxTiltAngle)
        {
            isRighting = true;

            // �������w��ײ��
            if (helperCollider != null)
            {
                helperCollider.SetActive(true);
                disableTimer = disableDelay; // �O���������t
            }

            // ��܇�v��������
            float newZ = Mathf.LerpAngle(zRotation, 0, rightingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZ);

            // �� Z �S�ӽ� 0 �r���J��������
            if (Mathf.Abs(newZ) < 0.1f)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
        }
    }

    // ���Ƕ��D�Q�� -180 �� 180 ����
    private float NormalizeAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }

}

