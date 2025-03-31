using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // 車輛的 Transform
    public float smoothSpeed = 0.125f; // 平滑速度

    private Vector3 initialOffset;     // 攝影機與車輛的初始相對位置

    void Start()
    {
        // 保存攝影機與車輛之間的初始相對位置
        initialOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // 根據初始相對位置計算攝影機的目標位置
        Vector3 desiredPosition = target.position + initialOffset;

        // 使用 Lerp 來平滑移動到目標位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 保持攝影機的原始旋轉，不進行旋轉
        // 如果你希望攝影機保持當前角度，就不修改旋轉
    }
}
