using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newlist_test : MonoBehaviour
{
    private MoneyManager moneyManager;
    public GameObject contentPanel1;
    public GameObject contentPanel2;

    public GameObject contentPanel3;  // �µļo����

    public GameObject contentPanel4; // �A�[�M����ӆ�ε�����
    public GameObject itemPrefab;
    public OrderController orderController;

    public GameObject previewPrefab; // �A�[ӆ�ε��A�u�w
    public Text ongoingOrderCountText; // ����@ʾ�M����ӆ�Δ������ı�

    private BagManager bagManager;
    private string foodimagename;

    public Dictionary<string, Sprite> foodSprites = new Dictionary<string, Sprite>(); // ʳ��DƬ������
    public Dictionary<string, Sprite> customerSprites = new Dictionary<string, Sprite>(); // ͈DƬ������
    bool isc = false;


    static public bool isEndPoint = false;
    public List<GameObject> itemObjects1 = new List<GameObject>();
    public List<GameObject> itemObjects2 = new List<GameObject>();
    public List<GameObject> itemObjects3 = new List<GameObject>();  // ��żo��Ŀ
    public List<GameObject> itemObjects4 = new List<GameObject>(); // �A�[�M���е�ӆ��
    private Dictionary<int, GameObject> previewItems = new Dictionary<int, GameObject>();
    private Dictionary<int, float> orderTimes = new Dictionary<int, float>(); //ӆ�Εr�g�ֵ�
    private int nextOrderId = 1;  // �Á�����Ψһ ID
    int currentOrderId;
    private Text orderTimeLimitText;

    private LevelManager levelManager;
    private Dictionary<int, string> orderStatuses = new Dictionary<int, string>(); //��¼ÿ��������״̬

    private Dictionary<int, string> orderCustomerNames = new Dictionary<int, string>(); // �ֵ��惦ӆ�� ID �c����Q

    public class OrderItem : MonoBehaviour
    {
        public int OrderId { get; private set; }

        public void Initialize(float order_time, string foodName, string cusName, string shopName, int orderId)
        {
            // ��ʼ������
            OrderId = orderId;
            // ������ʼ��߉݋
        }
    }

    private Dictionary<string, Dictionary<string, string>> restaurantFoods = new Dictionary<string, Dictionary<string, string>>
    {
        { "��˾��", new Dictionary<string, string>
            {
                { "�C�ω�˾", "JP_Sushi_01" },
                { "�C�����~Ƭ", "JP_Sushi_02" },
                { "�n�~��˾", "JP_Sushi_03" },
                { "�q�~��˾", "JP_Sushi_04" },
                { "���ӟ���˾", "JP_Sushi_05" },
                { "�q�~���־�", "JP_Sushi_06" },
                { "��Ƥ��˾", "JP_Sushi_07" },
                { "�S���n�~��˾", "JP_Sushi_08" },
                { "�r�r�J�S�֒�", "JP_Sushi_09" },
                { "�C���˟���˾", "JP_Sushi_10" },
                { "����˾", "JP_Sushi_11" }
            }
        },
        { "�Ӿ���", new Dictionary<string, string>
            {
                { "���~һҹ��", "JP_Izakaya_01" },
                { "�C���u�⴮", "JP_Izakaya_02" },
                { "̿������ţ", "JP_Izakaya_03" },
                { "�u���i�⴮", "JP_Izakaya_04" },
                { "�ɘ��", "JP_Izakaya_05" },
                { "��ơ��", "JP_Izakaya_06" },
                { "ɳ��", "JP_Izakaya_07" },
                { "�ƓP�u", "JP_Izakaya_08" },
                { "�������I", "JP_Izakaya_09" },
                { "�}���ﵶ�~", "JP_Izakaya_10" },
                { "��̫���u��", "JP_Izakaya_11" },
                { "ɽˎ��ɳ��", "JP_Izakaya_12" },
                { "�}�����ϴ�", "JP_Izakaya_13" }
            }
        },
                { "�����", new Dictionary<string, string>
                    {
                    { "���ѵ����", "JP_OmuRice_01" },
                    { "�����Wķ�����", "JP_OmuRice_02" },
                    { "�ƓP�u�����", "JP_OmuRice_03" },
                    { "ը�i�ŵ����", "JP_OmuRice_04" },
                    { "Ģ�������u�����", "JP_OmuRice_05" },
                    { "�����r�r�����", "JP_OmuRice_06" },
                    { "�t��ţ��Wķ�����", "JP_OmuRice_07" },
                    { "�r�ߵ����", "JP_OmuRice_08" },
                    { "�h���ŵ����", "JP_OmuRice_09" }
                    }

                },
                { "�����", new Dictionary<string, string>
                    {
                        { "�ƓP�u�����", "JP_KareRice_01" },
                        { "Ҭ��ţ��G�����", "JP_KareRice_02" },
                        { "ը�i�ſ����", "JP_KareRice_03" },
                        { "�����߲˿����", "JP_KareRice_04" },
                        { "�����u�⿧���", "JP_KareRice_05" },
                        { "ը�r�����", "JP_KareRice_06" },
                        { "�h���ſ����", "JP_KareRice_07" },
                        { "�O�������", "JP_KareRice_08" }
                    }
                },
                { "a", new Dictionary<string, string>
                    {
                        { "�˟���̫���q�~a", "JP_Onigiri_01" },
                        { "���K÷��a", "JP_Onigiri_02" },
                        { "�r�����ĵ�a", "JP_Onigiri_03" },
                        { "ţ��a", "JP_Onigiri_04" },
                        { "�՟��ua", "JP_Onigiri_05" },
                        { "���a", "JP_Onigiri_06" },
                        { "�n�~a", "JP_Onigiri_07" },
                        { "�u֭���~a", "JP_Onigiri_08" },
                        { "ɽˎ��a", "JP_Onigiri_09" }
                    }
                }

    };

    // ͈DƬ�����Y�ώ�
    private Dictionary<string, string> customerImages = new Dictionary<string, string>
    {
        { "����", "UI_NPC_Cat_01" },
        { "����", "UI_NPC_Cat_02" },
        { "�u��", "UI_NPC_Chicken_01" },
        { "�u��", "UI_NPC_Chicken_02" },
        { "С����", "UI_NPC_Rabbit_01" },
        { "С���", "UI_NPC_Rabbit_02" },
        { "С�", "UI_NPC_Raccoon_01" },
        { "С��", "UI_NPC_Raccoon_02" },
        { "С÷", "UI_NPC_SikaDeer_01" },
        { "С��", "UI_NPC_SikaDeer_02" }
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

        orderStatuses[currentOrderId] = "δȡ��"; // ��ʼ��״̬

        orderTimes[currentOrderId] = order_time;
        Order_id.text = currentOrderId.ToString();
        // ����� contentPanel2�����ӵ�Ӌ�r�K̎���o����
        if (contentPanel == contentPanel2)
        {
            StartCoroutine(StartCountdown(currentOrderId, newItem));
            orderController.SpawnCustomerAtLocation(Cus_Name, currentOrderId);
            newItem.tag = "order_start";
            bagManager.AddItemToBag(foodimagename, currentOrderId);
            CreatePreviewItem(currentOrderId, Cus_Name, Shop_Name, Food_Name, order_time);
            Debug.LogWarning("��ȡԭ��id: " + currentOrderId + " food: " + Food_Name);

            // ���ð��o�K�����ı��� "�M����"
            Button itemButton = newItem.GetComponentInChildren<Button>();
            if (itemButton != null)
            {
                itemButton.interactable = false;

                Text buttonText = itemButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = "�M����";
                }
            }

            orderController.InstantiateAtRestaurantPosition(currentOrderId, Shop_Name);
        }

        itemList.Add(newItem);

        Button deleteButton = newItem.GetComponentInChildren<Button>();
        deleteButton.onClick.AddListener(() => DeleteItem(newItem, order_time, Food_Name, Cus_Name, Shop_Name));
        //// ����� contentPanel2�����ӵ�Ӌ�r�K̎���o����
        //if (contentPanel == contentPanel2)
        //{
        //    StartCoroutine(StartCountdown(currentOrderId, newItem));
        //    orderController.SpawnCustomerAtLocation(Cus_Name);
        //    newItem.tag = "order_start";
        //    bagManager.AddItemToBag(foodimagename, currentOrderId);
        //    CreatePreviewItem(currentOrderId, Cus_Name, Shop_Name, Food_Name, order_time);
        //    Debug.LogWarning ("��ȡԭ��id" + currentOrderId + "food:" + Food_Name);
        //    // ���ð��o�K�����ı��� "�M����"
        //    Button itemButton = newItem.GetComponentInChildren<Button>();
        //    if (itemButton != null)
        //    {
        //        itemButton.interactable = false;

        //        Text buttonText = itemButton.GetComponentInChildren<Text>();
        //        if (buttonText != null)
        //        {
        //            buttonText.text = "�M����";
        //        }
        //    }

        //    orderController.InstantiateAtRestaurantPosition();
        //}
        //itemList.Add(newItem);
        //Button deleteButton = newItem.GetComponentInChildren<Button>();
        //deleteButton.onClick.AddListener(() => DeleteItem(newItem, order_time, Food_Name, Cus_Name, Shop_Name));
    }


    /// ��ԃ�K����ʳ��DƬ
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

    // ��ԃ�K����͈DƬ
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
        
        // �� contentPanel1 �Єh��ԓ�Ŀ
        itemObjects1.Remove(item);
        Text orderIdText = item.transform.Find("order_ID").GetComponent<Text>();
        int itemId = int.Parse(orderIdText.text);
        Destroy(item);
        // �� contentPanel2 ����������ԓ�Ŀ
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

            // ���� UI �@ʾ
            Text orderTimeText = item.transform.Find("OrderTimeText").GetComponent<Text>();
            orderTimeText.text = orderTimes[orderId].ToString();

            // ���µ����r�g�� UI
            if (orderTimeLimitText != null)
            {
                orderTimeLimitText.text = "ʣ�N" + orderTimes[orderId].ToString() + "s";
            }
            // �����A�[�е�ʣ�N�r�g
            if (previewItems.ContainsKey(orderId) != null)
            {
                GameObject previewItem = previewItems[orderId];
                if (previewItem != null)
                {
                    Text previewTimeText = previewItem.transform.Find("Order_TimeLimit").GetComponent<Text>();
                    if (previewTimeText != null)
                    {
                        previewTimeText.text = "ʣ�N" + orderTimes[orderId].ToString() + "s";
                    }

                }

            }
            //if (orderTimes[orderId] <= 0) {
            //    DeleteItemFromPanel2(item, false);
            //    bagManager.RemoveItemFromBagByID(orderId);
            //    DeletePreviewItem(orderId);
            //}
            // ��Ӌ�r�Y����������B�M��̎��
            if (orderStatuses[orderId] == "�����_" && orderTimes[orderId] >= 0)
            {
                // ̎������ɵ�ӆ��
                DeleteItemFromPanel2(item, true);
                bagManager.RemoveItemFromBagByID(orderId);
                DeletePreviewItem(orderId);
                //moneyManager.AddCompletion();
                Debug.Log($"ӆ�� {orderId} ��Ӌ�r�Y������B: {orderStatuses[orderId]}");


            }
            else if (orderTimes[orderId] < 0)
            {
                // ̎���r��ӆ��
                DeleteItemFromPanel2(item, false);
                //Debug.Log("���r");
                bagManager.RemoveItemFromBagByID(orderId);
                DeletePreviewItem(orderId);
                orderStatuses[orderId] = "���r";
                moneyManager.AddTimeout();
                Debug.Log($"ӆ�� {orderId} ��Ӌ�r�Y������B: {orderStatuses[orderId]}");


            }
            // �����P�б����Ƴ�

            //if (orderTimes[orderId] != 0)
            //{
            //    bool isCompleted = previewItems[orderId]; // �Զ��x߉݋�z�y�Ƿ����
            //    if (isCompleted)
            //    {
            //        Debug.LogWarning("�e�`�M��");
            //       // MarkOrderAsCompleted(item, initialTime - orderTimes[orderId]); // Ӌ�㻨�M�ĕr�g
            //    }
            //    else
            //    {
            //        DeleteItemFromPanel2(item, false); // �Ƅӵ��o����K��ӛ�鳬�r
            //        bagManager.RemoveItemFromBagByID(orderId); 
            //    }

            //    Debug.Log("ӆ�Εr�g����̎����ɻ򳬕r߉݋");
            //}


        }
    }
    public void VerifyOrderDelivery(int providedOrderId)
    {
        // �ҵ���һ����B�� "��ȡ��" ��ӆ��
        int? firstPickedOrderId = null;

        foreach (var entry in orderStatuses)
        {
            if (entry.Value == "��ȡ��")
            {
                firstPickedOrderId = entry.Key;
                break;
            }
        }

        if (firstPickedOrderId == null)
        {
            Debug.LogWarning("Ŀǰ�]���κ� '��ȡ��' ��ӆ��");
            return;
        }

        // �Ȍ�ӆ�� ID
        if (firstPickedOrderId == providedOrderId)
        {
            Debug.Log($"ӆ�����_��ID: {providedOrderId}");
            moneyManager.AddCompletion();
            levelManager.totalOrders += 1;
        }
        else
        {
            Debug.LogWarning($"ӆ���e�`���ṩ�� ID: {providedOrderId}������ ID: {firstPickedOrderId}");
            moneyManager.AddError();
            levelManager.totalErrors += 1;
            //�Bͬ�e�`ӆ��һ���h��

        }
    }
    public void DeleteItemFromPanel2(GameObject item, bool isCompleted)
    {
        Debug.Log("�յ�Ҫ��:" + item + "T F?" + isCompleted);
        if (itemObjects2.Contains(item))
        {
            // �� panel2 ���Ƴ�
            itemObjects2.Remove(item);
            item.transform.SetParent(contentPanel3.transform);  // ���Ŀ�Ƅӵ� contentPanel3
            itemObjects3.Add(item);  // ���뵽�o��б�


            Button itemButton = item.GetComponentInChildren<Button>();
            if (itemButton != null)
            {
                itemButton.interactable = false;  // ���ð��o

                // ������B���İ��o�ı�
                Text buttonText = itemButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = isCompleted ? "���" : "���r";
                }
            }

            Debug.Log("ӆ���Ŀ�я� contentPanel2 �Ƅӵ� contentPanel3 �K�ѽ��ð��o����");
        }
        else
        {
            Debug.Log("�]�п��Ƅӵ�ӆ���Ŀ");
        }
        CheckAndTriggerSettlement();
    }

    private void CreatePreviewItem(int orderId, string cusName, string shopName, string foodName, float orderTime)
    {
        GameObject previewItem = Instantiate(previewPrefab, contentPanel4.transform);

        Text orderTitleText = previewItem.transform.Find("OrderTitle").GetComponent<Text>();
        Text orderTimeLimitText = previewItem.transform.Find("Order_TimeLimit").GetComponent<Text>();

        orderTitleText.text = $"[{cusName}]{shopName}-{foodName}";
        orderTimeLimitText.text = "ʣ�N" + orderTimes[orderId].ToString() + "s";


        // ���A�[�Ŀ�����б��Kӛ��cӆ�� ID ���P�S
        previewItem.name = $"Preview_{orderId}";
        previewItems[orderId] = previewItem;  // ��ӵ��ֵ�
        itemObjects4.Add(previewItem);

        UpdateOngoingOrderCount();
    }

    // ��ѯ����״̬
    public string GetOrderStatus(int orderId)
    {
        if (orderStatuses.TryGetValue(orderId, out string status))
        {
            return status;
        }
        return "δ֪";
    }

    // ���¶���״̬
    public void UpdateOrderStatus(int orderId, string newStatus)
    {
        if (orderStatuses.ContainsKey(orderId))
        {
            orderStatuses[orderId] = newStatus;
            Debug.Log($"���� {orderId} ״̬����Ϊ: {newStatus}");
        }
        else
        {
            Debug.LogWarning($"�޷�����״̬������ {orderId} ������");
        }
    }







    public void DeletePreviewItem(int orderId)
    {
        // ���ҁK�h���������A�[�Ŀ
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
        //    ongoingOrderCountText.text = $"�M����ӆ�Δ���: {itemObjects4.Count}";
        //}
        int ongoingCount = previewItems.Count;

        if (ongoingOrderCountText != null)
        {
            ongoingOrderCountText.text = $"�M����ӆ��: {ongoingCount}";
        }
    }
    public void MarkOrderAsCompleted(GameObject item, float totalTime)
    {
        if (itemObjects2.Contains(item)) // �z��ԓ�Ŀ�Ƿ����� contentPanel2
        {
            // �� panel2 ���Ƴ�
            itemObjects2.Remove(item);

            // ��ԓ�Ŀ�Ƅӵ� contentPanel3 �K�������B
            item.transform.SetParent(contentPanel3.transform);
            itemObjects3.Add(item);

            // ���°��o��B
            Button itemButton = item.GetComponentInChildren<Button>();
            if (itemButton != null)
            {
                itemButton.interactable = false;

                Text buttonText = itemButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = "���";
                }
            }

            // ݔ��ԓӆ�ε���ɕr�g
            Debug.Log($"ӆ����ɣ������M�r�g: {totalTime} ��");

            // �@ʾ�� UI�����x����������
            Text orderTimeText = item.transform.Find("OrderTimeText").GetComponent<Text>();
            if (orderTimeText != null)
            {
                orderTimeText.text = $"��ɕr�g: {totalTime:F1}s";
            }
        }
        else
        {
            Debug.Log("ԓӆ���Ŀ���� contentPanel2 �У��o����ӛ�����");
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
            Debug.LogWarning($"δ�ҵ�����������Q��ӆ�� ID: {orderId}");
            return null;
        }
    }

    public void CheckAndTriggerSettlement()
    {
        // �z��ɂ��б��Ƿ񶼞��
        if (itemObjects1.Count == 0 && itemObjects2.Count == 0)
        {
            if (isc != true) { 
            moneyManager.CalculateFinalEarnings();
            isc = true;
            }
        }
        else
        {
            Debug.Log("�б������������δ�|�l�Y��߉݋��");
        }

    }
}
