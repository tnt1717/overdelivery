using System.Collections;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    private float rotationSpeed = 100f;  // 旋轉速度
    private bool isDragging = false;     // 檢查是否正在拖動
    private float previousMouseX = 0f;   // 儲存上一次滑鼠位置

    void Update()
    {
        // 檢查是否按下左鍵
        if (Input.GetMouseButtonDown(0))
        {
            // 如果滑鼠在物件上，開始拖動
            isDragging = true;
            previousMouseX = Input.mousePosition.x;  // 記錄當前的滑鼠 X 坐標
        }

        // 如果滑鼠按住並拖動
        if (isDragging)
        {
            float deltaX = Input.mousePosition.x - previousMouseX;  // 計算滑鼠移動的距離
            float rotationAmount = deltaX * rotationSpeed * Time.deltaTime;  // 計算旋轉的角度
            transform.Rotate(0f, -rotationAmount, 0f, Space.World);  // 根據 X 軸的移動量旋轉物件

            previousMouseX = Input.mousePosition.x;  // 更新上一次滑鼠位置
        }

        // 當滑鼠左鍵鬆開時停止拖動
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
