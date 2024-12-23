using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStarsManager : MonoBehaviour
{
    public List<Image> levelUIElements; // ���P�� UI �DƬ
    public Sprite unlockedSprite;       // ���i�ĈDƬ
    public Sprite lockedSprite;         // �i���ĈDƬ

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.instance;  // �@ȡ PlayerManager �Č���
        if (playerManager != null)
        {
            UpdateLevelUI();  // ���� UI �Է�ӳ��ҵ��P���M��
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

            // �������ǔ����O���DƬ
            levelUIElements[i].sprite = stars > 0 ? unlockedSprite : lockedSprite;
        }
    }
}
