using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ��춙z�y�������Q

[RequireComponent(typeof(Rigidbody))]
public class SpeedCalculator : MonoBehaviour
{
    [Header("�ٶ��@ʾ")]
    private Rigidbody rb;        // ����� Rigidbody
    public float speed;          // ��ǰ�ٶȣ���/�룩
    public float speedKmH;       // ��ǰ�ٶȣ�����/С�r��
    public Text speedText;       // �ٶ��@ʾ�ı�

    [Header("�ͺ��O��")]
    public Image fuelBar;        // �����@ʾ�l��Image Ԫ����
    public float fuelPercentage = 1.0f;  // ��ǰ�����ٷֱ� (1.0 = 100%)
    public float fuelConsumptionRate = 0.2f; // ÿ�ٹ������ĵ����� (20%)
    public float distanceTraveled = 0f;  // ��Ӌ�����x���ף�

    private string currentSceneName;

    static public bool isNearGasStation = false;  // �Ƿ��ڼ���վ������

    private PlayerManager playerManager;     // ��ҹ���������
    public GameObject Gas_ui;
    public Text Coins;

    void Start()
    {
        Gas_ui.gameObject.SetActive(false);
        //PlayerManager playerManager = GameObject.Find("PlayerSys").GetComponent<PlayerManager>();
        GameObject playerSys = GameObject.Find("PlayerSys");
        if (playerSys != null)
        {
            playerManager = playerSys.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("�� 'PlayerSys' ��δ�ҵ� PlayerManager �M����");
                return;
            }
        }
        else
        {
            Debug.LogError("δ�ҵ����Q�� 'PlayerSys' �������");
            return;
        }
        rb = GetComponent<Rigidbody>();
        currentSceneName = SceneManager.GetActiveScene().name; // �@ȡ��ǰ�������Q
    }

    void Update()
    {
        // Ӌ���ٶ�
        speed = rb.velocity.magnitude;
        speedKmH = speed * 3.6f;
        int speedKmHInt = Mathf.RoundToInt(speedKmH);

        // �����ٶ��@ʾ�ı�
        speedText.text = speedKmHInt + " km/h";

        if (Input.GetKey(KeyCode.F10)) f10();

        // Ӌ����������
        UpdateFuel();
        if (isNearGasStation && Input.GetKeyDown(KeyCode.F))
        {
            Refuel();
        }
    }

    void UpdateFuel()
    {
        
       
        //// "LV1" ��������������
        //if (currentSceneName == "LV1")
        //    return;

        // ��Ӌ�����x
        distanceTraveled += speed * Time.deltaTime;

        // ÿ��� 100 ���100,000 �ף������� fuelConsumptionRate ������
        if (distanceTraveled >= 100f)
        {
            fuelPercentage -= fuelConsumptionRate; // �p������
            distanceTraveled = 0f;                 // ������Ӌ���x
        }

        // �_�������� 0% - 100% ������
        fuelPercentage = Mathf.Clamp(fuelPercentage, 0f, 1f);

        // ���������@ʾ�l
        if (fuelBar != null)
        {
            fuelBar.fillAmount = fuelPercentage;
        }

        // ��������w�㣬�|�l�~���О飨��ֹͣ�Ƅӻ����¼��ͣ�
        if (fuelPercentage <= 0f)
        {
            OnFuelEmpty();
        }
    }

    void OnFuelEmpty()
    {
        // �������w��r���е��О�
        Debug.Log("Fuel is empty! Vehicle cannot move.");
        rb.velocity = Vector3.zero; // ֹͣ����Ƅ�
    }
    void Refuel()
    {
        // �z����ҽ����Ƿ����
        if (playerManager != null && playerManager.playerData.coins >= 200)
        {
            // ���Ľ��ŁK�������ӝM
            playerManager.playerData.coins -= 200;
            fuelPercentage = 1.0f;

            // ���� UI
            if (fuelBar != null)
            {
                fuelBar.fillAmount = fuelPercentage;
            }

            Debug.Log("Refueled! Coins left: " + playerManager.playerData.coins);
        }
        else
        {
            Debug.Log("Not enough coins to refuel.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GAS"))
        {
            isNearGasStation = true;
            Gas_ui.gameObject.SetActive(true);
            Coins.text = playerManager.playerData.coins.ToString();
            Debug.Log("Near gas station. Press F to refuel.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GAS"))
        {
            isNearGasStation = false;
            Gas_ui.gameObject.SetActive(false);

            Debug.Log("Left gas station.");
        }
    }
    public void f10() {
        fuelPercentage = 1.0f;
        if (fuelBar != null)
        {
            fuelBar.fillAmount = fuelPercentage;
        }

    }
}
