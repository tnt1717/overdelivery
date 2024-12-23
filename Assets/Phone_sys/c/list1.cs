using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class list1 : MonoBehaviour
{
    public GameObject contentPanel; // 內容面板
    public GameObject itemPrefab;   // 預製體

    private Dictionary<string, AchievementData> achievementDatabase;
    private HashSet<string> instantiatedAchievements = new HashSet<string>(); // 已實例化成就的鍵
    private PlayerManager playerManager;

    void Awake()
    {
        // 獲取 PlayerManager 物件與資料
        GameObject playerSys = GameObject.Find("PlayerSys");
        if (playerSys != null)
        {
            playerManager = playerSys.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("在 'PlayerSys' 上未找到 PlayerManager 組件！");
            }
        }
        else
        {
            Debug.LogError("未找到名稱為 'PlayerSys' 的物件！");
        }

        InitializeDatabase();
    }

    void Update()
    {
        if (playerManager != null && playerManager.playerData != null)
        {
            UpdateAchievements(playerManager.playerData.achievements);
        }
    }

    private void InitializeDatabase()
    {
        // 建立成就資料庫，從 Resources 加載圖片
        achievementDatabase = new Dictionary<string, AchievementData>
        {
            { "Achievement1", new AchievementData("送過頭", "成功完成第一單", LoadSprite("UI_Phone_Achievement_01")) },
            { "Achievement2", new AchievementData("食安問題", "在食物新鮮度為0時把食物交給客戶", LoadSprite("UI_Phone_Achievement_02")) },
            { "Achievement3", new AchievementData("菜雞", "在1-1僅拿到1顆星", LoadSprite("UI_Phone_Achievement_03")) },
            { "Achievement4", new AchievementData("新手駕駛", "撞到車子或行人", LoadSprite("UI_Phone_Achievement_04")) },
            { "Achievement5", new AchievementData("虔誠的信徒", "去神社參拜", LoadSprite("UI_Phone_Achievement_05")) }
        };
    }

    private Sprite LoadSprite(string imageName)
    {
        // 從 Resources/Sprites/Achievements 加載圖片
        return Resources.Load<Sprite>($"ach_icon/{imageName}");
    }

    private void UpdateAchievements(Dictionary<string, bool> achievements)
    {
        foreach (var achievement in achievements)
        {
            // 如果成就已解鎖且尚未實例化
            if (achievement.Value && !instantiatedAchievements.Contains(achievement.Key))
            {
                if (achievementDatabase.TryGetValue(achievement.Key, out AchievementData data))
                {
                    GameObject newItem = Instantiate(itemPrefab, contentPanel.transform);

                    // 設置標題
                    Text titleText = newItem.transform.Find("title").GetComponent<Text>();
                    if (titleText != null) titleText.text = data.title;

                    // 設置描述
                    Text descriptionText = newItem.transform.Find("text").GetComponent<Text>();
                    if (descriptionText != null) descriptionText.text = data.text;

                    // 設置圖片
                    Image iconImage = newItem.transform.Find("image").GetComponent<Image>();
                    if (iconImage != null && data.image != null)
                    {
                        iconImage.sprite = data.image;
                    }

                    // 添加到已實例化集合
                    instantiatedAchievements.Add(achievement.Key);
                }
            }
        }
    }

    // 成就資料結構
    private class AchievementData
    {
        public string title;   // 成就標題
        public string text;    // 成就描述
        public Sprite image;   // 成就圖片

        public AchievementData(string title, string text, Sprite image)
        {
            this.title = title;
            this.text = text;
            this.image = image;
        }
    }
}
