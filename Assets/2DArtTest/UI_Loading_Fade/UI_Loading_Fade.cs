using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class UI_Loading_Fade : MonoBehaviour
{
    public GameObject loading_0;
    public GameObject loading_1;
    public Vector3 targetScale_0 = new Vector3(20f, 20f, 20f); // 目標縮放比例
    public Vector3 targetScale_1 = new Vector3(0f, 0f, 0f);
    public float scaleSpeed_0 = 10; // 縮放速度
    public float scaleSpeed_1 = 5f;
    private Vector3 initialScale; // 初始比例

    bool testQ = false;
    bool testW = false;

    private void Start()
    {
        initialScale = transform.localScale; // 設定初始比例
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            testQ = true;
            testW = false;
            loading_0.SetActive(true);
            loading_1.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            testQ = false;
            testW = true;
            loading_0.SetActive(true);
            loading_1.SetActive(true);
        }

        //放大
        if (testQ == true && testW == false)
        {
            // 緩慢縮放到目標比例
            loading_0.transform.localScale = Vector3.Lerp(loading_0.transform.localScale, targetScale_0, scaleSpeed_0 * Time.deltaTime);

            // 判斷是否接近目標比例，防止不斷計算
            if (Vector3.Distance(loading_0.transform.localScale, targetScale_0) < 12f)
            {
                loading_0.transform.localScale = targetScale_0;
                loading_0.SetActive(false);
                loading_1.SetActive(false);
            }
        } 

        //縮小
        else if (testQ == false && testW == true)
        {
            // 緩慢縮放到目標比例
            loading_0.transform.localScale = Vector3.Lerp(loading_0.transform.localScale, targetScale_1, scaleSpeed_1 * Time.deltaTime);

            // 判斷是否接近目標比例，防止不斷計算
            if (Vector3.Distance(loading_0.transform.localScale, targetScale_1) < 0.1f)
            {
                loading_0.transform.localScale = targetScale_1;
                loading_0.SetActive(false);
                //loading_1.SetActive(false);
            }
        }
    }

    // 呼叫此方法可以重設大小到初始比例
    public void ResetScale()
    {
        loading_0.transform.localScale = initialScale;
    }
}
