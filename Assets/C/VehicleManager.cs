using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleManager : MonoBehaviour
{
    [System.Serializable]
    public class Vehicle
    {
        public string vehicleName;  // 車輛名稱
        public int price;           // 車輛價格
        public GameObject vehicleObject; // 車輛對應的物件
    }
    private PlayerManager playerManager; // 連結 PlayerManager 用於存取 PlayerData

    public Vehicle[] vehicles;      // 所有車輛的陣列
    public Text buttonText;         // 按鈕上的文字
    public Button actionButton;     // 執行購買或切換的按鈕
    public Text playerCoinsText;    // 顯示玩家金幣的文本
    private int currentIndex = 0;   // 當前選中的車輛索引
    //private PlayerData playerData;

    //public PlayerData playerData;   // 玩家數據物件
    private string currentVehicleKey = "currentVehicle"; // 使用中的車輛存儲鍵

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

        
        LoadPlayerData(); // 初始化玩家數據
        SavePlayerData(); // 確保默認狀態被保存
        UpdateVehicleUI(); // 更新UI
    }

    private void LoadPlayerData()
    {
        // 確保每輛車都有狀態
        foreach (var vehicle in vehicles)
        {
            if (!playerManager.playerData.vehicleStates.ContainsKey(vehicle.vehicleName))
            {
                playerManager.playerData.vehicleStates[vehicle.vehicleName] = false; // 默認為未擁有
            }
        }

        // 如果未設置當前車輛，或第一輛車狀態不正確，修正為第一輛車
        var defaultVehicle = vehicles[0]; // 預設為第一輛車
        if (string.IsNullOrEmpty(playerManager.playerData.currentVehicle) ||
            !playerManager.playerData.vehicleStates[defaultVehicle.vehicleName])
        {
            playerManager.playerData.currentVehicle = defaultVehicle.vehicleName;
            playerManager.playerData.vehicleStates[defaultVehicle.vehicleName] = true; // 設置為已擁有

            Debug.Log($"修正第一輛車 {defaultVehicle.vehicleName} 為預設車輛且狀態為已擁有");
        }
    }

    public void OnLeftButton()
    {
        currentIndex = (currentIndex - 1 + vehicles.Length) % vehicles.Length;
        Debug.Log($"切換到左邊車輛，索引：{currentIndex}");
        UpdateVehicleUI();
    }

    public void OnRightButton()
    {
        currentIndex = (currentIndex + 1) % vehicles.Length;
        Debug.Log($"切換到右邊車輛，索引：{currentIndex}");
        UpdateVehicleUI();
    }

    private void UpdateVehicleUI()
    {
        // 隱藏所有車輛物件
        foreach (var vehicle in vehicles)
        {
            vehicle.vehicleObject.SetActive(false);
        }

        // 顯示當前選中車輛
        vehicles[currentIndex].vehicleObject.SetActive(true);

        // 更新按鈕狀態
        var currentVehicle = vehicles[currentIndex];
        bool isOwned = playerManager.playerData.vehicleStates[currentVehicle.vehicleName];

        Debug.Log($"當前選中車輛：{currentVehicle.vehicleName}，是否已擁有：{isOwned}");

        if (isOwned)
        {
            if (playerManager.playerData.currentVehicle == currentVehicle.vehicleName)
            {
                buttonText.text = "使用中";
                actionButton.interactable = false;
                Debug.Log($"{currentVehicle.vehicleName} 正在使用中");
            }
            else
            {
                buttonText.text = "未使用";
                actionButton.interactable = true;
                Debug.Log($"{currentVehicle.vehicleName} 已擁有但未使用");
            }
        }
        else
        {
            buttonText.text = $"價格: {currentVehicle.price}";
            actionButton.interactable = playerManager.playerData.coins >= currentVehicle.price;
            Debug.Log($"{currentVehicle.vehicleName} 未擁有，價格為 {currentVehicle.price}");
        }

        // 更新玩家金幣顯示
        playerCoinsText.text = $"金幣: {playerManager.playerData.coins}";
    }
    public void OnActionButton()
    {
        var currentVehicle = vehicles[currentIndex];
        bool isOwned = playerManager.playerData.vehicleStates[currentVehicle.vehicleName];

        if (isOwned)
        {
            // 切換為當前車輛
            playerManager.playerData.currentVehicle = currentVehicle.vehicleName;
            Debug.Log($"切換車輛為：{currentVehicle.vehicleName}");

        }
        else
        {
            //int currentCoins = playerManager.playerData.coins;
            // 購買車輛
            if (playerManager.playerData.coins >= currentVehicle.price)
            {
                playerManager.playerData.coins -= currentVehicle.price;
                playerManager.playerData.vehicleStates[currentVehicle.vehicleName] = true;
                playerManager.playerData.currentVehicle = currentVehicle.vehicleName;

                Debug.Log($"購買車輛 {currentVehicle.vehicleName} 成功，剩餘金幣：{playerManager.playerData.coins}");
            }
            else
            {
                Debug.Log($"購買失敗，金幣不足！需要：{currentVehicle.price}，當前擁有：{playerManager.playerData.coins}");
                return;
            }
        }

        // 更新UI並保存數據
        UpdateVehicleUI();
        SavePlayerData();
    }

    private void SavePlayerData()
    {
        // 實現數據保存邏輯，例如使用PlayerPrefs或其他保存方式
        PlayerPrefs.SetInt("playerCoins", playerManager.playerData.coins);
        PlayerPrefs.SetString(currentVehicleKey, playerManager.playerData.currentVehicle);

        foreach (var state in playerManager.playerData.vehicleStates)
        {
            PlayerPrefs.SetInt(state.Key, state.Value ? 1 : 0);
        }

        PlayerPrefs.Save();
        Debug.Log("玩家數據已保存");
    }

    private void LoadPlayerDataFromPrefs()
    {
        playerManager.playerData.coins = PlayerPrefs.GetInt("playerCoins", 1000);
        playerManager.playerData.currentVehicle = PlayerPrefs.GetString(currentVehicleKey, vehicles[0].vehicleName);

        foreach (var vehicle in vehicles)
        {
            bool isOwned = PlayerPrefs.GetInt(vehicle.vehicleName, 0) == 1;
            playerManager.playerData.vehicleStates[vehicle.vehicleName] = isOwned;
        }

        // 確保第一輛車為已擁有
        var defaultVehicle = vehicles[0];
        if (!playerManager.playerData.vehicleStates[defaultVehicle.vehicleName])
        {
            playerManager.playerData.vehicleStates[defaultVehicle.vehicleName] = true;
            playerManager.playerData.currentVehicle = defaultVehicle.vehicleName;
            Debug.Log($"加載數據後修正第一輛車 {defaultVehicle.vehicleName} 為已擁有");
        }
    }
}



