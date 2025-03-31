using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ticketManager : MonoBehaviour
{
    // 引用 UI 的文字元件
    public Text sceneNameText;
    public GameObject ui;
    public GameObject pass;
    private PlayerManager playerManager;
    void Start()
    {

        ui.SetActive(false);
        pass.gameObject. SetActive(false);

        GameObject playerSys = GameObject.Find("PlayerSys");
        if (playerSys != null)
        {
            playerManager = playerSys.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("在 'PlayerSys' 上未找到 PlayerManager 組件！");
                return;
            }
        }
        else
        {
            Debug.LogError("未找到名稱為 'PlayerSys' 的物件！");
            return;
        }
        // 初始化時更新文字為當前場景名稱
        UpdateSceneName();
    }

    void Update()
    {
        // 檢查按鍵 F7 是否被按下
        if (Input.GetKeyDown(KeyCode.F7))
        {
            TriggerActionF7();
        }

        // 檢查按鍵 F8 是否被按下
        if (Input.GetKeyDown(KeyCode.F8))
        {
            TriggerActionF8();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            ui.SetActive(false);
            pass.gameObject.SetActive(false);


        }
    }

        // 更新文字為當前場景名稱
        private void UpdateSceneName()
    {
        if (sceneNameText != null)
        {
            sceneNameText.text = SceneManager.GetActiveScene().name;
        }
        
    }

    // F7 按鍵觸發的方法
    private void TriggerActionF7()
    {
        Debug.Log("F7 triggered: Perform your action here.");
        ui.SetActive(true);
        AudioManager.Instance.PlaySound("coin");
        playerManager.playerData.coins -= 1500;
        
        // 可以在這裡加入你希望執行的行為
    }

    // F8 按鍵觸發的方法
    private void TriggerActionF8()
    {
        ui.active = true;
        pass.gameObject.SetActive(true);
        
        
        Debug.Log("F8 triggered: Perform your action here.");
        // 可以在這裡加入另一種行為
    }
}
