using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static BagManager;

public class BagManager : MonoBehaviour
{
    [System.Serializable]
    public class Bag
    {
        public string contentName; // ���������Q
        public int contentID; // ������ ID
        public GameObject contentUI; // ������� UI���������ֻ���ʾ��
        public Image bagImage; // ���ӵ� Image �M��������ГQ Sprite
        public Image foodPic; // ʳ��DƬ
        public Button bagButton; // ÿ�����ӌ����İ��o
    }

    public List<Bag> bags = new List<Bag>(); // ����������ӵ����
    public Sprite spriteA; // �Ѓ�����r�ĈDƬ
    public Sprite spriteB; // �o������r�ĈDƬ
    private newlist_test newlist_test;
    private BagManager bagManager;

    void Start()
    {
  
        newlist_test = FindObjectOfType<newlist_test>();
        bagManager= FindObjectOfType<BagManager>();
        //if (newlist_test != null) Debug.LogWarning("OK");

        // ��ʼ�����ӵ� Sprite �Ͱ��o�¼�
        foreach (var bag in bags)
        {
            if (bag.bagButton != null)
            {
                bag.bagButton.onClick.RemoveAllListeners();
                bag.bagButton.onClick.AddListener(() => OnBagButtonClick(bag));
            }
        }

        UpdateBagSprites();
    }

    void Update()
    {
        HandleMouseHover();
        UpdateBagSprites();
    }

    private void UpdateBagSprites()
    {
        foreach (var bag in bags)
        {
            if (!string.IsNullOrEmpty(bag.contentName)) // ����Ѓ�����
            {
                bag.bagImage.sprite = spriteA;
                bag.bagButton.interactable = true; // ���ð��o
            }
            else // �]�Ѓ�����
            {
                bag.bagImage.sprite = spriteB;
                bag.bagButton.interactable = false; // ���ð��o
            }
        }
    }

    private void HandleMouseHover()
    {
        foreach (var bag in bags)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(bag.bagImage.rectTransform, Input.mousePosition))
            {
                if (!string.IsNullOrEmpty(bag.contentName)) // ��������Ѓ�����
                {
                    bag.contentUI.SetActive(true); // �@ʾ������� UI
                }
            }
            else
            {
                bag.contentUI.SetActive(false); // �����ڴ����ϣ��P�]������ UI
            }
        }
    }

    /// <summary>
    /// ������Ʒ��������
    /// </summary>
    public void AddItemToBag(string foodName, int orderID)
    {
        foreach (var bag in bags)
        {
            if (string.IsNullOrEmpty(bag.contentName)) // �ҵ���һ���յĴ���
            {
                bag.contentName = foodName;
                bag.contentID = orderID;

                Debug.Log($"������Ʒ: {foodName} (ID: {orderID})");

                // �Lԇ�d��DƬ�YԴ
                Sprite itemSprite = Resources.Load<Sprite>("Order_Pic/" + foodName);
                if (itemSprite != null)
                {
                    bag.foodPic.sprite = itemSprite;
                }

                UpdateBagSprites(); // ���´��ӵ��@ʾ��B
                return;
            }
        }

        Debug.LogWarning("���д��Ӷ��ѝM���o��������Ʒ��");
    }

    public void RemoveItemFromBagByID(int orderID)
    {
        foreach (var bag in bags)
        {
            if (bag.contentID == orderID) // �ҵ������� ID
            {
                bag.contentName = string.Empty; // ��Ճ��������Q
                bag.contentID = 0; // ��Ճ����� ID
                bag.foodPic.sprite = null; // ��ՈDƬ
                bag.bagImage.sprite = spriteB; // ���DƬ���O��o���ݵĈDƬ
                UpdateBagSprites(); // ���´��ӵ��@ʾ��B

                Debug.Log($"���Ƴ� ID �� {orderID} �ă�����");
                return;
            }
        }

        Debug.LogWarning($"�Ҳ��� ID �� {orderID} �ă����");
    }

    /// <summary>
    /// �����Ӱ��o���c���r�|�l���¼�
    /// </summary>
    /// <param name="bag">�����Ĵ���</param>
    private void OnBagButtonClick(Bag bag)
    {
        if (!string.IsNullOrEmpty(bag.contentName))
        {
            Debug.Log($"���´��Ӱ��o�����������Q: {bag.contentName}, ӆ�� ID: {bag.contentID}");
            
            newlist_test.VerifyOrderDelivery(bag.contentID);
            newlist_test.DeletePreviewItem(bag.contentID);
    
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(); // �@ȡ�������������

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == bag.contentID.ToString())
                {
                    Debug.LogWarning($"�h�����: {obj.name}");
                    Destroy(obj); // �h��ԓ���
                }
            }

            newlist_test.UpdateOrderStatus(bag.contentID, "�����_");
            bagManager.RemoveItemFromBagByID(bag.contentID);


        }
        else
        {
            Debug.LogWarning("�����ǿյģ��o���@ʾ�����");
        }
    }
}
