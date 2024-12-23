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
        public string contentName; // 內容物名稱
        public int contentID; // 內容物 ID
        public GameObject contentUI; // 內容物的 UI（例如文字或提示框）
        public Image bagImage; // 袋子的 Image 組件，用於切換 Sprite
        public Image foodPic; // 食物圖片
        public Button bagButton; // 每個袋子對應的按鈕
    }

    public List<Bag> bags = new List<Bag>(); // 存放六個袋子的陣列
    public Sprite spriteA; // 有內容物時的圖片
    public Sprite spriteB; // 無內容物時的圖片
    private newlist_test newlist_test;
    private BagManager bagManager;

    void Start()
    {
  
        newlist_test = FindObjectOfType<newlist_test>();
        bagManager= FindObjectOfType<BagManager>();
        //if (newlist_test != null) Debug.LogWarning("OK");

        // 初始化袋子的 Sprite 和按鈕事件
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
            if (!string.IsNullOrEmpty(bag.contentName)) // 如果有內容物
            {
                bag.bagImage.sprite = spriteA;
                bag.bagButton.interactable = true; // 啟用按鈕
            }
            else // 沒有內容物
            {
                bag.bagImage.sprite = spriteB;
                bag.bagButton.interactable = false; // 禁用按鈕
            }
        }
    }

    private void HandleMouseHover()
    {
        foreach (var bag in bags)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(bag.bagImage.rectTransform, Input.mousePosition))
            {
                if (!string.IsNullOrEmpty(bag.contentName)) // 如果袋子有內容物
                {
                    bag.contentUI.SetActive(true); // 顯示內容物的 UI
                }
            }
            else
            {
                bag.contentUI.SetActive(false); // 滑鼠不在袋子上，關閉內容物 UI
            }
        }
    }

    /// <summary>
    /// 新增物品到袋子中
    /// </summary>
    public void AddItemToBag(string foodName, int orderID)
    {
        foreach (var bag in bags)
        {
            if (string.IsNullOrEmpty(bag.contentName)) // 找到第一個空的袋子
            {
                bag.contentName = foodName;
                bag.contentID = orderID;

                Debug.Log($"新增物品: {foodName} (ID: {orderID})");

                // 嘗試載入圖片資源
                Sprite itemSprite = Resources.Load<Sprite>("Order_Pic/" + foodName);
                if (itemSprite != null)
                {
                    bag.foodPic.sprite = itemSprite;
                }

                UpdateBagSprites(); // 更新袋子的顯示狀態
                return;
            }
        }

        Debug.LogWarning("所有袋子都已滿，無法新增物品！");
    }

    public void RemoveItemFromBagByID(int orderID)
    {
        foreach (var bag in bags)
        {
            if (bag.contentID == orderID) // 找到對應的 ID
            {
                bag.contentName = string.Empty; // 清空內容物名稱
                bag.contentID = 0; // 清空內容物 ID
                bag.foodPic.sprite = null; // 清空圖片
                bag.bagImage.sprite = spriteB; // 將圖片重設為無內容的圖片
                UpdateBagSprites(); // 更新袋子的顯示狀態

                Debug.Log($"已移除 ID 為 {orderID} 的內容物");
                return;
            }
        }

        Debug.LogWarning($"找不到 ID 為 {orderID} 的內容物！");
    }

    /// <summary>
    /// 當袋子按鈕被點擊時觸發的事件
    /// </summary>
    /// <param name="bag">對應的袋子</param>
    private void OnBagButtonClick(Bag bag)
    {
        if (!string.IsNullOrEmpty(bag.contentName))
        {
            Debug.Log($"按下袋子按鈕，內容物名稱: {bag.contentName}, 訂單 ID: {bag.contentID}");
            
            newlist_test.VerifyOrderDelivery(bag.contentID);
            newlist_test.DeletePreviewItem(bag.contentID);
    
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(); // 獲取場景中所有物件

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == bag.contentID.ToString())
                {
                    Debug.LogWarning($"刪除物件: {obj.name}");
                    Destroy(obj); // 刪除該物件
                }
            }

            newlist_test.UpdateOrderStatus(bag.contentID, "已送達");
            bagManager.RemoveItemFromBagByID(bag.contentID);


        }
        else
        {
            Debug.LogWarning("袋子是空的，無法顯示內容物！");
        }
    }
}
