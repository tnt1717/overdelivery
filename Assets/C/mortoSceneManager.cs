using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static mortoSceneManager;

[System.Serializable]
public class mortoSceneManager : MonoBehaviour

{
    [System.Serializable]
    public class Item
    {
        public string itemName;     // 商品名稱
        public int basePrice;       // 升級價格基數
        public Button buyButton;    // 購買按鈕
        public Text titleText;      // 顯示名稱+等級的文本
        public Text buybtnText;
        public Text priceText;      // 顯示價格的文本
        public Sprite normalSprite;  // 普通按鈕圖像（A）
        public Sprite maxLevelSprite; // 滿級按鈕圖像（B）
    }

    public GameObject toolbox;        // 工具箱物件
    public GameObject upgradeMenuUI;  // 升級選單 UI
    public GameObject vehicleMenuUI;  // 車輛選單 UI
    private bool isUpgradeMenuOpen = false;  // 用於追蹤升級選單是否開啟

    public GameObject[] upgradeItems;   // 存儲升級項目 UI 的陣列
    private int currentIndex = 1;       // 當前顯示的升級項目索引

    private PlayerManager playerManager; // 連結 PlayerManager 用於存取 PlayerData

    public Text playercoins;
    
    public Item[] items; // 商品陣列
    public Button buyButton;    // 購買按鈕

    //private void Awake()
    //{
    //    // 在 Awake() 中進行初始化，確保 Carobjects 在 Start() 前就被設置
    //    InitializeItems(); // 初始化商品資料9
    //}

    private void Start()
    {
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

        
        // 確保 UI 狀態在開始時為關閉
        upgradeMenuUI.SetActive(false);
        vehicleMenuUI.SetActive(true);
        InitializeItems(); // 初始化商品資料
        // 綁定每個商品的升級按鈕，傳遞商品物件作為參數
        foreach (var item in items)
        {
            // 使用lambda表達式來傳遞商品參數
            item.buyButton.onClick.AddListener(() => UpgradeItem(item));
        }
    }

    public void UpgradeItem(Item item)
    {
        // 取得當前商品的等級
        int currentLevel = playerManager.playerData.itemLevels.ContainsKey(item.itemName) ? playerManager.playerData.itemLevels[item.itemName] : 0;
        int upgradeCost = item.basePrice * (currentLevel + 1);  // 計算升級價格

        // 檢查玩家金幣是否足夠
        //Debug.LogError(playerManager.playerData.coins);
        
        if (playerManager.playerData.coins >= upgradeCost)
        {
            // 扣除金幣
            playerManager.playerData.coins -= upgradeCost;

            // 升級商品等級
            currentLevel++;

            // 更新玩家資料
            playerManager.playerData.itemLevels[item.itemName] = currentLevel;

            // 保存玩家資料
            SaveSystem.SavePlayerData(playerManager.playerData);

            Debug.Log($"升級成功! {item.itemName} 現在等級為 {currentLevel}");

            // 更新UI顯示
            UpdateItemUI(item, currentLevel);
        }
        else
        {
            Debug.Log("金幣不足，無法升級");
        }
    }

    // 更新商品的UI顯示
    private void UpdateItemUI(Item item, int currentLevel)
    {
        // 顯示名稱和等級
        item.titleText.text = $"{item.itemName} Lv{currentLevel}";

        // 計算升級價格
        int nextUpgradePrice = item.basePrice * (currentLevel + 1);
        item.priceText.text = $"價格: {nextUpgradePrice}";

        // 如果達到等級 3，將按鈕圖像設為「B」，並顯示「已滿級」
        if (currentLevel >= 3)
        {
            item.buyButton.interactable = false; // 禁用按鈕
            item.buyButton.GetComponent<Image>().sprite = item.maxLevelSprite; // 更換為滿級圖像
            item.priceText.text = "已滿";  // 顯示已滿級
            item.buybtnText.text = "已滿級";

        }
        else
        {
            item.buyButton.interactable = true;  // 開啟按鈕
            item.buyButton.GetComponent<Image>().sprite = item.normalSprite;  // 恢復正常圖像
        }
        playercoins.text = playerManager.playerData.coins.ToString();
    }

    private void InitializeItems()
    {
        foreach (var item in items)
        {
            int currentLevel = playerManager.playerData.itemLevels.ContainsKey(item.itemName) ? playerManager.playerData.itemLevels[item.itemName] : 0;
            UpdateItemUI(item, currentLevel);
        }
    }

    private void Update()
    {
        // 檢查是否點擊了滑鼠左鍵
        if (Input.GetMouseButtonDown(0))
        {
            // 檢查滑鼠是否在 UI 上
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // 建立 Ray，從攝影機射出到滑鼠位置
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // 檢查是否射線打到工具箱
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == toolbox)
                {
                    // 切換升級選單 UI 的顯示狀態
                    isUpgradeMenuOpen = !isUpgradeMenuOpen;
                    upgradeMenuUI.SetActive(isUpgradeMenuOpen);
                    vehicleMenuUI.SetActive(!isUpgradeMenuOpen);

                    // 顯示狀態變更訊息
                    Debug.Log(isUpgradeMenuOpen ? "工具箱被點擊，打開升級選單 UI，關閉車輛選單 UI"
                                                : "工具箱被點擊，關閉升級選單 UI，開啟車輛選單 UI");
                    //vehicleButton.gameObject.SetActive(!isUpgradeMenuOpen); // 新增：切換車輛按鈕顯示狀態
                }
            }
        }
    }

    public void OnRightButton()
    {
        // 隱藏當前升級項目
        upgradeItems[currentIndex].SetActive(false);

        // 更新索引並循環回到陣列開頭
        currentIndex = (currentIndex + 1) % upgradeItems.Length;

        // 顯示新的升級項目
        upgradeItems[currentIndex].SetActive(true);
    }

    // 按鈕事件：切換到上一個升級項目
    public void OnLeftButton()
    {
        // 隱藏當前升級項目
        upgradeItems[currentIndex].SetActive(false);

        // 更新索引並循環回到陣列結尾
        currentIndex = (currentIndex - 1 + upgradeItems.Length) % upgradeItems.Length;

        // 顯示新的升級項目
        upgradeItems[currentIndex].SetActive(true);
    }
    // 左按鈕事件，顯示上一個物件
    
}
