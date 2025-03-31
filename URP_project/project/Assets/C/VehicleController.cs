using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float speed = 5f; // 車輛的移動速度
    public float turnSpeed = 10f; // 車輛的轉彎速度
    private Transform[] corners; // 長方體的四個角落
    private int currentCornerIndex; // 當前目標角落的索引
    public int CornerVisitCount { get; private set; } = 0; // 已巡邏的角落數量

    public void Initialize(Transform[] cornerPoints, int startingIndex)
    {
        corners = cornerPoints;
        currentCornerIndex = (startingIndex + 1) % corners.Length; // 設定起始目標角落
        transform.position = corners[startingIndex].position; // 將車輛位置設置為起始角落
        CornerVisitCount = 1; // 初始巡邏記錄
    }

    void Update()
    {
        MoveTowardsCorner();
    }

    void MoveTowardsCorner()
    {
        if (corners == null || corners.Length == 0) return;

        // 獲取目標角落的位置
        Vector3 targetPosition = corners[currentCornerIndex].position;

        // 計算方向與旋轉
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // 移動車輛
        transform.position += direction * speed * Time.deltaTime;

        // 平滑轉向
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // 檢測是否到達目標角落
        if (Vector3.Distance(transform.position, targetPosition) <0.9f)
        {
            // 更新到下一個角落
            currentCornerIndex = (currentCornerIndex + 1) % corners.Length;

            // 增加巡邏記錄
            CornerVisitCount++;
        }
    }
}
