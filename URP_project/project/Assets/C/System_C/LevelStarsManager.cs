using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStarsManager : MonoBehaviour
{
    public List<Image> levelUIElements; // 各關卡 UI 圖片
    public Sprite unlockedSprite;       // 解鎖的圖片
    public Sprite lockedSprite;         // 鎖定的圖片

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.instance;  // 獲取 PlayerManager 的實例
        if (playerManager != null)
        {
            UpdateLevelUI();  // 更新 UI 以反映玩家的關卡進度
        }
        else
        {
            Debug.LogError("LevelStarsManager: PlayerManager instance not found.");
        }
    }

    private void UpdateLevelUI()
    {
        for (int i =1; i < levelUIElements.Count; i++)
        {
            string previousLevelName = $"LV{i}";
            int stars = playerManager.playerData.levelStars.ContainsKey(previousLevelName)
                        ? playerManager.playerData.levelStars[previousLevelName]
                        : 0;

            // 根據星星數量設定圖片
            levelUIElements[i].sprite = stars > 0 ? unlockedSprite : lockedSprite;
        }
    }
}
