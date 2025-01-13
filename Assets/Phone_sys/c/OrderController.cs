using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 負責生成訂單
/// </summary>
public class OrderController : MonoBehaviour
{
    // 顧客名稱與 Prefab 的映射
    public Dictionary<string, GameObject> customerPrefabs = new Dictionary<string, GameObject>();

    // 初始化映射表（可在 Inspector 中設定）
    [System.Serializable]
    public struct CustomerData
    {
        public string customerName; // 顧客名稱
        public GameObject customerPrefab; // 顧客的 Prefab
    }

    public List<CustomerData> customerDataList = new List<CustomerData>();

    [System.Serializable]
    public class LevelDifficulty
    {
        public float order_time; // 訂單時間
        public List<string> allowedRestaurants; // 允許的餐廳
        public List<string> allowedCustomers; // 允許的顧客
        public int orderCount;
    }

    private float order_time;
    public string cusname;

    public newlist_test listScript; // 引用第一個腳本的實例
    private Transform customer, restaurant;
    // key店家名稱，店家食物列表
    private Dictionary<string, List<string>> restaurantFoods = new Dictionary<string, List<string>>();

    //key店家名稱，店家座標
    private Dictionary<string, Transform> restaurantTransforms = new Dictionary<string, Transform>();

    //key顧客名稱，顧客座標
    private Dictionary<string, Transform> customerTransforms = new Dictionary<string, Transform>();

    private Dictionary<int, LevelDifficulty> levelDifficulty = new Dictionary<int, LevelDifficulty>();

    public GameObject deliveryObjectPrefab;

    private List<string> availableRestaurants; // 儲存當前關卡允許的餐廳
    private List<string> availableCustomers; // 儲存當前關卡允許的顧客

    void Start()
    {

        // 將 List 中的資料初始化到 Dictionary
        foreach (var data in customerDataList)
        {
            if (!customerPrefabs.ContainsKey(data.customerName))
            {
                customerPrefabs.Add(data.customerName, data.customerPrefab);
            }
        }
        InitializeRestaurantData();
        InitializeLevelDifficulty(); // 初始化難度條件

        string sceneName = SceneManager.GetActiveScene().name;
        int levelNumber = ExtractLevelNumber(sceneName);
        Debug.LogWarning ("提取的關卡數字: " + levelNumber);

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
        // 找到字母 "V" 的位置並截取後面的部分
        string numberPart = sceneName.Substring(sceneName.IndexOf('V') + 1);
        return int.Parse(numberPart); // 轉換成整數並返回
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F12))
        {
            GenerateRandomOrder();
        }
    }
    // 初始化餐廳與食物資料
    private void InitializeRestaurantData()
    {
        restaurantFoods.Add("壽司店", new List<string> { "綜合壽司", "綜合生魚片", "鮪魚壽司", "鮭魚壽司", "玉子燒壽司",
            "鮭魚卵手卷","豆皮壽司","黃金鮪魚壽司","鮮蝦蘆筍手捲","綜合炙燒壽司","花壽司" });
        restaurantFoods.Add("居酒屋", new List<string> { "花魚一夜干", "炭烤骰子牛", "醬烤豬肉串", "可樂餅",
            "生啤酒","沙瓦","唐揚雞","炒烏龍麵","鹽烤秋刀魚","明太子雞翅","山藥泥沙拉","鹽烤櫛瓜串"});
        restaurantFoods.Add("丼飯", new List<string> { "牛肉壽喜定食","炸蝦天婦羅定食","蒲燒鰻魚定食","炸豬排定食",
            "鮭魚親子丼","鹽烤鯖魚定食","松坂豚定食","特級三色丼","招牌海鮮丼"});
        restaurantFoods.Add("蛋包飯", new List<string> {
            "番茄蛋包飯",
            "咖哩歐姆蛋包飯",
            "唐揚雞蛋包飯",
            "炸豬排蛋包飯",
            "蘑菇奶油醬蛋包飯",
            "咖哩鮮蝦蛋包飯",
            "紅酒牛肉歐姆蛋包飯",
            "鮮蔬蛋包飯",
            "漢堡排蛋包飯"
        });
        restaurantFoods.Add("咖哩飯", new List<string> {
            "唐揚雞咖哩飯",
            "椰香牛肉綠咖哩飯",
            "炸豬排咖哩飯",
            "菇菇蔬菜咖哩飯",
            "經典雞肉咖哩飯",
            "炸蝦咖哩飯",
            "漢堡排咖哩飯",
            "極辛咖哩飯"
        });



        restaurantTransforms.Add("壽司店", GameObject.Find("壽司店").transform);
        restaurantTransforms.Add("居酒屋", GameObject.Find("居酒屋").transform);
        restaurantTransforms.Add("丼飯", GameObject.Find("丼飯").transform);
        restaurantTransforms.Add("蛋包飯", GameObject.Find("蛋包飯").transform);

        restaurantTransforms.Add("咖哩飯", GameObject.Find("咖哩飯").transform);

        customerTransforms.Add("雞哥", GameObject.Find("雞哥").transform);
        customerTransforms.Add("喵喵", GameObject.Find("喵喵").transform);
        customerTransforms.Add("雞咕", GameObject.Find("雞咕").transform);
        customerTransforms.Add("小梅", GameObject.Find("小梅").transform);
        customerTransforms.Add("小白姐", GameObject.Find("小白姐").transform);
        //customerTransforms.Add("咪咪", GameObject.Find("咪咪").transform);
        customerTransforms.Add("小白妹", GameObject.Find("小白妹").transform);
        //customerTransforms.Add("小浣", GameObject.Find("小浣").transform);
        customerTransforms.Add("小狸", GameObject.Find("小狸").transform);
        //customerTransforms.Add("小梅", GameObject.Find("小梅").transform);
        //customerTransforms.Add("小斑", GameObject.Find("小斑").transform);

    }
    private void InitializeLevelDifficulty()
    {
        levelDifficulty.Add(1, new LevelDifficulty
        {
            order_time = 300f,
            allowedRestaurants = new List<string> { "壽司店" },
            allowedCustomers = new List<string> { "雞哥" },
            orderCount = 1
        });

        levelDifficulty.Add(2, new LevelDifficulty
        {
            order_time = 100f,
            allowedRestaurants = new List<string> { "壽司店", "蛋包飯" },
            allowedCustomers = new List<string> { "喵喵", "雞咕" },
            orderCount = 5
        });

        levelDifficulty.Add(3, new LevelDifficulty
        {
            order_time = 400f,
            allowedRestaurants = new List<string> { "蛋包飯", "咖哩飯" },
            allowedCustomers = new List<string> { "小梅", "小白妹", "小狸" },
            orderCount = 20
        });
        levelDifficulty.Add(4, new LevelDifficulty
        {
            order_time = 400f,
            allowedRestaurants = new List<string> { "丼飯", "咖哩飯","居酒屋" },
            allowedCustomers = new List<string> { "小梅", "小白妹", "小狸" },
            orderCount = 20
        });
        levelDifficulty.Add(5, new LevelDifficulty
        {
            order_time = 400f,
            allowedRestaurants = new List<string> { "丼飯", "咖哩飯", "居酒屋" },
            allowedCustomers = new List<string> { "小梅", "小白妹", "小狸" },
            orderCount = 20
        });
        levelDifficulty.Add(6, new LevelDifficulty
        {
            order_time = 400f,
            allowedRestaurants = new List<string> { "丼飯", "咖哩飯", "居酒屋" },
            allowedCustomers = new List<string> { "小梅", "小白妹", "小狸" },
            orderCount = 20
        });
        // 添加更多關卡設定...
    }

    private void GenerateRandomOrder() //GenerateRandomOrder(string customerName, float orderTime)
    {
        // 確保隨機選擇餐廳和顧客時，僅從當前關卡允許的餐廳和顧客中選擇
        int randomCustomerIndex = Random.Range(0, availableCustomers.Count);
        string selectedCustomer = availableCustomers[randomCustomerIndex];

        int randomRestaurantIndex = Random.Range(0, availableRestaurants.Count);
        string selectedRestaurant = availableRestaurants[randomRestaurantIndex];

        // 隨機選擇餐點
        List<string> foods = restaurantFoods[selectedRestaurant];
        int randomFoodIndex = Random.Range(0, foods.Count);
        string selectedFood = foods[randomFoodIndex];

        Transform restaurantTransform = restaurantTransforms[selectedRestaurant];
        Transform customerTransform = customerTransforms[selectedCustomer];

        // 生成訂單
        listScript.CreateNewItem(order_time, selectedFood, selectedCustomer, selectedRestaurant, listScript.contentPanel1, listScript.itemObjects1);
        Debug.Log($"隨機訂單: {selectedRestaurant}，的:{selectedFood}，顧客: {selectedCustomer} \n 顧客座標: {customerTransform.position}，餐廳座標: {restaurantTransform.position} ");
        //Debug.Log("生成顧客名稱:" + selectedCustomer);
        
        customer = customerTransform;
        restaurant = restaurantTransform;
    }


    // 根據目前關卡設定難度條件
    private void SetLevelConditions(int level)
    {
        if (levelDifficulty.ContainsKey(level))
        {
            LevelDifficulty levelData = levelDifficulty[level];

            // 設定時間
            order_time = levelData.order_time;

            // 設定可用餐廳
            availableRestaurants = new List<string>();
            foreach (var restaurantName in levelData.allowedRestaurants)
            {
                if (restaurantFoods.ContainsKey(restaurantName))
                {
                    availableRestaurants.Add(restaurantName);
                }
            }

            // 設定可用顧客
            availableCustomers = new List<string>();
            foreach (var customerName in levelData.allowedCustomers)
            {
                if (customerTransforms.ContainsKey(customerName))
                {
                    availableCustomers.Add(customerName);
                }
            }

            // 顯示當前關卡設定
            Debug.Log($"關卡 {level} 設定完成: 時間 {order_time}, 餐廳 {string.Join(", ", availableRestaurants)}, 顧客 {string.Join(", ", availableCustomers)}");
        }
        else
        {
            Debug.LogError("無此關卡");
        }
    }
    public void InstantiateAtRestaurantPosition(int id,string pos)
    {
        GameObject deliveryObject = Instantiate(deliveryObjectPrefab, restaurantTransforms[pos].gameObject.transform.position, Quaternion.identity);
        //Instantiate(deliveryObjectPrefab, restaurant.transform.position, Quaternion.identity);
        deliveryObject.name = id.ToString();
        deliveryObject.tag = "order_start";
        Debug.Log("生成餐廳訂單ID::" + id);
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
        //在顧客的座標生成物件
        //Instantiate(deliveryObjectPrefab, customer.transform.position, Quaternion.identity);
        Debug.Log("生成客人訂單ID::" + id);

        //生成顧客
        //SpawnCustomerAtLocation(cusname, customer.transform.position);
        //FindObjectOfType<CameraController>().StartGuide(1);

    }


    /// <summary>
    /// 根據顧客名稱和座標生成對應的顧客 Prefab
    /// </summary>
    /// <param name="customerName">顧客名稱</param>
    /// <param name="position">生成位置</param>
    public void SpawnCustomerAtLocation(string customerName,int id )
    {
        if (customerPrefabs.ContainsKey(customerName))
        {
            Debug.LogWarning("接收到"+customerName);
            GameObject customerPrefab = customerPrefabs[customerName];
            GameObject pos = GameObject.Find(customerName);
            if (customerPrefab != null && pos!= null)
            {
                // 生成顧客
                GameObject newObject = Instantiate(customerPrefab, pos.transform.position, Quaternion.identity);
                newObject.name = id.ToString();
                //Instantiate(customerPrefab, pos.transform.position, Quaternion.identity);
                Debug.Log($"成功生成顧客 '{customerName}' 在位置 {pos.transform.position}");
            }
            else
            {
                Debug.LogWarning($"顧客 '{customerName}' 的 Prefab 尚未指定！");
            }
        }
        else
        {
            Debug.LogWarning($"找不到名稱為 '{customerName}' 的顧客！");
        }
    }



}
