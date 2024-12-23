using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 負責檢測玩家碰撞取餐點
/// </summary>
public class Order_state : MonoBehaviour
{
    public OrderController orderController; // 引用OrderController
    public newlist_test newlist_test;
    public LevelManager levelManager;
    public CustomerController customerController;
    private BagManager bagManager;
    public int orderId; // 訂單 ID
    public string orderName;    

    void Start()
    {
        bagManager = FindObjectOfType<BagManager>();
        orderController = FindObjectOfType<OrderController>();
        newlist_test = FindObjectOfType<newlist_test>();
        levelManager = FindObjectOfType<LevelManager>();
        customerController = FindObjectOfType<CustomerController>();
        orderId = int.Parse(gameObject.name);
        
    }
    private void Update()
    {
        if(newlist_test.GetOrderStatus(orderId)=="超時") Destroy(gameObject);
               
        orderName = newlist_test.GetCustomerNameByOrderId(orderId);


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);
            if (newlist_test.GetOrderStatus(orderId) != "已取餐")
            {
                AudioManager.Instance.PlaySound("order_S");

                newlist_test.UpdateOrderStatus(orderId, "已取餐");
                newlist_test.isEndPoint = true;
                orderController.InstantiateAtCustomerPosition(orderId, orderName);
                Debug.Log("玩家與訂單起點接觸，生成終點。");
                Destroy(this.gameObject);
            }
            else
            {
                

                Debug.Log("玩家與終點接觸"+ gameObject.name);
                if (Input.GetKey(KeyCode.F)) {

                    //Debug.Log("玩家與終點接觸，訂單完成！ ID:"+gameObject.name);
                    AudioManager.Instance.PlaySound("order_F");
                    //newlist_test.DeletePreviewItem(orderId);
                    //bagManager.RemoveItemFromBagByID(orderId);

                    //是否有相關成就
                    if (SceneManager.GetActiveScene().name == "LV1")
                    {

                        AchievementUIManager.Instance.ShowAchievement("Achievement1");
                    }

                    //Destroy(this.gameObject);

                }

                //調取該訂單所有紀錄。
                //
                //customerController.SpawnCustomerAndMove();


            }
        }
    }
}

