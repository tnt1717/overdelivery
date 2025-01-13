using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject targetObject; // 需要控制的物件

    void Start()
    {
        // 獲取 targetObject 上的 TargetScript 並將其禁用
        targetObject.GetComponent<BIKE>().enabled = false;
    }

    void Update()
    {
        // 按下空白鍵啟用 TargetScript
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetObject.GetComponent<BIKE>().enabled = true;
            Debug.Log("TargetScript 已啟用");
        }

        // 按下 F 鍵禁用 TargetScript
        if (Input.GetKeyDown(KeyCode.F))
        {
            targetObject.GetComponent<BIKE>().enabled = false;
            Debug.Log("TargetScript 已禁用");
        }
    }
}
