using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newlist_test : MonoBehaviour
{
    private MoneyManager moneyManager;
    public GameObject contentPanel1;
    public GameObject contentPanel2;

    public GameObject contentPanel3;  // 新的紀錄面板

    public GameObject contentPanel4; // 預覽進行中訂單的容器
    public GameObject itemPrefab;
    public OrderController orderController;

    public GameObject previewPrefab; // 預覽訂單的預製體
    public Text ongoingOrderCountText; // 用於顯示進行中訂單數量的文本

    private BagManager bagManager;
    private string foodimagename;

    public Dictionary<string, Sprite> foodSprites = new Dictionary<string, Sprite>(); // 食物圖片對應表
    public Dictionary<string, Sprite> customerSprites = new Dictionary<string, Sprite>(); // 顧客圖片對應表
    bool isc = false;


    static public bool isEndPoint = false;
    public List<GameObject> itemObjects1 = new List<GameObject>();
    public List<GameObject> itemObjects2 = new List<GameObject>();
    public List<GameObject> itemObjects3 = new List<GameObject>();  // 存放紀錄項目
    public List<GameObject> itemObjects4 = new List<GameObject>(); // 預覽進行中的訂單
    private Dictionary<int, GameObject> previewItems = new Dictionary<int, GameObject>();
    private Dictionary<int, float> orderTimes = new Dictionary<int, float>(); //訂單時間字典
    private int nextOrderId = 1;  // 用來生成唯一 ID
    int currentOrderId;
    private Text orderTimeLimitText;

    private LevelManager levelManager;
    private Dictionary<int, string> orderStatuses = new Dictionary<int, string>(); //记录每个订单的状态

    private Dictionary<int, string> orderCustomerNames = new Dictionary<int, string>(); // 字典來存儲訂單 ID 與顧客名稱

    public class OrderItem : MonoBehaviour
    {
        public int OrderId { get; private set; }

        public void Initialize(float order_time, string foodName, string cusName, string shopName, int orderId)
        {
            // 初始化數據
            OrderId = orderId;
            // 其他初始化邏輯
        }
    }

    private Dictionary<string, Dictionary<string, string>> restaurantFoods = new Dictionary<string, Dictionary<string, string>>
    {
        { "壽司店", new Dictionary<string, string>
            {
                { "綜合壽司", "JP_Sushi_01" },
                { "綜合生魚片", "JP_Sushi_02" },
                { "鮪魚壽司", "JP_Sushi_03" },
                { "鮭魚壽司", "JP_Sushi_04" },
                { "玉子燒壽司", "JP_Sushi_05" },
                { "鮭魚卵手卷", "JP_Sushi_06" },
                { "豆皮壽司", "JP_Sushi_07" },
                { "黃金鮪魚壽司", "JP_Sushi_08" },
                { "鮮蝦蘆筍手捲", "JP_Sushi_09" },
                { "綜合炙燒壽司", "JP_Sushi_10" },
                { "花壽司", "JP_Sushi_11" }
            }
        },
        { "居酒屋", new Dictionary<string, string>
            {
                { "花魚一夜干", "JP_Izakaya_01" },
                { "綜合雞肉串", "JP_Izakaya_02" },
                { "炭烤骰子牛", "JP_Izakaya_03" },
                { "醬烤豬肉串", "JP_Izakaya_04" },
                { "可樂餅", "JP_Izakaya_05" },
                { "生啤酒", "JP_Izakaya_06" },
                { "沙瓦", "JP_Izakaya_07" },
                { "唐揚雞", "JP_Izakaya_08" },
                { "炒烏龍麵", "JP_Izakaya_09" },
                { "鹽烤秋刀魚", "JP_Izakaya_10" },
                { "明太子雞翅", "JP_Izakaya_11" },
                { "山藥泥沙拉", "JP_Izakaya_12" },
                { "鹽烤櫛瓜串", "JP_Izakaya_13" }
            }
        },
                { "蛋包飯", new Dictionary<string, string>
                    {
                    { "番茄蛋包飯", "JP_OmuRice_01" },
                    { "咖哩歐姆蛋包飯", "JP_OmuRice_02" },
                    { "唐揚雞蛋包飯", "JP_OmuRice_03" },
                    { "炸豬排蛋包飯", "JP_OmuRice_04" },
                    { "蘑菇奶油醬蛋包飯", "JP_OmuRice_05" },
                    { "咖哩鮮蝦蛋包飯", "JP_OmuRice_06" },
                    { "紅酒牛肉歐姆蛋包飯", "JP_OmuRice_07" },
                    { "鮮蔬蛋包飯", "JP_OmuRice_08" },
                    { "漢堡排蛋包飯", "JP_OmuRice_09" }
                    }

                },
                { "咖哩飯", new Dictionary<string, string>
                    {
                        { "唐揚雞咖哩飯", "JP_KareRice_01" },
                        { "椰香牛肉綠咖哩飯", "JP_KareRice_02" },
                        { "炸豬排咖哩飯", "JP_KareRice_03" },
                        { "菇菇蔬菜咖哩飯", "JP_KareRice_04" },
                        { "經典雞肉咖哩飯", "JP_KareRice_05" },
                        { "炸蝦咖哩飯", "JP_KareRice_06" },
                        { "漢堡排咖哩飯", "JP_KareRice_07" },
                        { "極辛咖哩飯", "JP_KareRice_08" }
                    }
                },
                { "飯糰", new Dictionary<string, string>
                    {
                        { "炙燒明太子鮭魚飯糰", "JP_Onigiri_01" },
                        { "紫蘇梅子飯糰", "JP_Onigiri_02" },
                        { "鮮蔬溏心蛋飯糰", "JP_Onigiri_03" },
                        { "牛蒡飯糰", "JP_Onigiri_04" },
                        { "照燒雞飯糰", "JP_Onigiri_05" },
                        { "肉鬆飯糰", "JP_Onigiri_06" },
                        { "鮪魚飯糰", "JP_Onigiri_07" },
                        { "醬汁柴魚飯糰", "JP_Onigiri_08" },
                        { "山藥泥飯糰", "JP_Onigiri_09" }
                    }
                }

    };

    // 顧客圖片對應資料庫
    private Dictionary<string, string> customerImages = new Dictionary<string, string>
    {
        { "喵喵", "UI_NPC_Cat_01" },
        { "咪咪", "UI_NPC_Cat_02" },
        { "雞哥", "UI_NPC_Chicken_01" },
        { "雞咕", "UI_NPC_Chicken_02" },
        { "小白妹", "UI_NPC_Rabbit_01" },
        { "小白姊", "UI_NPC_Rabbit_02" },
        { "小浣", "UI_NPC_Raccoon_01" },
        { "小狸", "UI_NPC_Raccoon_02" },
        { "小梅", "UI_NPC_SikaDeer_01" },
        { "小斑", "UI_NPC_SikaDeer_02" }
    };
    private void Start()
    {
        orderController = FindObjectOfType<OrderController>();
        bagManager = FindObjectOfType<BagManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        levelManager = FindObjectOfType<LevelManager>();

    }
    //public void CreateNewItem(float order_time, string Food_Name, string Cus_Name, string Shop_Name, GameObject contentPanel, List<GameObject> itemList)
    //{
    public void CreateNewItem(float order_time, string Food_Name, string Cus_Name, string Shop_Name, GameObject contentPanel, List<GameObject> itemList, int? orderId = null)
    {
        currentOrderId = orderId ?? nextOrderId++;
        orderCustomerNames[currentOrderId] = Cus_Name;
        GameObject newItem = Instantiate(itemPrefab, contentPanel.transform);

        Text orderTimeText = newItem.transform.Find("OrderTimeText").GetComponent<Text>();
        Text foodNameText = newItem.transform.Find("FoodNameText").GetComponent<Text>();
        Text cusNameText = newItem.transform.Find("CusNameText").GetComponent<Text>();
        Text shopNameText = newItem.transform.Find("ShopNameText").GetComponent<Text>();
        Image foodImage = newItem.transform.Find("FOOD_pic").GetComponent<Image>();
        Image cusImage = newItem.transform.Find("CUS_pic").GetComponent<Image>();
        Text Order_id = newItem.transform.Find("order_ID").GetComponent<Text>();

        orderTimeText.text = order_time.ToString();
        foodNameText.text = Food_Name;
        cusNameText.text = Cus_Name;
        shopNameText.text = Shop_Name;

        LoadFoodImage(Food_Name, foodImage);
        LoadCustomerImage(Cus_Name, cusImage);

        orderStatuses[currentOrderId] = "未取餐"; // 初始化状态

        orderTimes[currentOrderId] = order_time;
        Order_id.text = currentOrderId.ToString();
        // 如果是 contentPanel2，啟動倒計時並處理按鈕禁用
        if (contentPanel == contentPanel2)
        {
            StartCoroutine(StartCountdown(currentOrderId, newItem));
            orderController.SpawnCustomerAtLocation(Cus_Name, currentOrderId);
            newItem.tag = "order_start";
            bagManager.AddItemToBag(foodimagename, currentOrderId);
            CreatePreviewItem(currentOrderId, Cus_Name, Shop_Name, Food_Name, order_time);
            Debug.LogWarning("提取原先id: " + currentOrderId + " food: " + Food_Name);

            // 禁用按鈕並更改文本為 "進行中"
            Button itemButton = newItem.GetComponentInChildren<Button>();
            if (itemButton != null)
            {
                itemButton.interactable = false;

                Text buttonText = itemButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = "進行中";
                }
            }

            orderController.InstantiateAtRestaurantPosition(currentOrderId, Shop_Name);
        }

        itemList.Add(newItem);

        Button deleteButton = newItem.GetComponentInChildren<Button>();
        deleteButton.onClick.AddListener(() => DeleteItem(newItem, order_time, Food_Name, Cus_Name, Shop_Name));
        //// 如果是 contentPanel2，啟動倒計時並處理按鈕禁用
        //if (contentPanel == contentPanel2)
        //{
        //    StartCoroutine(StartCountdown(currentOrderId, newItem));
        //    orderController.SpawnCustomerAtLocation(Cus_Name);
        //    newItem.tag = "order_start";
        //    bagManager.AddItemToBag(foodimagename, currentOrderId);
        //    CreatePreviewItem(currentOrderId, Cus_Name, Shop_Name, Food_Name, order_time);
        //    Debug.LogWarning ("提取原先id" + currentOrderId + "food:" + Food_Name);
        //    // 禁用按鈕並更改文本為 "進行中"
        //    Button itemButton = newItem.GetComponentInChildren<Button>();
        //    if (itemButton != null)
        //    {
        //        itemButton.interactable = false;

        //        Text buttonText = itemButton.GetComponentInChildren<Text>();
        //        if (buttonText != null)
        //        {
        //            buttonText.text = "進行中";
        //        }
        //    }

        //    orderController.InstantiateAtRestaurantPosition();
        //}
        //itemList.Add(newItem);
        //Button deleteButton = newItem.GetComponentInChildren<Button>();
        //deleteButton.onClick.AddListener(() => DeleteItem(newItem, order_time, Food_Name, Cus_Name, Shop_Name));
    }


    /// 查詢並更新食物圖片
    private void LoadFoodImage(string foodName, Image foodImage)
    {
        string foodImageName = "";
        foreach (var restaurant in restaurantFoods)
        {
            if (restaurant.Value.ContainsKey(foodName))
            {
                foodImageName = restaurant.Value[foodName];
                break;
            }
        }

        if (!string.IsNullOrEmpty(foodImageName))
        {
            foodImage.sprite = Resources.Load<Sprite>("Order_Pic/" + foodImageName);
            //Debug.LogWarning ("Order_Pic/" + foodImageName);
            foodimagename = foodImageName;
        }

    }

    // 查詢並更新顧客圖片
    private void LoadCustomerImage(string cusName, Image cusImage)
    {
        if (customerImages.ContainsKey(cusName))
        {
            string customerImageName = customerImages[cusName];
            cusImage.sprite = Resources.Load<Sprite>("cus_pic/" + customerImageName);
        }
    }

    void DeleteItem(GameObject item, float order_time, string Food_Name, string Cus_Name, string Shop_Name)
    {
        
        // 從 contentPanel1 中刪除該項目
        itemObjects1.Remove(item);
        Text orderIdText = item.transform.Find("order_ID").GetComponent<Text>();
        int itemId = int.Parse(orderIdText.text);
        Destroy(item);
        // 在 contentPanel2 中重新實例化該項目
        //CreateNewItem(order_time, Food_Name, Cus_Name, Shop_Name, contentPanel2, itemObjects2);
        CreateNewItem(order_time, Food_Name, Cus_Name, Shop_Name, contentPanel2, itemObjects2, itemId);
    }

    private IEnumerator StartCountdown(int orderId, GameObject item)
    {
        UpdateOngoingOrderCount();
        float initialTime = orderTimes[orderId];
        while (orderTimes[orderId] > 0)
        {
            yield return new WaitForSeconds(1);
            orderTimes[orderId]--;
            if (Input.GetKey(KeyCode.F11)) orderTimes[orderId] -= 10;

            // 更新 UI 顯示
            Text orderTimeText = item.transform.Find("OrderTimeText").GetComponent<Text>();
            orderTimeText.text = orderTimes[orderId].ToString();

            // 更新倒數時間的 UI
            if (orderTimeLimitText != null)
            {
                orderTimeLimitText.text = "剩餘" + orderTimes[orderId].ToString() + "s";
            }
            // 更新預覽中的剩餘時間
            if (previewItems.ContainsKey(orderId) != null)
            {
                GameObject previewItem = previewItems[orderId];
                if (previewItem != null)
                {
                    Text previewTimeText = previewItem.transform.Find("Order_TimeLimit").GetComponent<Text>();
                    if (previewTimeText != null)
                    {
                        previewTimeText.text = "剩餘" + orderTimes[orderId].ToString() + "s";
                    }

                }

            }
            //if (orderTimes[orderId] <= 0) {
            //    DeleteItemFromPanel2(item, false);
            //    bagManager.RemoveItemFromBagByID(orderId);
            //    DeletePreviewItem(orderId);
            //}
            // 倒計時結束，根據狀態進行處理
            if (orderStatuses[orderId] == "已送達" && orderTimes[orderId] >= 0)
            {
                // 處理已完成的訂單
                DeleteItemFromPanel2(item, true);
                bagManager.RemoveItemFromBagByID(orderId);
                DeletePreviewItem(orderId);
                //moneyManager.AddCompletion();
                Debug.Log($"訂單 {orderId} 倒計時結束，狀態: {orderStatuses[orderId]}");


            }
            else if (orderTimes[orderId] < 0)
            {
                // 處理超時的訂單
                DeleteItemFromPanel2(item, false);
                //Debug.Log("超時");
                bagManager.RemoveItemFromBagByID(orderId);
                DeletePreviewItem(orderId);
                orderStatuses[orderId] = "超時";
                moneyManager.AddTimeout();
                Debug.Log($"訂單 {orderId} 倒計時結束，狀態: {orderStatuses[orderId]}");


            }
            // 從相關列表中移除

            //if (orderTimes[orderId] != 0)
            //{
            //    bool isCompleted = previewItems[orderId]; // 自定義邏輯檢測是否完成
            //    if (isCompleted)
            //    {
            //        Debug.LogWarning("錯誤進入");
            //       // MarkOrderAsCompleted(item, initialTime - orderTimes[orderId]); // 計算花費的時間
            //    }
            //    else
            //    {
            //        DeleteItemFromPanel2(item, false); // 移動到紀錄面板並標記為超時
            //        bagManager.RemoveItemFromBagByID(orderId); 
            //    }

            //    Debug.Log("訂單時間到，處理完成或超時邏輯");
            //}


        }
    }
    public void VerifyOrderDelivery(int providedOrderId)
    {
        // 找到第一個狀態為 "已取餐" 的訂單
        int? firstPickedOrderId = null;

        foreach (var entry in orderStatuses)
        {
            if (entry.Value == "已取餐")
            {
                firstPickedOrderId = entry.Key;
                break;
            }
        }

        if (firstPickedOrderId == null)
        {
            Debug.LogWarning("目前沒有任何 '已取餐' 的訂單");
            return;
        }

        // 比對訂單 ID
        if (firstPickedOrderId == providedOrderId)
        {
            Debug.Log($"訂單正確，ID: {providedOrderId}");
            moneyManager.AddCompletion();
            levelManager.totalOrders += 1;
        }
        else
        {
            Debug.LogWarning($"訂單錯誤，提供的 ID: {providedOrderId}，應為 ID: {firstPickedOrderId}");
            moneyManager.AddError();
            levelManager.totalErrors += 1;
            //連同錯誤訂單一起刪除

        }
    }
    public void DeleteItemFromPanel2(GameObject item, bool isCompleted)
    {
        Debug.Log("收到要求:" + item + "T F?" + isCompleted);
        if (itemObjects2.Contains(item))
        {
            // 從 panel2 中移除
            itemObjects2.Remove(item);
            item.transform.SetParent(contentPanel3.transform);  // 將項目移動到 contentPanel3
            itemObjects3.Add(item);  // 加入到紀錄列表


            Button itemButton = item.GetComponentInChildren<Button>();
            if (itemButton != null)
            {
                itemButton.interactable = false;  // 禁用按鈕

                // 根據狀態更改按鈕文本
                Text buttonText = itemButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = isCompleted ? "完成" : "超時";
                }
            }

            Debug.Log("訂單項目已從 contentPanel2 移動到 contentPanel3 並已禁用按鈕功能");
        }
        else
        {
            Debug.Log("沒有可移動的訂單項目");
        }
        CheckAndTriggerSettlement();
    }

    private void CreatePreviewItem(int orderId, string cusName, string shopName, string foodName, float orderTime)
    {
        GameObject previewItem = Instantiate(previewPrefab, contentPanel4.transform);

        Text orderTitleText = previewItem.transform.Find("OrderTitle").GetComponent<Text>();
        Text orderTimeLimitText = previewItem.transform.Find("Order_TimeLimit").GetComponent<Text>();

        orderTitleText.text = $"[{cusName}]{shopName}-{foodName}";
        orderTimeLimitText.text = "剩餘" + orderTimes[orderId].ToString() + "s";


        // 將預覽項目加入列表，並記錄與訂單 ID 的關係
        previewItem.name = $"Preview_{orderId}";
        previewItems[orderId] = previewItem;  // 添加到字典
        itemObjects4.Add(previewItem);

        UpdateOngoingOrderCount();
    }

    // 查询订单状态
    public string GetOrderStatus(int orderId)
    {
        if (orderStatuses.TryGetValue(orderId, out string status))
        {
            return status;
        }
        return "未知";
    }

    // 更新订单状态
    public void UpdateOrderStatus(int orderId, string newStatus)
    {
        if (orderStatuses.ContainsKey(orderId))
        {
            orderStatuses[orderId] = newStatus;
            Debug.Log($"订单 {orderId} 状态更新为: {newStatus}");
        }
        else
        {
            Debug.LogWarning($"无法更新状态，订单 {orderId} 不存在");
        }
    }







    public void DeletePreviewItem(int orderId)
    {
        // 查找並刪除對應的預覽項目
        GameObject previewItem = itemObjects4.Find(item => item.name == $"Preview_{orderId}");
        if (previewItem != null)
        {
            itemObjects4.Remove(previewItem);
            Destroy(previewItem);
        }

        UpdateOngoingOrderCount();
    }

    private void UpdateOngoingOrderCount()
    {
        //if (ongoingOrderCountText != null)
        //{
        //    ongoingOrderCountText.text = $"進行中訂單數量: {itemObjects4.Count}";
        //}
        int ongoingCount = previewItems.Count;

        if (ongoingOrderCountText != null)
        {
            ongoingOrderCountText.text = $"進行中訂單: {ongoingCount}";
        }
    }
    public void MarkOrderAsCompleted(GameObject item, float totalTime)
    {
        if (itemObjects2.Contains(item)) // 檢查該項目是否存在於 contentPanel2
        {
            // 從 panel2 中移除
            itemObjects2.Remove(item);

            // 將該項目移動到 contentPanel3 並更新其狀態
            item.transform.SetParent(contentPanel3.transform);
            itemObjects3.Add(item);

            // 更新按鈕狀態
            Button itemButton = item.GetComponentInChildren<Button>();
            if (itemButton != null)
            {
                itemButton.interactable = false;

                Text buttonText = itemButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = "完成";
                }
            }

            // 輸出該訂單的完成時間
            Debug.Log($"訂單完成！總花費時間: {totalTime} 秒");

            // 顯示在 UI（可選，根據需求）
            Text orderTimeText = item.transform.Find("OrderTimeText").GetComponent<Text>();
            if (orderTimeText != null)
            {
                orderTimeText.text = $"完成時間: {totalTime:F1}s";
            }
        }
        else
        {
            Debug.Log("該訂單項目不在 contentPanel2 中，無法標記為完成");
        }
    }

    public string GetCustomerNameByOrderId(int orderId)
    {
        if (orderCustomerNames.TryGetValue(orderId, out string customerName))
        {
            return customerName;
        }
        else
        {
            Debug.LogWarning($"未找到對應的顧客名稱，訂單 ID: {orderId}");
            return null;
        }
    }

    public void CheckAndTriggerSettlement()
    {
        // 檢查兩個列表是否都為空
        if (itemObjects1.Count == 0 && itemObjects2.Count == 0)
        {
            if (isc != true) { 
            moneyManager.CalculateFinalEarnings();
            isc = true;
            }
        }
        else
        {
            Debug.Log("列表中仍有物件，未觸發結算邏輯。");
        }

    }
}
