using System.Collections;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    private float rotationSpeed = 100f;  // ���D�ٶ�
    private bool isDragging = false;     // �z���Ƿ������τ�
    private float previousMouseX = 0f;   // ������һ�λ���λ��

    void Update()
    {
        // �z���Ƿ������I
        if (Input.GetMouseButtonDown(0))
        {
            // �������������ϣ��_ʼ�τ�
            isDragging = true;
            previousMouseX = Input.mousePosition.x;  // ӛ䛮�ǰ�Ļ��� X ����
        }

        // �������ס�K�τ�
        if (isDragging)
        {
            float deltaX = Input.mousePosition.x - previousMouseX;  // Ӌ�㻬���Ƅӵľ��x
            float rotationAmount = deltaX * rotationSpeed * Time.deltaTime;  // Ӌ�����D�ĽǶ�
            transform.Rotate(0f, -rotationAmount, 0f, Space.World);  // ���� X �S���Ƅ������D���

            previousMouseX = Input.mousePosition.x;  // ������һ�λ���λ��
        }

        // ���������I��_�rֹͣ�τ�
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
