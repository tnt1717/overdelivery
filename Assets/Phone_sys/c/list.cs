using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class list : MonoBehaviour
{
    // �o�B�Y���б�
    static public List<string> dataList = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5", "Item 6", "Item 7" };
    static public List<string> deletedDataList = new List<string>(); // ��ńh������Y��

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
            CreateNewItem(i, contentPanel1, itemObjects1, false); // ��contentPanel1��?��?Ŀ
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

            
            Debug.Log("�R���F contentPanel2 �����Ĥ@�Ӫ���");
        }
        else
        {
            Debug.Log("contentPanel2 ���S������i�H�R��");
        }
    }


    void CreateNewItem(int index, GameObject contentPanel, List<GameObject> itemList, bool isDeleted)
    {
        GameObject newItem = Instantiate(itemPrefab, contentPanel.transform);

        // �O���������Ŀ�������фh���Ŀ���@ʾ����
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
        // ȡ�Äh�����Y��
        string deletedData = dataList[index];

        // ���Y�Ϗ�dataList�Єh���K��ӵ�deletedDataList��
        dataList.RemoveAt(index);
        deletedDataList.Add(deletedData);

        // �h��������UI���
        Destroy(itemObjects1[index]);

        // ������б����Ƴ�?UI���
        itemObjects1.RemoveAt(index);

        // �ڵڶ�Scroll View��ʹ����ͬ��itemPrefab
        CreateNewItem(deletedDataList.Count - 1, contentPanel2, itemObjects2, true); 

        RefreshItems();
    }
    void RefreshItems()
    {
        // ��լF�е�UI���
        foreach (GameObject item in itemObjects1)
        {
            Destroy(item);
        }
        itemObjects1.Clear();

        for (int i = 0; i < dataList.Count; i++)
        {
            CreateNewItem(i, contentPanel1, itemObjects1, false); 
            Debug.Log(dataList[i]+"����");
        }
    }
}
