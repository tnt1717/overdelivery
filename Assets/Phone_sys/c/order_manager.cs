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

    public Image foodImage; // ���c�DƬ�M��
    public Image customerImage; // ͈DƬ�M��


    private Dictionary<string, Dictionary<string, string>> restaurantFoods; // �͏d�c���c�����P�S
    private Dictionary<string, string> customerImages; // ͈DƬ�����P�S

    private string currentFoodName = "";
    private string currentCusName = "";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateImages();  // ���DƬ

    }

    public void InitializeOrder(int id, string foodName, string cusName)
    {
        //orderId = id;
        //��ԓ���԰ѕr�gҲ���^��
        currentFoodName = foodName;
        currentCusName = cusName;
        //UpdateImages();  // ���DƬ
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
