using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarRatingManager: MonoBehaviour
{
    // ���ǈDƬ
    public Image starA;
    public Image starB;
    public Image starC;


    public Text cmp;
    public Text error;
    public Text delay;
    public Text totalcoins;

    // ���ǵĆ��ÈDƬ�cδ���ÈDƬ
    public Sprite filledStar;  // �������ǈDƬ
    public Sprite emptyStar;   // δ�������ǈD
    private PlayerManager playerManager;

    public GameObject ui;
    private RandomModelSpawner randomModelSpawner;


    // ����ÿһ�P���u�֗l��
    [System.Serializable]
    public class LevelStarCondition
    {
        public int totalOrders;  // ���Δ��l��
        public int maxErrors;    // �e�`���ֵ
        public int minCorrect;   // ���_��Сֵ
    }

    public Dictionary<string, LevelStarCondition> levelConditions = new Dictionary<string, LevelStarCondition>();

    public void CloseAllOtherUIByLayer()
    {
        // �@ȡ��ǰ������ڵ� Layer
        int uiLayer = LayerMask.NameToLayer("UI");

        // ��v���������м�������
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // �z������Ƿ��� UI �ӁK�Ҳ��Ǯ�ǰ���
            if (obj.layer == uiLayer && obj != this.gameObject && obj.activeSelf)
            {
                obj.SetActive(false); // �P�]���� UI
            }
        }
    }
    private void Start()
    {

        ui.gameObject.SetActive(false);

        randomModelSpawner=FindObjectOfType<RandomModelSpawner>();
        GameObject playerSys = GameObject.Find("PlayerSys");
        if (playerSys != null)
        {
            playerManager = playerSys.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("�� 'PlayerSys' ��δ�ҵ� PlayerManager �M����");
                return;
            }
        }
        else
        {
            Debug.LogError("δ�ҵ����Q�� 'PlayerSys' �������");
            return;
        }

        // ��ʼ��ÿһ�P�l��
        levelConditions.Add("LV1", new LevelStarCondition { totalOrders = 1, maxErrors = 0, minCorrect = 1 });
        levelConditions.Add("LV2", new LevelStarCondition { totalOrders = 4, maxErrors = 2, minCorrect = 3 });
        levelConditions.Add("LV3", new LevelStarCondition { totalOrders = 15, maxErrors = 4, minCorrect = 10 });
        levelConditions.Add("LV4", new LevelStarCondition { totalOrders = 20, maxErrors = 5, minCorrect = 15 });
        levelConditions.Add("LV5", new LevelStarCondition { totalOrders = 25, maxErrors = 5, minCorrect = 22 });
        levelConditions.Add("LV6", new LevelStarCondition { totalOrders = 30, maxErrors = 6, minCorrect = 27 });


        // ������Ҫ���������P��
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SetStarRating(15, 5, 0, 0, 300);


    }

    /// <summary>
    /// �O�������u��
    /// </summary>
    /// <param name="level">�P����</param>
    /// <param name="totalOrders">��ɵĿ��Δ�</param>
    /// <param name="correctOrders">���_��ɵĆΔ�</param>
    /// <param name="errorOrders">�e�`�ĆΔ�</param>
    /// <param name="timeoutOrders">���r�ĆΔ�</param>
    /// <param name="income">��ҫ@�õ�����</param>
    public void SetStarRating(int totalOrders, int correctOrders, int errorOrders, int timeoutOrders, int income)
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        CloseAllOtherUIByLayer();
        ui.gameObject.SetActive(true);

        randomModelSpawner.SpawnRandomModels();
        if (!levelConditions.ContainsKey(currentLevel))
        {
            Debug.LogWarning($"�Ҳ����� {currentLevel} �P�����Ǘl��");
            return;
        }

        // ȡ�î�ǰ�P�������Ǘl��
        LevelStarCondition condition = levelConditions[currentLevel];

        // Ӌ�������u��
        int stars = 0;


        if (totalOrders >= condition.totalOrders) stars++;
        if (errorOrders <= condition.maxErrors) stars++;
        if (correctOrders >= condition.minCorrect) stars++;

        cmp.text = "���ӆ�Δ�:" + correctOrders+"/"+ condition.totalOrders;
        error.text = "�e�`ӆ�Δ�:" + errorOrders + "/" + condition.maxErrors;
        delay.text = "���rӆ�Δ�:" + timeoutOrders + "/" + condition.minCorrect;
        totalcoins.text = "������:" + income;
        playerManager.playerData.coins += income;
        // ���������@ʾ
        UpdateStars(stars);
        playerManager.playerData.levelStars[currentLevel] = stars;
        Debug.LogWarning(currentLevel + "+" + stars);

    }

    /// <summary>
    /// �������ǵ��@ʾ
    /// </summary>
    /// <param name="stars">�@�õ����ǔ���</param>
    private void UpdateStars(int stars)
    {
        // ��ʼ���������Ǟ�δ���à�B
        starA.sprite = emptyStar;
        starB.sprite = emptyStar;
        starC.sprite = emptyStar;

        // ���Ì�������������
        if (stars >= 1) starA.sprite = filledStar;
        if (stars >= 2) starB.sprite = filledStar;
        if (stars >= 3) starC.sprite = filledStar;
        
    }
}
