using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10f;       // 車輛的移動速度
    public float turnSpeed = 70f;       // 車輛的轉向速度

    public float rightingSpeed = 1f; // 回正速度
    public float maxTiltAngle = 20f; // 觸發翻正的角度閾值
    public GameObject helperCollider; // 幫助翻正的球體碰撞箱
    public float disableDelay = 2f; // 禁用球體碰撞箱的延遲時間

    private bool isRighting = false; // 是否正在回正
    private float disableTimer = 0f; // 碰撞箱禁用的計時器
    void Start()
    {
        if (helperCollider != null)
        {
            helperCollider.SetActive(false); // 初始時禁用球體碰撞箱
        }
    }
    void Update()
    {
        // 垂直方向控制車輛的前進和後退（W/S 鍵）
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // 水平方向控制車輛的左右轉向（A/D 鍵）
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // 移動車輛沿著當前前進方向
        transform.Translate(Vector3.forward * move);

        // 旋轉車輛左右轉向
        transform.Rotate(Vector3.up * turn);
        if (helperCollider != null)
        {
            helperCollider.SetActive(false); // 初始時禁用球體碰撞箱
        }
        CheckAndRightCar();

        // 處理球體碰撞箱禁用的計時
        if (isRighting && helperCollider != null && disableTimer > 0)
        {
            disableTimer -= Time.deltaTime;
            if (disableTimer <= 0)
            {
                helperCollider.SetActive(false); // 禁用球體碰撞箱
                isRighting = false;
            }
        }
    }
    private void CheckAndRightCar()
    {
        // 獲取當前的 Z 軸旋轉角度，確保在 -180 到 180 範圍內
        float zRotation = NormalizeAngle(transform.eulerAngles.z);

        // 如果 Z 軸角度超過閾值，啟動回正
        if (Mathf.Abs(zRotation) > maxTiltAngle)
        {
            isRighting = true;

            // 啟用球體碰撞箱
            if (helperCollider != null)
            {
                helperCollider.SetActive(true);
                disableTimer = disableDelay; // 設定禁用延遲
            }

            // 將車輛緩慢回正
            float newZ = Mathf.LerpAngle(zRotation, 0, rightingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZ);

            // 當 Z 軸接近 0 時，認為回正完成
            if (Mathf.Abs(newZ) < 0.1f)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
        }
    }

    // 將角度轉換為 -180 到 180 範圍
    private float NormalizeAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }

}

