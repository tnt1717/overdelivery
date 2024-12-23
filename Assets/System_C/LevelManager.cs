using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    // �P���l�����Y�ϽY��������ӆ�������e�`�����ơ��r�g����
    public class LevelConditions
    {
        public int requiredOrders;  // �����ɵ�ӆ�Δ�
        public int maxErrors;       // ���S������e�`��
        public int timeLimit;       // �r�g����

        public LevelConditions(int requiredOrders, int maxErrors, int timeLimit)
        {
            this.requiredOrders = requiredOrders;
            this.maxErrors = maxErrors;
            this.timeLimit = timeLimit;
        }
    }

    public Dictionary<string, LevelConditions> levelConditions; // ���������P���ėl��
    public Text orderCountText;      // �@ʾӆ����ɔ�������
    public Text errorRateText;       // �@ʾ�e�`�ʵ�����
    public Text timeLimitText;
    public Text orderCountTextW;      // �@ʾӆ����ɔ�������
    public Text errorRateTextW;       // �@ʾ�e�`�ʵ�����
    public Text timeLimitTextW;       // �@ʾ�r�g���Ƶ�����

    public int totalOrders;
    public int totalErrors;


    //�Y��^

    public Text totalOrdersText;                   // �@ʾ��ӆ�Δ���
    public Text totalErrorsText;                   // �@ʾ���e�`����

    public Text totalEarningsText;                 // �@ʾ�@�ý��~

    private float currentTime; // ��ǰʣ�N�r�g
    private bool isTimerRunning = true; // �Ƿ�Ӌ�r�������\��

    private LevelConditions currentLevelConditions;
    private MoneyManager moneyManager;

    private void Start()
    {
        InitializeLevelConditions(); // ��ʼ���P���l��
        LoadLevel(SceneManager.GetActiveScene().name);  
        moneyManager=FindObjectOfType<MoneyManager>();

    }

    // ��ʼ�������P���l��
    private void InitializeLevelConditions()
    {
        levelConditions = new Dictionary<string, LevelConditions>
        {
            { "LV1", new LevelConditions(1, 0, 100) },
            { "LV2", new LevelConditions(3, 2, 200) },
            { "LV3", new LevelConditions(10, 4, 300) },
            { "LV4", new LevelConditions(15, 5, 300) },
            { "LV5", new LevelConditions(22, 5, 300) },
            { "LV6", new LevelConditions(27, 6, 300) }


        };
    }

    // �d���ض��P���ėl��
    public void LoadLevel(string levelName)
    {
        if (levelConditions.TryGetValue(levelName, out currentLevelConditions))
        {
            Debug.Log($"�d�� {levelName}��ӆ�Δ�������: {currentLevelConditions.requiredOrders}������e�`��: {currentLevelConditions.maxErrors}���r�g����: {currentLevelConditions.timeLimit}��");
            currentTime = currentLevelConditions.timeLimit;
        }
        else
        {
            Debug.LogWarning($"δ�ҵ��P�� {levelName} ���Y�ϡ�");
        }
    }

    private void Update()
    {
        UpdateUI();
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime; // �p�ٕr�g

            // ���r�g���_ 0 �r�|�l�Y��
            if (currentTime <= 0f)
            {
                currentTime = 0f; // ��ֹ�r�g׃��ֵؓ
                Time.timeScale = 0.9f; // ��ͣ�[��
                isTimerRunning = false; // ֹͣӋ�r��
                moneyManager.CalculateFinalEarnings();
            }
        }
    }

    // ���� UI �@ʾ
    private void UpdateUI()
    {

        orderCountText.text = $"ӆ��: {totalOrders}/{currentLevelConditions.requiredOrders}";
        errorRateText.text = $"�e�`: {totalErrors}/{currentLevelConditions.maxErrors}";
        //timeLimitText.text = $": {order_manager.orderTimer.ToString("f0")} / {currentLevelConditions.timeLimit} ��";
        //customerNameText.text = " ";
        timeLimitText.text = $"�r��:{Mathf.RoundToInt(currentTime)}/{currentLevelConditions.timeLimit}";

        orderCountTextW.text = $"ӆ��: {totalOrders}/{currentLevelConditions.requiredOrders}";
        errorRateTextW.text = $"�e�`: {totalErrors}/{currentLevelConditions.maxErrors}";
        timeLimitTextW.text = $"�r��:{Mathf.RoundToInt(currentTime)}/{currentLevelConditions.timeLimit}";
    }

    // �@ʾ�Y��UI
    public void ShowResultUI(string completedOrders, string errors, string timeSpent, int earnings)
    {
;
    }



}
