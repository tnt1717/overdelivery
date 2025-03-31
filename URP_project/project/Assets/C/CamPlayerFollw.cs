using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayerFollw : MonoBehaviour
{
    public Transform target;           // 玩家角色的 Transform
    public float distance = 5.0f;      // 鏡頭與角色的距離
    public float height = 2.0f;        // 鏡頭與角色的高度差
    public float smoothSpeed = 0.1f;   // 平滑跟隨速度
    public float rotationSpeed = 5.0f; // 鏡頭旋轉的速度

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        // 獲取角色的後方位置
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        // 平滑移動到目標位置
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // 更新鏡頭位置
        transform.position = smoothedPosition;

        // 讓鏡頭緩慢對齊角色的旋轉
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
