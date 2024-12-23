using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ؓ؟����ӆ��
/// </summary>
public class OrderController : MonoBehaviour
{
    // ����Q�c Prefab ��ӳ��
    public Dictionary<string, GameObject> customerPrefabs = new Dictionary<string, GameObject>();

    // ��ʼ��ӳ������� Inspector ���O����
    [System.Serializable]
    public struct CustomerData
    {
        public string customerName; // ����Q
        public GameObject customerPrefab; // ͵� Prefab
    }

    public List<CustomerData> customerDataList = new List<CustomerData>();

    [System.Serializable]
    public class LevelDifficulty
    {
        public float order_time; // ӆ�Εr�g
        public List<string> allowedRestaurants; // ���S�Ĳ͏d
        public List<string> allowedCustomers; // ���S���
        public int orderCount;
    }

    private float order_time;
    public string cusname;

    public newlist_test listScript; // ���õ�һ���_���Č���
    private Transform customer, restaurant;
    // key������Q�����ʳ���б�
    private Dictionary<string, List<string>> restaurantFoods = new Dictionary<string, List<string>>();

    //key������Q���������
    private Dictionary<string, Transform> restaurantTransforms = new Dictionary<string, Transform>();

    //key����Q�������
    private Dictionary<string, Transform> customerTransforms = new Dictionary<string, Transform>();

    private Dictionary<int, LevelDifficulty> levelDifficulty = new Dictionary<int, LevelDifficulty>();

    public GameObject deliveryObjectPrefab;

    private List<string> availableRestaurants; // ���殔ǰ�P�����S�Ĳ͏d
    private List<string> availableCustomers; // ���殔ǰ�P�����S���

    void Start()
    {

        // �� List �е��Y�ϳ�ʼ���� Dictionary
        foreach (var data in customerDataList)
        {
            if (!customerPrefabs.ContainsKey(data.customerName))
            {
                customerPrefabs.Add(data.customerName, data.customerPrefab);
            }
        }
        InitializeRestaurantData();
        InitializeLevelDifficulty(); // ��ʼ���y�ȗl��

        string sceneName = SceneManager.GetActiveScene().name;
        int levelNumber = ExtractLevelNumber(sceneName);
        Debug.LogWarning ("��ȡ���P������: " + levelNumber);

        SetLevelConditions(levelNumber);
        //order_manager.orderTimer = order_time;
        //GenerateRandomOrder();
        if (levelDifficulty.ContainsKey(levelNumber))
        {
            int orderCount = levelDifficulty[levelNumber].orderCount;
            for (int i = 0; i < orderCount; i++)
            {
                GenerateRandomOrder();
            }
        }



    }
    private int ExtractLevelNumber(string sceneName)
    {
        // �ҵ���ĸ "V" ��λ�ÁK��ȡ����Ĳ���
        string numberPart = sceneName.Substring(sceneName.IndexOf('V') + 1);
        return int.Parse(numberPart); // �D�Q�������K����
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F12))
        {
            GenerateRandomOrder();
        }
    }
    // ��ʼ���͏d�cʳ���Y��
    private void InitializeRestaurantData()
    {
        restaurantFoods.Add("��˾��", new List<string> { "�C�ω�˾", "�C�����~Ƭ", "�n�~��˾", "�q�~��˾", "���ӟ���˾",
            "�q�~���־�","��Ƥ��˾","�S���n�~��˾","�r�r�J�S�֒�","�C���˟���˾","����˾" });
        restaurantFoods.Add("�Ӿ���", new List<string> { "���~һҹ��", "̿������ţ", "�u���i�⴮", "�ɘ��",
            "��ơ��","ɳ��","�ƓP�u","�������I","�}���ﵶ�~","��̫���u��","ɽˎ��ɳ��","�}�����ϴ�"});
        restaurantFoods.Add("�S�", new List<string> { "ţ���ϲ��ʳ","ը�r��D�_��ʳ","�џ����~��ʳ","ը�i�Ŷ�ʳ",
            "�q�~�H�ӁS","�}�����~��ʳ","�����ඨʳ","�ؼ���ɫ�S","���ƺ��r�S"});
        restaurantFoods.Add("�����", new List<string> {
            "���ѵ����",
            "�����Wķ�����",
            "�ƓP�u�����",
            "ը�i�ŵ����",
            "Ģ�������u�����",
            "�����r�r�����",
            "�t��ţ��Wķ�����",
            "�r�ߵ����",
            "�h���ŵ����"
        });
        restaurantFoods.Add("�����", new List<string> {
            "�ƓP�u�����",
            "Ҭ��ţ��G�����",
            "ը�i�ſ����",
            "�����߲˿����",
            "�����u�⿧���",
            "ը�r�����",
            "�h���ſ����",
            "�O�������"
        });



        restaurantTransforms.Add("��˾��", GameObject.Find("��˾��").transform);
        restaurantTransforms.Add("�Ӿ���", GameObject.Find("�Ӿ���").transform);
        restaurantTransforms.Add("�S�", GameObject.Find("�S�").transform);
        restaurantTransforms.Add("�����", GameObject.Find("�����").transform);

        restaurantTransforms.Add("�����", GameObject.Find("�����").transform);

        customerTransforms.Add("�u��", GameObject.Find("�u��").transform);
        customerTransforms.Add("����", GameObject.Find("����").transform);
        customerTransforms.Add("�u��", GameObject.Find("�u��").transform);
        customerTransforms.Add("С÷", GameObject.Find("С÷").transform);
        customerTransforms.Add("С�׽�", GameObject.Find("С�׽�").transform);
        //customerTransforms.Add("����", GameObject.Find("����").transform);
        customerTransforms.Add("С����", GameObject.Find("С����").transform);
        //customerTransforms.Add("С�", GameObject.Find("С�").transform);
        customerTransforms.Add("С��", GameObject.Find("С��").transform);
        //customerTransforms.Add("С÷", GameObject.Find("С÷").transform);
        //customerTransforms.Add("С��", GameObject.Find("С��").transform);

    }
    private void InitializeLevelDifficulty()
    {
        levelDifficulty.Add(1, new LevelDifficulty
        {
            order_time = 300f,
            allowedRestaurants = new List<string> { "��˾��" },
            allowedCustomers = new List<string> { "�u��" },
            orderCount = 1
        });

        levelDifficulty.Add(2, new LevelDifficulty
        {
            order_time = 100f,
            allowedRestaurants = new List<string> { "��˾��", "�����" },
            allowedCustomers = new List<string> { "����", "�u��" },
            orderCount = 5
        });

        levelDifficulty.Add(3, new LevelDifficulty
        {
            order_time = 400f,
            allowedRestaurants = new List<string> { "�����", "�����" },
            allowedCustomers = new List<string> { "С÷", "С����", "С��" },
            orderCount = 20
        });
        levelDifficulty.Add(4, new LevelDifficulty
        {
            order_time = 400f,
            allowedRestaurants = new List<string> { "�S�", "�����","�Ӿ���" },
            allowedCustomers = new List<string> { "С÷", "С����", "С��" },
            orderCount = 20
        });
        levelDifficulty.Add(5, new LevelDifficulty
        {
            order_time = 400f,
            allowedRestaurants = new List<string> { "�S�", "�����", "�Ӿ���" },
            allowedCustomers = new List<string> { "С÷", "С����", "С��" },
            orderCount = 20
        });
        levelDifficulty.Add(6, new LevelDifficulty
        {
            order_time = 400f,
            allowedRestaurants = new List<string> { "�S�", "�����", "�Ӿ���" },
            allowedCustomers = new List<string> { "С÷", "С����", "С��" },
            orderCount = 20
        });
        // ��Ӹ����P���O��...
    }

    private void GenerateRandomOrder() //GenerateRandomOrder(string customerName, float orderTime)
    {
        // �_���S�C�x��͏d��͕r���H�Į�ǰ�P�����S�Ĳ͏d������x��
        int randomCustomerIndex = Random.Range(0, availableCustomers.Count);
        string selectedCustomer = availableCustomers[randomCustomerIndex];

        int randomRestaurantIndex = Random.Range(0, availableRestaurants.Count);
        string selectedRestaurant = availableRestaurants[randomRestaurantIndex];

        // �S�C�x����c
        List<string> foods = restaurantFoods[selectedRestaurant];
        int randomFoodIndex = Random.Range(0, foods.Count);
        string selectedFood = foods[randomFoodIndex];

        Transform restaurantTransform = restaurantTransforms[selectedRestaurant];
        Transform customerTransform = customerTransforms[selectedCustomer];

        // ����ӆ��
        listScript.CreateNewItem(order_time, selectedFood, selectedCustomer, selectedRestaurant, listScript.contentPanel1, listScript.itemObjects1);
        Debug.Log($"�S�Cӆ��: {selectedRestaurant}����:{selectedFood}���: {selectedCustomer} \n �����: {customerTransform.position}���͏d����: {restaurantTransform.position} ");
        //Debug.Log("��������Q:" + selectedCustomer);
        
        customer = customerTransform;
        restaurant = restaurantTransform;
    }


    // ����Ŀǰ�P���O���y�ȗl��
    private void SetLevelConditions(int level)
    {
        if (levelDifficulty.ContainsKey(level))
        {
            LevelDifficulty levelData = levelDifficulty[level];

            // �O���r�g
            order_time = levelData.order_time;

            // �O�����ò͏d
            availableRestaurants = new List<string>();
            foreach (var restaurantName in levelData.allowedRestaurants)
            {
                if (restaurantFoods.ContainsKey(restaurantName))
                {
                    availableRestaurants.Add(restaurantName);
                }
            }

            // �O�������
            availableCustomers = new List<string>();
            foreach (var customerName in levelData.allowedCustomers)
            {
                if (customerTransforms.ContainsKey(customerName))
                {
                    availableCustomers.Add(customerName);
                }
            }

            // �@ʾ��ǰ�P���O��
            Debug.Log($"�P�� {level} �O�����: �r�g {order_time}, �͏d {string.Join(", ", availableRestaurants)}, � {string.Join(", ", availableCustomers)}");
        }
        else
        {
            Debug.LogError("�o���P��");
        }
    }
    public void InstantiateAtRestaurantPosition(int id,string pos)
    {
        GameObject deliveryObject = Instantiate(deliveryObjectPrefab, restaurantTransforms[pos].gameObject.transform.position, Quaternion.identity);
        //Instantiate(deliveryObjectPrefab, restaurant.transform.position, Quaternion.identity);
        deliveryObject.name = id.ToString();
        deliveryObject.tag = "order_start";
        Debug.Log("���ɲ͏dӆ��ID::" + id);
        Order_state order_State= deliveryObjectPrefab.GetComponent<Order_state>();
        order_State.orderId = id;
       //FindObjectOfType<CameraController>().StartGuide(0);
    }

    public void InstantiateAtCustomerPosition(int id, string pos)
    {
        GameObject Object = Instantiate(deliveryObjectPrefab, customerTransforms[pos].transform.position, Quaternion.identity);
        //Instantiate(deliveryObjectPrefab, restaurant.transform.position, Quaternion.identity);
        Object.name = id.ToString();
        Object.tag = "order_end";
        //��͵������������
        //Instantiate(deliveryObjectPrefab, customer.transform.position, Quaternion.identity);
        Debug.Log("���ɿ���ӆ��ID::" + id);

        //�����
        //SpawnCustomerAtLocation(cusname, customer.transform.position);
        //FindObjectOfType<CameraController>().StartGuide(1);

    }


    /// <summary>
    /// ��������Q���������Ɍ������ Prefab
    /// </summary>
    /// <param name="customerName">����Q</param>
    /// <param name="position">����λ��</param>
    public void SpawnCustomerAtLocation(string customerName,int id )
    {
        if (customerPrefabs.ContainsKey(customerName))
        {
            Debug.LogWarning("���յ�"+customerName);
            GameObject customerPrefab = customerPrefabs[customerName];
            GameObject pos = GameObject.Find(customerName);
            if (customerPrefab != null && pos!= null)
            {
                // �����
                GameObject newObject = Instantiate(customerPrefab, pos.transform.position, Quaternion.identity);
                newObject.name = id.ToString();
                //Instantiate(customerPrefab, pos.transform.position, Quaternion.identity);
                Debug.Log($"�ɹ������ '{customerName}' ��λ�� {pos.transform.position}");
            }
            else
            {
                Debug.LogWarning($"� '{customerName}' �� Prefab ��δָ����");
            }
        }
        else
        {
            Debug.LogWarning($"�Ҳ������Q�� '{customerName}' ��ͣ�");
        }
    }



}
