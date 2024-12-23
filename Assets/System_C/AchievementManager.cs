using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private void Update()
    {
        

    }
    public static AchievementManager Instance { get; private set; } // ����ģʽ

    private void Awake()
    {
        // �_��ֻ��һ�� AchievementManager ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // ����������ڈ����ГQ��
    }

    /// <summary>
    /// ���i�ɾ�1����ɵ�һ�P
    /// </summary>
    //public void UnlockAchievement0()
    //{
    //    UnlockAchievement("Achievement0");
    //    AchievementUIManager.Instance.ShowAchievement("Achievement0");
    //}
    /// <summary>
    /// ���i�ɾ͵�ͨ�÷���
    /// </summary>
    /// <param name="achievementKey">�ɾ͵��Iֵ</param>
    public void UnlockAchievement(string achievementKey)
    {
        var playerData = PlayerManager.instance.playerData;

        // �_���ɾ��I�������Ҕ�����
        if (playerData.achievements.ContainsKey(achievementKey))
        {
            // �����δ���i���t���i�K����
            if (!playerData.achievements[achievementKey])
            {
                playerData.achievements[achievementKey] = true;
                //PlayerManager.instance.SavePlayerData();
                Debug.LogWarning("Achievement" + achievementKey);
                AchievementUIManager.Instance.ShowAchievement(achievementKey);
                Debug.Log($"�ɾͽ��i: {achievementKey}");
            }
            else
            {
                Debug.Log($"�ɾ� {achievementKey} �ѽ��i��");
            }
        }
        else
        {
            Debug.LogWarning($"�ɾ��I {achievementKey} ���������Ҕ����У�");
        }
    }
}
