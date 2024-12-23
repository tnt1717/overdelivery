using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class order_manager : MonoBehaviour
{

    public int orderId;

    public Text order_time;
    public Text Food_Name;
    public Text Cus_Name;
    public Text Shop_Name;
    //public int x;
    public float orderTimer ;
    public GameObject order_state;
    public GameObject btn_state;

    public Image foodImage; // 餐點圖片組件
    public Image customerImage; // 顧客圖片組件


    private Dictionary<string, Dictionary<string, string>> restaurantFoods; // 餐廳與餐點對應關係
    private Dictionary<string, string> customerImages; // 顧客圖片對應關係

    private string currentFoodName = "";
    private string currentCusName = "";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateImages();  // 更新圖片

    }

    public void InitializeOrder(int id, string foodName, string cusName)
    {
        //orderId = id;
        //應該可以把時間也叫過來
        currentFoodName = foodName;
        currentCusName = cusName;
        //UpdateImages();  // 更新圖片
        //Debug.Log(foodName + "+" + cusName);
    }
    private void FixedUpdate()
    {
        if (order_state.tag== "order_start") {
            Debug.Log("timer_start");
            Debug.Log("ID:"+orderId);
            //orderTimer += Time.deltaTime;
            //Debug.Log(orderTimer);
            //order_time.text = 
            //btn_state.SetActive(false); 
        }
    }
    
    
}
