using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order
{
    public int Id;
    public float OrderTime;
    public string FoodName;
    public string CusName;
    public string ShopName;
    public GameObject UIItem; // êPÂ“µÄ UI ÔªËØ

    public Order(int id, float orderTime, string foodName, string cusName, string shopName)
    {
        Id = id;
        OrderTime = orderTime;
        FoodName = foodName;
        CusName = cusName;
        ShopName = shopName;
    }
}

public class newlist_n2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
