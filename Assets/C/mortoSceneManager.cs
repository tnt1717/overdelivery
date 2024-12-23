using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static mortoSceneManager;

[System.Serializable]
public class mortoSceneManager : MonoBehaviour

{
    [System.Serializable]
    public class Item
    {
        public string itemName;     // ��Ʒ���Q
        public int basePrice;       // �����r�����
        public Button buyButton;    // ُ�I���o
        public Text titleText;      // �@ʾ���Q+�ȼ����ı�
        public Text buybtnText;
        public Text priceText;      // �@ʾ�r����ı�
        public Sprite normalSprite;  // ��ͨ���o�D��A��
        public Sprite maxLevelSprite; // �M�����o�D��B��
    }

    public GameObject toolbox;        // ���������
    public GameObject upgradeMenuUI;  // �����x�� UI
    public GameObject vehicleMenuUI;  // ܇�v�x�� UI
    private bool isUpgradeMenuOpen = false;  // ���׷ۙ�����x���Ƿ��_��

    public GameObject[] upgradeItems;   // �惦�����Ŀ UI �����
    private int currentIndex = 1;       // ��ǰ�@ʾ�������Ŀ����

    private PlayerManager playerManager; // �B�Y PlayerManager ��춴�ȡ PlayerData

    public Text playercoins;
    
    public Item[] items; // ��Ʒ���
    public Button buyButton;    // ُ�I���o

    //private void Awake()
    //{
    //    // �� Awake() ���M�г�ʼ�����_�� Carobjects �� Start() ǰ�ͱ��O��
    //    InitializeItems(); // ��ʼ����Ʒ�Y��9
    //}

    private void Start()
    {
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

        
        // �_�� UI ��B���_ʼ�r���P�]
        upgradeMenuUI.SetActive(false);
        vehicleMenuUI.SetActive(true);
        InitializeItems(); // ��ʼ����Ʒ�Y��
        // ����ÿ����Ʒ���������o�����f��Ʒ������酢��
        foreach (var item in items)
        {
            // ʹ��lambda���_ʽ����f��Ʒ����
            item.buyButton.onClick.AddListener(() => UpgradeItem(item));
        }
    }

    public void UpgradeItem(Item item)
    {
        // ȡ�î�ǰ��Ʒ�ĵȼ�
        int currentLevel = playerManager.playerData.itemLevels.ContainsKey(item.itemName) ? playerManager.playerData.itemLevels[item.itemName] : 0;
        int upgradeCost = item.basePrice * (currentLevel + 1);  // Ӌ�������r��

        // �z����ҽ����Ƿ����
        //Debug.LogError(playerManager.playerData.coins);
        
        if (playerManager.playerData.coins >= upgradeCost)
        {
            // �۳�����
            playerManager.playerData.coins -= upgradeCost;

            // ������Ʒ�ȼ�
            currentLevel++;

            // ��������Y��
            playerManager.playerData.itemLevels[item.itemName] = currentLevel;

            // ��������Y��
            SaveSystem.SavePlayerData(playerManager.playerData);

            Debug.Log($"�����ɹ�! {item.itemName} �F�ڵȼ��� {currentLevel}");

            // ����UI�@ʾ
            UpdateItemUI(item, currentLevel);
        }
        else
        {
            Debug.Log("���Ų��㣬�o������");
        }
    }

    // ������Ʒ��UI�@ʾ
    private void UpdateItemUI(Item item, int currentLevel)
    {
        // �@ʾ���Q�͵ȼ�
        item.titleText.text = $"{item.itemName} Lv{currentLevel}";

        // Ӌ�������r��
        int nextUpgradePrice = item.basePrice * (currentLevel + 1);
        item.priceText.text = $"�r��: {nextUpgradePrice}";

        // ����_���ȼ� 3�������o�D���O�顸B�����K�@ʾ���ѝM����
        if (currentLevel >= 3)
        {
            item.buyButton.interactable = false; // ���ð��o
            item.buyButton.GetComponent<Image>().sprite = item.maxLevelSprite; // ���Q��M���D��
            item.priceText.text = "�ѝM";  // �@ʾ�ѝM��
            item.buybtnText.text = "�ѝM��";

        }
        else
        {
            item.buyButton.interactable = true;  // �_�����o
            item.buyButton.GetComponent<Image>().sprite = item.normalSprite;  // �֏������D��
        }
        playercoins.text = playerManager.playerData.coins.ToString();
    }

    private void InitializeItems()
    {
        foreach (var item in items)
        {
            int currentLevel = playerManager.playerData.itemLevels.ContainsKey(item.itemName) ? playerManager.playerData.itemLevels[item.itemName] : 0;
            UpdateItemUI(item, currentLevel);
        }
    }

    private void Update()
    {
        // �z���Ƿ��c���˻������I
        if (Input.GetMouseButtonDown(0))
        {
            // �z�黬���Ƿ��� UI ��
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // ���� Ray���ĔzӰ�C���������λ��
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // �z���Ƿ��侀�򵽹�����
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == toolbox)
                {
                    // �ГQ�����x�� UI ���@ʾ��B
                    isUpgradeMenuOpen = !isUpgradeMenuOpen;
                    upgradeMenuUI.SetActive(isUpgradeMenuOpen);
                    vehicleMenuUI.SetActive(!isUpgradeMenuOpen);

                    // �@ʾ��B׃��ӍϢ
                    Debug.Log(isUpgradeMenuOpen ? "�����䱻�c�������_�����x�� UI���P�]܇�v�x�� UI"
                                                : "�����䱻�c�����P�]�����x�� UI���_��܇�v�x�� UI");
                    //vehicleButton.gameObject.SetActive(!isUpgradeMenuOpen); // �������ГQ܇�v���o�@ʾ��B
                }
            }
        }
    }

    public void OnRightButton()
    {
        // �[�خ�ǰ�����Ŀ
        upgradeItems[currentIndex].SetActive(false);

        // ���������Kѭ�h�ص�����_�^
        currentIndex = (currentIndex + 1) % upgradeItems.Length;

        // �@ʾ�µ������Ŀ
        upgradeItems[currentIndex].SetActive(true);
    }

    // ���o�¼����ГQ����һ�������Ŀ
    public void OnLeftButton()
    {
        // �[�خ�ǰ�����Ŀ
        upgradeItems[currentIndex].SetActive(false);

        // ���������Kѭ�h�ص���нYβ
        currentIndex = (currentIndex - 1 + upgradeItems.Length) % upgradeItems.Length;

        // �@ʾ�µ������Ŀ
        upgradeItems[currentIndex].SetActive(true);
    }
    // ���o�¼����@ʾ��һ�����
    
}
