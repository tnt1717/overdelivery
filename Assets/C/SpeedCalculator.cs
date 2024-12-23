using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 用於檢測場景名稱

[RequireComponent(typeof(Rigidbody))]
public class SpeedCalculator : MonoBehaviour
{
    [Header("速度顯示")]
    private Rigidbody rb;        // 物件的 Rigidbody
    public float speed;          // 當前速度（米/秒）
    public float speedKmH;       // 當前速度（公里/小時）
    public Text speedText;       // 速度顯示文本

    [Header("油耗設定")]
    public Image fuelBar;        // 油量顯示條（Image 元件）
    public float fuelPercentage = 1.0f;  // 當前油量百分比 (1.0 = 100%)
    public float fuelConsumptionRate = 0.2f; // 每百公里消耗的油量 (20%)
    public float distanceTraveled = 0f;  // 累計行駛距離（米）

    private string currentSceneName;

    static public bool isNearGasStation = false;  // 是否在加油站範圍內

    private PlayerManager playerManager;     // 玩家管理器引用
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
                Debug.LogError("在 'PlayerSys' 上未找到 PlayerManager 組件！");
                return;
            }
        }
        else
        {
            Debug.LogError("未找到名稱為 'PlayerSys' 的物件！");
            return;
        }
        rb = GetComponent<Rigidbody>();
        currentSceneName = SceneManager.GetActiveScene().name; // 獲取當前場景名稱
    }

    void Update()
    {
        // 計算速度
        speed = rb.velocity.magnitude;
        speedKmH = speed * 3.6f;
        int speedKmHInt = Mathf.RoundToInt(speedKmH);

        // 更新速度顯示文本
        speedText.text = speedKmHInt + " km/h";

        if (Input.GetKey(KeyCode.F10)) f10();

        // 計算油量消耗
        UpdateFuel();
        if (isNearGasStation && Input.GetKeyDown(KeyCode.F))
        {
            Refuel();
        }
    }

    void UpdateFuel()
    {
        
       
        //// "LV1" 場景不消耗油量
        //if (currentSceneName == "LV1")
        //    return;

        // 累計行駛距離
        distanceTraveled += speed * Time.deltaTime;

        // 每行駛 100 公里（100,000 米），消耗 fuelConsumptionRate 的油量
        if (distanceTraveled >= 100f)
        {
            fuelPercentage -= fuelConsumptionRate; // 減少油量
            distanceTraveled = 0f;                 // 重置累計距離
        }

        // 確保油量在 0% - 100% 範圍內
        fuelPercentage = Mathf.Clamp(fuelPercentage, 0f, 1f);

        // 更新油量顯示條
        if (fuelBar != null)
        {
            fuelBar.fillAmount = fuelPercentage;
        }

        // 如果油量歸零，觸發額外行為（如停止移動或重新加油）
        if (fuelPercentage <= 0f)
        {
            OnFuelEmpty();
        }
    }

    void OnFuelEmpty()
    {
        // 當油量歸零時執行的行為
        Debug.Log("Fuel is empty! Vehicle cannot move.");
        rb.velocity = Vector3.zero; // 停止物件移動
    }
    void Refuel()
    {
        // 檢查玩家金幣是否足夠
        if (playerManager != null && playerManager.playerData.coins >= 200)
        {
            // 消耗金幣並將油量加滿
            playerManager.playerData.coins -= 200;
            fuelPercentage = 1.0f;

            // 更新 UI
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
