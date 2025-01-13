using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LV1_tip_manager : MonoBehaviour
{
    public logueManager logueManager;
    public GameObject imageObject;

    // Start is called before the first frame update
    void Start()
    {
        // 檢查 logueManager 是否為 null
        if (logueManager == null)
        {
            // 如果為 null，顯示錯誤訊息，並停止繼續執行
            Debug.LogError("logueManager 尚未設置！請將 logueManager 拖到 Inspector 中的欄位！");
            return; // 這會終止 Start 方法的執行
        }

        // 隱藏圖片
        imageObject.SetActive(false);
        Debug.Log("logueManager設置！！");

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DisplayImageAndWait()
    {
        // 顯示圖片
        imageObject.gameObject.SetActive(true);

        // 等待玩家按下任意鍵
        yield return new WaitUntil(() => Input.anyKeyDown);

        // 玩家按下任意鍵後，關閉圖片物件
        imageObject.gameObject.SetActive(false);
    }
    IEnumerator WaitAndDoSomething()
    {
        // 等待三秒
        yield return new WaitForSeconds(3f);

        // 等待結束後執行的操作
        Debug.Log("三秒已經過去了！");
        // 您可以在這裡加入後續的操作，例如顯示圖片或啟動其他功能
    }
}
