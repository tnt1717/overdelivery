using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    // �O����n·�������Йn����Ŀ��µ� "test_Data" �Y�ϊA
    private static string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "test_Data");
    private static string filePath = Path.Combine(folderPath, "playerData.json");

    // ��������Y��
    public static void SavePlayerData(PlayerData data)
    {
        // �_���Y�ϊA����
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("SaveSystem: Created folder at " + folderPath);
        }

        // �����Y�ϵ� JSON �ļ�
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("SaveSystem: Player data saved to " + filePath);
       
    }

    // ���d����Y��
    public static PlayerData LoadPlayerData()
    {
        // ����n�����ڣ��xȡ�Y��
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("SaveSystem: Loaded player data from " + filePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // �n�������ڕr�������µ�����Y��
            Debug.LogWarning("SaveSystem: No save file found. Creating new PlayerData.");
            return new PlayerData(); // �����µ�����Y��
        }
    }
}