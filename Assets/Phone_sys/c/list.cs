using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class list : MonoBehaviour
{
    // 靜態資料列表
    static public List<string> dataList = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5", "Item 6", "Item 7" };
    static public List<string> deletedDataList = new List<string>(); // 存放刪除後的資料

    static public bool order_start =true;
    
    public GameObject contentPanel1; 
    public GameObject contentPanel2; 
    public GameObject itemPrefab;    

    private List<GameObject> itemObjects1 = new List<GameObject>(); 
    private List<GameObject> itemObjects2 = new List<GameObject>(); 

    void Start()
    {
        
        for (int i = 0; i < dataList.Count; i++)
        {
            CreateNewItem(i, contentPanel1, itemObjects1, false); // 在contentPanel1中?建?目
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {

            DeleteFirstItemFromPanel2();
        }
    }

    void DeleteFirstItemFromPanel2()
    {
        if (itemObjects2.Count > 0) 
        {
            int indexToDelete = 0;

            Destroy(itemObjects2[indexToDelete]);

            itemObjects2.RemoveAt(indexToDelete);

            if (deletedDataList.Count > 0)
            {
                deletedDataList.RemoveAt(indexToDelete);
            }

            
            Debug.Log("埃 contentPanel2 い材ン");
        }
        else
        {
            Debug.Log("contentPanel2 い⊿Τン埃");
        }
    }


    void CreateNewItem(int index, GameObject contentPanel, List<GameObject> itemList, bool isDeleted)
    {
        GameObject newItem = Instantiate(itemPrefab, contentPanel.transform);

        // 設置其他的項目，比如已刪除項目的顯示文字
        newItem.GetComponentInChildren<Text>().text = isDeleted ? deletedDataList[index] : dataList[index];

       
        itemList.Add(newItem);

        if (isDeleted)
        {
            newItem.tag = "order_start";
        }

        if (!isDeleted)
        {

            Button deleteButton = newItem.GetComponentInChildren<Button>();
            deleteButton.onClick.AddListener(() => DeleteItem(index));
        }
    }

    void DeleteItem(int index)
    {
        Debug.Log(dataList[index]);
        // 取得刪除的資料
        string deletedData = dataList[index];

        // 將資料從dataList中刪除並添加到deletedDataList中
        dataList.RemoveAt(index);
        deletedDataList.Add(deletedData);

        // 刪除對應的UI物件
        Destroy(itemObjects1[index]);

        // 從物件列表中移除?UI物件
        itemObjects1.RemoveAt(index);

        // 在第二Scroll View，使用相同的itemPrefab
        CreateNewItem(deletedDataList.Count - 1, contentPanel2, itemObjects2, true); 

        RefreshItems();
    }
    void RefreshItems()
    {
        // 清空現有的UI物件
        foreach (GameObject item in itemObjects1)
        {
            Destroy(item);
        }
        itemObjects1.Clear();

        for (int i = 0; i < dataList.Count; i++)
        {
            CreateNewItem(i, contentPanel1, itemObjects1, false); 
            Debug.Log(dataList[i]+"生成");
        }
    }
}
