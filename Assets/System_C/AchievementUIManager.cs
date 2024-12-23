using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUIManager : MonoBehaviour
{
    // 單例模式
    public static AchievementUIManager Instance { get; private set; }

    [System.Serializable]
    public class AchievementData
    {
        public string achievementName;  // 成就名稱
        public string achtitle;
        public Sprite achievementUI;   // 成就對應的圖片
        public string achievementText; // 成就描述文本
    }

    public List<AchievementData> achievements; // 成就數據列表

    public Canvas achievementCanvas; // 成就畫布
    public Image achievementImage;   // 圖片組件
    public Text achievementTitle;    // 成就名稱文本
    public Text achievementDescription; // 成就描述文本

    public float displayTime = 2f; // 顯示時長

    private Coroutine currentDisplayRoutine;

    private void Awake()
    {
        // 確保只有一個實例存在
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 如果已經有實例存在，銷毀重複的物件
            return;
        }

        DontDestroyOnLoad(gameObject); // 跨場景保留
    }

    private void Start()
    {
        achievementCanvas.gameObject.SetActive(false); // 初始化隱藏畫布
    }

    /// <summary>
    /// 顯示成就 UI
    /// </summary>
    /// <param name="achievementKey">成就鍵值</param>
    public void ShowAchievement(string achievementKey)
    {
        // 根據成就鍵值找到對應的數據
        AchievementData data = achievements.Find(a => a.achievementName == achievementKey);

        if (data != null)
        {
            // 更新 UI 元素
            achievementTitle.text = data.achtitle;
            achievementDescription.text = data.achievementText;
            achievementImage.sprite = data.achievementUI;

            // 如果已有顯示，取消並重啟
            if (currentDisplayRoutine != null)
            {
                StopCoroutine(currentDisplayRoutine);
            }

            // 顯示成就並自動隱藏
            currentDisplayRoutine = StartCoroutine(DisplayAchievementUI());
        }
        else
        {
            Debug.LogWarning($"未找到成就數據: {achievementKey}");
        }
    }

    /// <summary>
    /// 顯示成就畫布並在設定時間後隱藏
    /// </summary>
    private IEnumerator DisplayAchievementUI()
    {
        achievementCanvas.gameObject.SetActive(true); // 顯示畫布
        yield return new WaitForSeconds(displayTime); // 等待
        achievementCanvas.gameObject.SetActive(false); // 隱藏畫布
        currentDisplayRoutine = null; // 清空當前協程
    }
}
