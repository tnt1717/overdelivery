using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class list1 : MonoBehaviour
{
    public GameObject contentPanel; // �������
    public GameObject itemPrefab;   // �A�u�w

    private Dictionary<string, AchievementData> achievementDatabase;
    private HashSet<string> instantiatedAchievements = new HashSet<string>(); // �ь������ɾ͵��I
    private PlayerManager playerManager;

    void Awake()
    {
        // �@ȡ PlayerManager ����c�Y��
        GameObject playerSys = GameObject.Find("PlayerSys");
        if (playerSys != null)
        {
            playerManager = playerSys.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("�� 'PlayerSys' ��δ�ҵ� PlayerManager �M����");
            }
        }
        else
        {
            Debug.LogError("δ�ҵ����Q�� 'PlayerSys' �������");
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
        // �����ɾ��Y�ώ죬�� Resources ���d�DƬ
        achievementDatabase = new Dictionary<string, AchievementData>
        {
            { "Achievement1", new AchievementData("���^�^", "�ɹ���ɵ�һ��", LoadSprite("UI_Phone_Achievement_01")) },
            { "Achievement2", new AchievementData("ʳ�����}", "��ʳ�����r�Ȟ�0�r��ʳ�ｻ�o�͑�", LoadSprite("UI_Phone_Achievement_02")) },
            { "Achievement3", new AchievementData("���u", "��1-1�H�õ�1�w��", LoadSprite("UI_Phone_Achievement_03")) },
            { "Achievement4", new AchievementData("�����{�", "ײ��܇�ӻ�����", LoadSprite("UI_Phone_Achievement_04")) },
            { "Achievement5", new AchievementData("��\����ͽ", "ȥ���煢��", LoadSprite("UI_Phone_Achievement_05")) }
        };
    }

    private Sprite LoadSprite(string imageName)
    {
        // �� Resources/Sprites/Achievements ���d�DƬ
        return Resources.Load<Sprite>($"ach_icon/{imageName}");
    }

    private void UpdateAchievements(Dictionary<string, bool> achievements)
    {
        foreach (var achievement in achievements)
        {
            // ����ɾ��ѽ��i����δ������
            if (achievement.Value && !instantiatedAchievements.Contains(achievement.Key))
            {
                if (achievementDatabase.TryGetValue(achievement.Key, out AchievementData data))
                {
                    GameObject newItem = Instantiate(itemPrefab, contentPanel.transform);

                    // �O�Ø��}
                    Text titleText = newItem.transform.Find("title").GetComponent<Text>();
                    if (titleText != null) titleText.text = data.title;

                    // �O������
                    Text descriptionText = newItem.transform.Find("text").GetComponent<Text>();
                    if (descriptionText != null) descriptionText.text = data.text;

                    // �O�ÈDƬ
                    Image iconImage = newItem.transform.Find("image").GetComponent<Image>();
                    if (iconImage != null && data.image != null)
                    {
                        iconImage.sprite = data.image;
                    }

                    // ��ӵ��ь���������
                    instantiatedAchievements.Add(achievement.Key);
                }
            }
        }
    }

    // �ɾ��Y�ϽY��
    private class AchievementData
    {
        public string title;   // �ɾ͘��}
        public string text;    // �ɾ�����
        public Sprite image;   // �ɾ͈DƬ

        public AchievementData(string title, string text, Sprite image)
        {
            this.title = title;
            this.text = text;
            this.image = image;
        }
    }
}
