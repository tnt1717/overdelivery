using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUIManager : MonoBehaviour
{
    // ����ģʽ
    public static AchievementUIManager Instance { get; private set; }

    [System.Serializable]
    public class AchievementData
    {
        public string achievementName;  // �ɾ����Q
        public string achtitle;
        public Sprite achievementUI;   // �ɾ͌����ĈDƬ
        public string achievementText; // �ɾ������ı�
    }

    public List<AchievementData> achievements; // �ɾ͔����б�

    public Canvas achievementCanvas; // �ɾͮ���
    public Image achievementImage;   // �DƬ�M��
    public Text achievementTitle;    // �ɾ����Q�ı�
    public Text achievementDescription; // �ɾ������ı�

    public float displayTime = 2f; // �@ʾ�r�L

    private Coroutine currentDisplayRoutine;

    private void Awake()
    {
        // �_��ֻ��һ����������
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ����ѽ��Ќ������ڣ��N�����}�����
            return;
        }

        DontDestroyOnLoad(gameObject); // ���������
    }

    private void Start()
    {
        achievementCanvas.gameObject.SetActive(false); // ��ʼ���[�خ���
    }

    /// <summary>
    /// �@ʾ�ɾ� UI
    /// </summary>
    /// <param name="achievementKey">�ɾ��Iֵ</param>
    public void ShowAchievement(string achievementKey)
    {
        // �����ɾ��Iֵ�ҵ������Ĕ���
        AchievementData data = achievements.Find(a => a.achievementName == achievementKey);

        if (data != null)
        {
            // ���� UI Ԫ��
            achievementTitle.text = data.achtitle;
            achievementDescription.text = data.achievementText;
            achievementImage.sprite = data.achievementUI;

            // ��������@ʾ��ȡ���K�؆�
            if (currentDisplayRoutine != null)
            {
                StopCoroutine(currentDisplayRoutine);
            }

            // �@ʾ�ɾ́K�Ԅ��[��
            currentDisplayRoutine = StartCoroutine(DisplayAchievementUI());
        }
        else
        {
            Debug.LogWarning($"δ�ҵ��ɾ͔���: {achievementKey}");
        }
    }

    /// <summary>
    /// �@ʾ�ɾͮ����K���O���r�g���[��
    /// </summary>
    private IEnumerator DisplayAchievementUI()
    {
        achievementCanvas.gameObject.SetActive(true); // �@ʾ����
        yield return new WaitForSeconds(displayTime); // �ȴ�
        achievementCanvas.gameObject.SetActive(false); // �[�خ���
        currentDisplayRoutine = null; // ��ծ�ǰ�f��
    }
}
