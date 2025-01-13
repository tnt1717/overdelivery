using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarRatingManager: MonoBehaviour
{
    // 星星圖片
    public Image starA;
    public Image starB;
    public Image starC;


    public Text cmp;
    public Text error;
    public Text delay;
    public Text totalcoins;

    // 星星的啟用圖片與未啟用圖片
    public Sprite filledStar;  // 啟用星星圖片
    public Sprite emptyStar;   // 未啟用星星圖
    private PlayerManager playerManager;

    public GameObject ui;
    private RandomModelSpawner randomModelSpawner;


    // 儲存每一關的評分條件
    [System.Serializable]
    public class LevelStarCondition
    {
        public int totalOrders;  // 總單數條件
        public int maxErrors;    // 錯誤最大值
        public int minCorrect;   // 正確最小值
    }

    public Dictionary<string, LevelStarCondition> levelConditions = new Dictionary<string, LevelStarCondition>();

    public void CloseAllOtherUIByLayer()
    {
        // 獲取當前物件所在的 Layer
        int uiLayer = LayerMask.NameToLayer("UI");

        // 遍歷場景中所有激活的物件
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // 檢查物件是否在 UI 層並且不是當前物件
            if (obj.layer == uiLayer && obj != this.gameObject && obj.activeSelf)
            {
                obj.SetActive(false); // 關閉其他 UI
            }
        }
    }
    private void Start()
    {

        ui.gameObject.SetActive(false);

        randomModelSpawner=FindObjectOfType<RandomModelSpawner>();
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

        // 初始化每一關條件
        levelConditions.Add("LV1", new LevelStarCondition { totalOrders = 1, maxErrors = 0, minCorrect = 1 });
        levelConditions.Add("LV2", new LevelStarCondition { totalOrders = 4, maxErrors = 2, minCorrect = 3 });
        levelConditions.Add("LV3", new LevelStarCondition { totalOrders = 15, maxErrors = 4, minCorrect = 10 });
        levelConditions.Add("LV4", new LevelStarCondition { totalOrders = 20, maxErrors = 5, minCorrect = 15 });
        levelConditions.Add("LV5", new LevelStarCondition { totalOrders = 25, maxErrors = 5, minCorrect = 22 });
        levelConditions.Add("LV6", new LevelStarCondition { totalOrders = 30, maxErrors = 6, minCorrect = 27 });


        // 可依需要新增更多關卡
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SetStarRating(15, 5, 0, 0, 300);


    }

    /// <summary>
    /// 設定星星評分
    /// </summary>
    /// <param name="level">關卡數</param>
    /// <param name="totalOrders">完成的總單數</param>
    /// <param name="correctOrders">正確完成的單數</param>
    /// <param name="errorOrders">錯誤的單數</param>
    /// <param name="timeoutOrders">超時的單數</param>
    /// <param name="income">玩家獲得的收入</param>
    public void SetStarRating(int totalOrders, int correctOrders, int errorOrders, int timeoutOrders, int income)
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        CloseAllOtherUIByLayer();
        ui.gameObject.SetActive(true);

        randomModelSpawner.SpawnRandomModels();
        if (!levelConditions.ContainsKey(currentLevel))
        {
            Debug.LogWarning($"找不到第 {currentLevel} 關的星星條件");
            return;
        }

        // 取得當前關卡的星星條件
        LevelStarCondition condition = levelConditions[currentLevel];

        // 計算星星評分
        int stars = 0;


        if (totalOrders >= condition.totalOrders) stars++;
        if (errorOrders <= condition.maxErrors) stars++;
        if (correctOrders >= condition.minCorrect) stars++;

        cmp.text = "完成訂單數:" + correctOrders+"/"+ condition.totalOrders;
        error.text = "錯誤訂單數:" + errorOrders + "/" + condition.maxErrors;
        delay.text = "超時訂單數:" + timeoutOrders + "/" + condition.minCorrect;
        totalcoins.text = "總收入:" + income;
        playerManager.playerData.coins += income;
        // 更新星星顯示
        UpdateStars(stars);
        playerManager.playerData.levelStars[currentLevel] = stars;
        Debug.LogWarning(currentLevel + "+" + stars);

    }

    /// <summary>
    /// 更新星星的顯示
    /// </summary>
    /// <param name="stars">獲得的星星數量</param>
    private void UpdateStars(int stars)
    {
        // 初始化所有星星為未啟用狀態
        starA.sprite = emptyStar;
        starB.sprite = emptyStar;
        starC.sprite = emptyStar;

        // 啟用對應數量的星星
        if (stars >= 1) starA.sprite = filledStar;
        if (stars >= 2) starB.sprite = filledStar;
        if (stars >= 3) starC.sprite = filledStar;
        
    }
}
