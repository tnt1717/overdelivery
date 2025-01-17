using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    // 使用 Application.dataPath 指向遊戲根目錄（在編輯器中是 Assets 目錄，在構建版本中是遊戲根目錄）
    private static string folderPath = Path.Combine(Application.dataPath, "../test_Data");
    private static string filePath = Path.Combine(folderPath, "playerData.json");

    // 保存玩家資料
    public static void SavePlayerData(PlayerData data)
    {
        // 確保資料夾存在
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("SaveSystem: Created folder at " + folderPath);
        }

        // 保存資料到 JSON 文件
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("SaveSystem: Player data saved to " + filePath);
    }

    // 加載玩家資料
    public static PlayerData LoadPlayerData()
    {
        // 如果檔案存在，讀取資料
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("SaveSystem: Loaded player data from " + filePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // 檔案不存在時，建立新的玩家資料
            Debug.LogWarning("SaveSystem: No save file found. Creating new PlayerData.");
            return new PlayerData(); // 返回新的玩家資料
        }
    }
}
