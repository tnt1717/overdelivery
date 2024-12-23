using UnityEngine;

public class AdaptiveSpawner : MonoBehaviour
{
    public GameObject prefab; // Ҫ���ɵ� prefab
    public Transform centerPoint; // �����c
    public int numberOfPrefabs = 3; // ���ɵ� prefab ������1-5��
    [Range(1f, 10f)]
    public float spacing = 2f; // prefab �g�ľ��x�����{��

    void Start()
    {
        GeneratePrefabs();
        //CloseAllOtherUI();
        CloseAllOtherUIByLayer();
    }

    public void GeneratePrefabs()
    {
        // �_�����ɔ����� 1 �� 5 ֮�g
        numberOfPrefabs = Mathf.Clamp(numberOfPrefabs, 1, 5);

        // Ӌ�㿂��ȣ��_������ prefab �������c�錦�Qλ��
        float totalWidth = (numberOfPrefabs - 1) * spacing;
        float startX = centerPoint.position.x - totalWidth / 2;

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Ӌ��ÿ�� prefab ��λ��
            Vector3 position = new Vector3(startX + i * spacing, centerPoint.position.y, centerPoint.position.z);

            // ���� prefab �K�O�Þ������c������������x��
            GameObject spawnedPrefab = Instantiate(prefab, position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(centerPoint, true);
        }
    }
    public void CloseAllOtherUIByLayer()
    {
        // �@ȡ��ǰ������ڵ� Layer
        int uiLayer = LayerMask.NameToLayer("UI");

        // ��v���������м�������
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // �z������Ƿ��� UI �ӁK�Ҳ��Ǯ�ǰ���
            if (obj.layer == uiLayer && obj != this.gameObject && obj.activeSelf)
            {
                obj.SetActive(false); // �P�]���� UI
            }
        }
    }

    public void CloseAllOtherUI()
    {
        // �ҵ����������І����е� Canvas ������ UI Ԫ��
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in canvases)
        {
            // ������Ǯ�ǰ������t����
            if (canvas.gameObject != this.gameObject && canvas.gameObject.activeSelf)
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }

    public void UpdatePrefabs(int newCount, float newSpacing)
    {
        // ����f�� prefab
        foreach (Transform child in centerPoint)
        {
            Destroy(child.gameObject);
        }

        // �������ɔ������g��
        numberOfPrefabs = Mathf.Clamp(newCount, 1, 5);
        spacing = newSpacing;

        // ��������
        GeneratePrefabs();
    }
}
