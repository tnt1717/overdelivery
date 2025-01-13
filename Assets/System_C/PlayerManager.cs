using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public PlayerData playerData;

    public void Update()
    {
        if (Input.GetKey(KeyCode.F3)) playerData.coins += 5000;
        if (Input.GetKey(KeyCode.F4))
        {
            playerData.levelStars["LV1"] = 3;
            playerData.levelStars["LV2"] = 3;

            playerData.levelStars["LV3"] = 3;

            playerData.levelStars["LV4"] = 3;

            playerData.levelStars["LV5"] = 3;
            playerData.levelStars["LV6"] = 3;


        }

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            playerData = SaveSystem.LoadPlayerData();
            Debug.Log("PlayerManager: Loaded player data.");
            Debug.Log("載入音量:"+playerData.Volume);
            Debug.Log("載入音效:" + playerData.MusicVolume);



        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SavePlayerData(playerData);
        Debug.Log("PlayerManager: Saved player data on application quit.");
        Debug.Log("存入音量:" + playerData.Volume);
        Debug.Log("存入音效:" + playerData.MusicVolume);

    }

    public void UnlockAchievement(string achievementName)
    {
        if (playerData.achievements.ContainsKey(achievementName))
        {
            playerData.achievements[achievementName] = true;
            AchievementUIManager.Instance.ShowAchievement(achievementName);
            SaveSystem.SavePlayerData(playerData);
            Debug.Log($"玩家 {achievementName} unlocked and saved.");
        }
        else
        {
            Debug.LogWarning($"Achievement {achievementName} does not exist.");
        }
    }

    public void UnlockOutfit(string outfitName)
    {
        if (playerData.outfits.ContainsKey(outfitName))
        {
            playerData.outfits[outfitName] = true;
            SaveSystem.SavePlayerData(playerData);
            Debug.Log($"Outfit {outfitName} unlocked and saved.");
        }
        else
        {
            Debug.LogWarning($"Outfit {outfitName} does not exist.");
        }
    }
}
