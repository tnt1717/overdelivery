using UnityEngine;

public class AdaptiveSpawner : MonoBehaviour
{
    public GameObject prefab; // 要生成的 prefab
    public Transform centerPoint; // 中心點
    public int numberOfPrefabs = 3; // 生成的 prefab 數量（1-5）
    [Range(1f, 10f)]
    public float spacing = 2f; // prefab 間的距離（可調）

    void Start()
    {
        GeneratePrefabs();
        //CloseAllOtherUI();
        CloseAllOtherUIByLayer();
    }

    public void GeneratePrefabs()
    {
        // 確保生成數量在 1 到 5 之間
        numberOfPrefabs = Mathf.Clamp(numberOfPrefabs, 1, 5);

        // 計算總跨度，確保所有 prefab 以中心點為對稱位置
        float totalWidth = (numberOfPrefabs - 1) * spacing;
        float startX = centerPoint.position.x - totalWidth / 2;

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // 計算每個 prefab 的位置
            Vector3 position = new Vector3(startX + i * spacing, centerPoint.position.y, centerPoint.position.z);

            // 生成 prefab 並設置為中心點的子物件（可選）
            GameObject spawnedPrefab = Instantiate(prefab, position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(centerPoint, true);
        }
    }
    public void CloseAllOtherUIByLayer()
    {
        // 獲取當前物件所在的 Layer
        int uiLayer = LayerMask.NameToLayer("UI");

        // 遍歷場景中所有激活的物件
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // 檢查物件是否在 UI 層並且不是當前物件
            if (obj.layer == uiLayer && obj != this.gameObject && obj.activeSelf)
            {
                obj.SetActive(false); // 關閉其他 UI
            }
        }
    }

    public void CloseAllOtherUI()
    {
        // 找到場景中所有啟用中的 Canvas 或其他 UI 元件
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in canvases)
        {
            // 如果不是當前物件，則禁用
            if (canvas.gameObject != this.gameObject && canvas.gameObject.activeSelf)
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }

    public void UpdatePrefabs(int newCount, float newSpacing)
    {
        // 清空舊的 prefab
        foreach (Transform child in centerPoint)
        {
            Destroy(child.gameObject);
        }

        // 更新生成數量和間距
        numberOfPrefabs = Mathf.Clamp(newCount, 1, 5);
        spacing = newSpacing;

        // 重新生成
        GeneratePrefabs();
    }
}
