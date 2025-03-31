using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public GameObject cam; // 玩家物件
    //public GameObject camb; // 玩家物件

    public GameObject customerPrefab; // 顧客物件Prefab
    public GameObject cameraPrefab; // 用來跟隨顧客的攝影機Prefab
    public Transform pointA; // 顧客起始點A
    public Transform pointB; // 顧客目標點B
    public float speed = 3f; // 顧客移動的速度
    public float moveDuration = 2f; // 顧客移動的時間（動畫時間）

    public LevelManager levelManager;


    private List<GameObject> activeUIs = new List<GameObject>(); // 記錄啟用中的 UI 元素
    private bool isUIDisabled = false; // 追蹤 UI 是否已經被關閉
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

    }
    // 這是公開的函數，讓其他腳本可以調用
    public void SpawnCustomerAndMove()
    {
        StartCoroutine(HandleCustomerSequence());
    }
    public void Update()
    {
        if(Input.GetKey(KeyCode.G))
        FindObjectOfType<NPCResponseManager>().ShowNPCResponse(0, 50, 100, 0);
    }

    private IEnumerator HandleCustomerSequence()
    {
        // 1. 關閉所有 UI 並切換到引導攝影機
        DisableAllUI();
        cam.gameObject.SetActive(false);
        cameraPrefab.gameObject.SetActive(true);

        // 2. 生成顧客並啟動移動動畫
        //customerPrefab = Instantiate(customerPrefab, pointA.position, Quaternion.identity);


        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            customerPrefab.transform.position = Vector3.Lerp(pointA.position, pointB.position, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        FindObjectOfType<NPCResponseManager>().ShowNPCResponse(0, 50, 100, 0);


        // 3. 確保顧客最終到達目標位置並刪除顧客與攝影機
        //customerPrefab.transform.position = pointB.position;
       // Destroy(customerPrefab);

        // 4. 切回主攝影機並還原所有 UI
        cameraPrefab.gameObject.SetActive(false);

        cam.gameObject.SetActive(true);
        RestoreUI();
        

    }
    private void DisableAllUI()
    {
        if (isUIDisabled) return;

        activeUIs.Clear();
        Graphic[] graphics = FindObjectsOfType<Graphic>();
        foreach (Graphic graphic in graphics)
        {
            if (graphic.gameObject.activeSelf)
            {
                activeUIs.Add(graphic.gameObject);
                graphic.gameObject.SetActive(false);
            }
        }
        isUIDisabled = true;
    }

    private void RestoreUI()
    {
        foreach (GameObject ui in activeUIs)
        {
            if (ui != null)
            {
                ui.SetActive(true);
            }
        }
        activeUIs.Clear();
        isUIDisabled = false;
    }

}
