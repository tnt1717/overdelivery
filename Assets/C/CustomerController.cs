using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public GameObject cam; // ������
    //public GameObject camb; // ������

    public GameObject customerPrefab; // ����Prefab
    public GameObject cameraPrefab; // �Á���S͵ĔzӰ�CPrefab
    public Transform pointA; // ���ʼ�cA
    public Transform pointB; // �Ŀ���cB
    public float speed = 3f; // ��Ƅӵ��ٶ�
    public float moveDuration = 2f; // ��Ƅӵĕr�g���Ӯ��r�g��

    public LevelManager levelManager;


    private List<GameObject> activeUIs = new List<GameObject>(); // ӛ䛆����е� UI Ԫ��
    private bool isUIDisabled = false; // ׷ۙ UI �Ƿ��ѽ����P�]
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

    }
    // �@�ǹ��_�ĺ�����׌�����_�������{��
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
        // 1. �P�]���� UI �K�ГQ�������zӰ�C
        DisableAllUI();
        cam.gameObject.SetActive(false);
        cameraPrefab.gameObject.SetActive(true);

        // 2. ����́K�����ƄӄӮ�
        //customerPrefab = Instantiate(customerPrefab, pointA.position, Quaternion.identity);


        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            customerPrefab.transform.position = Vector3.Lerp(pointA.position, pointB.position, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        FindObjectOfType<NPCResponseManager>().ShowNPCResponse(0, 50, 100, 0);


        // 3. �_�����K���_Ŀ��λ�ÁK�h����c�zӰ�C
        //customerPrefab.transform.position = pointB.position;
       // Destroy(customerPrefab);

        // 4. �л����zӰ�C�K߀ԭ���� UI
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
