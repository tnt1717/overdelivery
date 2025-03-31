using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private void Update()
    {
        

    }
    public static AchievementManager Instance { get; private set; } // 單例模式

    private void Awake()
    {
        // 確保只有一個 AchievementManager 存在
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 保留此物件在場景切換中
    }

    /// <summary>
    /// 解鎖成就1：完成第一關
    /// </summary>
    //public void UnlockAchievement0()
    //{
    //    UnlockAchievement("Achievement0");
    //    AchievementUIManager.Instance.ShowAchievement("Achievement0");
    //}
    /// <summary>
    /// 解鎖成就的通用方法
    /// </summary>
    /// <param name="achievementKey">成就的鍵值</param>
    public void UnlockAchievement(string achievementKey)
    {
        var playerData = PlayerManager.instance.playerData;

        // 確保成就鍵存在於玩家數據中
        if (playerData.achievements.ContainsKey(achievementKey))
        {
            // 如果尚未解鎖，則解鎖並保存
            if (!playerData.achievements[achievementKey])
            {
                playerData.achievements[achievementKey] = true;
                //PlayerManager.instance.SavePlayerData();
                Debug.LogWarning("Achievement" + achievementKey);
                AchievementUIManager.Instance.ShowAchievement(achievementKey);
                Debug.Log($"成就解鎖: {achievementKey}");
            }
            else
            {
                Debug.Log($"成就 {achievementKey} 已解鎖。");
            }
        }
        else
        {
            Debug.LogWarning($"成就鍵 {achievementKey} 不存在於玩家數據中！");
        }
    }
}
