using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsUIUpdater : MonoBehaviour
{
    public Sprite noStarSprite; // 沒有星星的圖片
    public Sprite starSprite;   // 有星星的圖片

    private void Start()
    {
        UpdateStarsUI("LV1");
        UpdateStarsUI("LV2");
        UpdateStarsUI("LV3");
        // 若有更多關卡，依次調用
    }

    private void UpdateStarsUI(string level)
    {
        // 確認PlayerManager已加載，取得玩家數據
        PlayerData playerData = PlayerManager.instance.playerData;

        // 取得該關卡的星星數量
        if (playerData.levelStars.TryGetValue(level, out int stars))
        {
            // 迭代該關卡的星星UI物件，命名格式為 "LV1_1", "LV1_2", "LV1_3" 等
            for (int i = 1; i <= 3; i++)
            {
                string starObjectName = level + "_" + i;
                GameObject starObject = GameObject.Find(starObjectName);

                if (starObject != null)
                {
                    Image starImage = starObject.GetComponent<Image>();

                    if (starImage != null)
                    {
                        // 根據星星數替換圖片
                        starImage.sprite = i <= stars ? starSprite : noStarSprite;
                    }
                    else
                    {
                        Debug.LogWarning($"{starObjectName} 沒有 Image 組件。");
                    }
                }
                else
                {
                    Debug.LogWarning($"{starObjectName} 未找到於場景中。");
                }
            }
        }
        else
        {
            Debug.LogWarning($"關卡 {level} 的星星數量不存在於玩家資料中。");
        }
    }
}
