using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtPlayer : MonoBehaviour
{
    public Transform player; // ��ҵ�λ��

    void Update()
    {
        if (player != null)
        {
            // ʹ�����������
            transform.LookAt(player);
        }
    }
}
