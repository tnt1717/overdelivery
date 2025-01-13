using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public Material skyboxMaterial;  // 用來參考您的天空盒材質
    public float transitionSpeed = 2f; // 變化速度 (每分鐘變化的次數)

    private float transitionValue = 0f; // 當前的過渡值
    private bool isIncreasing = true; // 用來控制是增加還是減少

    void Update()
    {
        // 每幀計算過渡值
        if (isIncreasing)
        {
            transitionValue += Time.deltaTime * transitionSpeed / 60f; // 將時間轉換為分鐘
            if (transitionValue >= 0.8f)
            {
                transitionValue = 0.8f; // 確保不超過0.6
                isIncreasing = false; // 改變狀態為減少
            }
        }
        else
        {
            transitionValue -= Time.deltaTime * transitionSpeed / 60f; // 將時間轉換為分鐘
            if (transitionValue <= 0.4f)
            {
                transitionValue = 0.4f; // 確保不低於0
                isIncreasing = true; // 改變狀態為增加
            }
        }

        // 更新天空盒的 CubemapTransition 值
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_CubemapTransition", transitionValue);
        }
    }
}
