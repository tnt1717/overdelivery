using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//GameObject playerSys = GameObject.Find("PlayerSys");
//if (playerSys != null)
//{
//    playerManager = playerSys.GetComponent<PlayerManager>();
//    if (playerManager == null)
//    {
//        Debug.LogError("在 'PlayerSys' 上未找到 PlayerManager 組件！");
//        return;
//    }
//}
//else
//{
//    Debug.LogError("未找到名稱為 'PlayerSys' 的物件！");
//    return;
//}


[System.Serializable]
public class PlayerData 
{
    public int coins=1000;
    public Dictionary<string, bool> achievements; // 成就名稱和解鎖狀態

    // 服裝資料
    public Dictionary<string, bool> outfits; // 服裝名稱和擁有狀態
    public List<string> unlockedClothings; // 已解鎖服裝名稱


    // 表情資料
    public Dictionary<string, bool> expressions; // 表情名稱和解鎖狀態
    public List<string> unlockedExpressions; // 已解鎖表情名稱


    public Dictionary<string, int> levelStars; // 關卡名稱和星星數量
    public Dictionary<string, int> itemLevels; // 商品名稱和等級

    public bool isCharacterCreated = false;
    public string Playermodle,PlayerName;

    //public Dictionary<string, string> vehicleStatus = new Dictionary<string, string>(); // 車輛狀態

    public Dictionary<string, bool> vehicleStates = new Dictionary<string, bool>(); 
    public string currentVehicle; //使用中的車輛
    public List<int> unlockedVehicles;         // 已解鎖車輛索引
    public List<int> unlockedTextures;         



    public string currentClothing; // 當前使用中的服裝名稱
    //缺少已解鎖表情索引
    //缺少已解鎖服裝索引
    public string currentExpression; // 當前使用中的表情名稱

    public float Volume;
    public float MusicVolume;

    public PlayerData()
    {
        coins = 1000;
        Volume = 0.5f;
        MusicVolume = 0.5f;
        achievements = new Dictionary<string, bool>
        {
            { "Achievement1", false },
            { "Achievement2", false},
            { "Achievement3", false },
            { "Achievement4", false },
            { "Achievement5", false },
            // 可以繼續添加其他成就...
        };

        outfits = new Dictionary<string, bool>
        {
            { "a", false },
            { "b", false },
            // 可以繼續添加其他服裝...
        };
        unlockedClothings = new List<string>();
        currentClothing = null;
        // 初始化表情
        expressions = new Dictionary<string, bool>
        {
            { "Expression1", false },
            { "Expression2", false },
        };
        unlockedExpressions = new List<string>();
        currentExpression = null;


        levelStars = new Dictionary<string, int>
        {
            { "LV1", 1},
            { "LV2", 0},
            { "LV3", 0 },
            { "LV4", 0},
            { "LV5",0},
            { "LV6",0}



        };
        itemLevels = new Dictionary<string, int>();
        { 
        
        
        };

        Debug.Log("PlayerData initialized with default values.");
        itemLevels = new Dictionary<string, int>();
        unlockedVehicles = new List<int>();
        //currentVehicle = -1; // 預設為無車輛
    }

}
