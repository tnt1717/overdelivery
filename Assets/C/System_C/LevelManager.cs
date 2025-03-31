using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    // 關卡條件的資料結構，包含訂單需求、錯誤率限制、時間限制
    public class LevelConditions
    {
        public int requiredOrders;  // 必須完成的訂單數
        public int maxErrors;       // 允許的最大錯誤數
        public int timeLimit;       // 時間限制

        public LevelConditions(int requiredOrders, int maxErrors, int timeLimit)
        {
            this.requiredOrders = requiredOrders;
            this.maxErrors = maxErrors;
            this.timeLimit = timeLimit;
        }
    }

    public Dictionary<string, LevelConditions> levelConditions; // 儲存所有關卡的條件
    public Text orderCountText;      // 顯示訂單完成數的文字
    public Text errorRateText;       // 顯示錯誤率的文字
    public Text timeLimitText;
    public Text orderCountTextW;      // 顯示訂單完成數的文字
    public Text errorRateTextW;       // 顯示錯誤率的文字
    public Text timeLimitTextW;       // 顯示時間限制的文字

    public int totalOrders;
    public int totalErrors;


    //結算區

    public Text totalOrdersText;                   // 顯示總訂單數量
    public Text totalErrorsText;                   // 顯示總錯誤數量

    public Text totalEarningsText;                 // 顯示獲得金額

    private float currentTime; // 當前剩餘時間
    private bool isTimerRunning = true; // 是否計時器正在運行

    private LevelConditions currentLevelConditions;
    private MoneyManager moneyManager;

    private void Start()
    {
        InitializeLevelConditions(); // 初始化關卡條件
        LoadLevel(SceneManager.GetActiveScene().name);  
        moneyManager=FindObjectOfType<MoneyManager>();

    }

    // 初始化所有關卡條件
    private void InitializeLevelConditions()
    {
        levelConditions = new Dictionary<string, LevelConditions>
        {
            { "LV1", new LevelConditions(1, 0, 100) },
            { "LV2", new LevelConditions(3, 2, 200) },
            { "LV3", new LevelConditions(10, 4, 300) },
            { "LV4", new LevelConditions(15, 5, 300) },
            { "LV5", new LevelConditions(22, 5, 300) },
            { "LV6", new LevelConditions(27, 6, 300) }


        };
    }

    // 載入特定關卡的條件
    public void LoadLevel(string levelName)
    {
        if (levelConditions.TryGetValue(levelName, out currentLevelConditions))
        {
            Debug.Log($"載入 {levelName}：訂單數量需求: {currentLevelConditions.requiredOrders}，最大錯誤數: {currentLevelConditions.maxErrors}，時間限制: {currentLevelConditions.timeLimit}秒");
            currentTime = currentLevelConditions.timeLimit;
        }
        else
        {
            Debug.LogWarning($"未找到關卡 {levelName} 的資料。");
        }
    }

    private void Update()
    {
        UpdateUI();
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime; // 減少時間

            // 當時間到達 0 時觸發結算
            if (currentTime <= 0f)
            {
                currentTime = 0f; // 防止時間變為負值
                Time.timeScale = 0.9f; // 暫停遊戲
                isTimerRunning = false; // 停止計時器
                moneyManager.CalculateFinalEarnings();
            }
        }
    }

    // 更新 UI 顯示
    private void UpdateUI()
    {

        orderCountText.text = $"訂單: {totalOrders}/{currentLevelConditions.requiredOrders}";
        errorRateText.text = $"錯誤: {totalErrors}/{currentLevelConditions.maxErrors}";
        //timeLimitText.text = $": {order_manager.orderTimer.ToString("f0")} / {currentLevelConditions.timeLimit} 秒";
        //customerNameText.text = " ";
        timeLimitText.text = $"時限:{Mathf.RoundToInt(currentTime)}/{currentLevelConditions.timeLimit}";

        orderCountTextW.text = $"訂單: {totalOrders}/{currentLevelConditions.requiredOrders}";
        errorRateTextW.text = $"錯誤: {totalErrors}/{currentLevelConditions.maxErrors}";
        timeLimitTextW.text = $"時限:{Mathf.RoundToInt(currentTime)}/{currentLevelConditions.timeLimit}";
    }

    // 顯示結算UI
    public void ShowResultUI(string completedOrders, string errors, string timeSpent, int earnings)
    {
;
    }



}
