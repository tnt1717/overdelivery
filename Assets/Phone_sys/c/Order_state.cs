using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// ؓ؟�z�y�����ײȡ���c
/// </summary>
public class Order_state : MonoBehaviour
{
    public OrderController orderController; // ����OrderController
    public newlist_test newlist_test;
    public LevelManager levelManager;
    public CustomerController customerController;
    private BagManager bagManager;
    public int orderId; // ӆ�� ID
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
        if(newlist_test.GetOrderStatus(orderId)=="���r") Destroy(gameObject);
               
        orderName = newlist_test.GetCustomerNameByOrderId(orderId);


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);
            if (newlist_test.GetOrderStatus(orderId) != "��ȡ��")
            {
                AudioManager.Instance.PlaySound("order_S");

                newlist_test.UpdateOrderStatus(orderId, "��ȡ��");
                newlist_test.isEndPoint = true;
                orderController.InstantiateAtCustomerPosition(orderId, orderName);
                Debug.Log("����cӆ�����c���|�����ɽK�c��");
                Destroy(this.gameObject);
            }
            else
            {
                

                Debug.Log("����c�K�c���|"+ gameObject.name);
                if (Input.GetKey(KeyCode.F)) {

                    //Debug.Log("����c�K�c���|��ӆ����ɣ� ID:"+gameObject.name);
                    AudioManager.Instance.PlaySound("order_F");
                    //newlist_test.DeletePreviewItem(orderId);
                    //bagManager.RemoveItemFromBagByID(orderId);

                    //�Ƿ������P�ɾ�
                    if (SceneManager.GetActiveScene().name == "LV1")
                    {

                        AchievementUIManager.Instance.ShowAchievement("Achievement1");
                    }

                    //Destroy(this.gameObject);

                }

                //�{ȡԓӆ�����мo䛡�
                //
                //customerController.SpawnCustomerAndMove();


            }
        }
    }
}

