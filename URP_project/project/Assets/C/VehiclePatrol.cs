using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehiclePatrol : MonoBehaviour
{
    public Transform[] corners; // 四個角落的位置
    public GameObject[] vehiclePrefabs; // 車輛的預製體陣列
    public Transform vehicleParent; // 車輛的母物件
    public Transform player; // 玩家
    public float spawnInterval = 7f; // 車輛生成間隔
    public float deleteDistance = 25f; // 車輛刪除的距離

    private List<GameObject> activeVehicles = new List<GameObject>(); // 當前存在的車輛
    public int cars = 0;

    void Start()
    {
        StartCoroutine(SpawnVehicleRoutine());
    }

    void Update()
    {
        CheckVehicleDistance();
    }

    IEnumerator SpawnVehicleRoutine()
    {
        while (true)
        {
            if (cars <= 10) { 
            SpawnVehicle();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnVehicle()
    {
        // 隨機選擇一條邊
        int edgeIndex = Random.Range(0, 4);
        Transform startCorner = corners[edgeIndex];
        Transform endCorner = corners[(edgeIndex + 1) % 4];

        // 在該邊上隨機選擇一個位置
        float t = Random.Range(0f, 1f);
        Vector3 spawnPosition = Vector3.Lerp(startCorner.position, endCorner.position, t);

        // 隨機選擇一個車輛模型
        GameObject selectedVehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];

        // 實例化車輛並設置為母物件的子物件
        GameObject vehicle = Instantiate(selectedVehiclePrefab, spawnPosition, Quaternion.identity, vehicleParent);

        // 初始化車輛的目標點
        VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
        if (vehicleController != null)
        {
            vehicleController.Initialize(corners, edgeIndex);
        }

        activeVehicles.Add(vehicle);
        cars += 1;
    }

    void CheckVehicleDistance()
    {
        for (int i = activeVehicles.Count - 1; i >= 0; i--)
        {
            GameObject vehicle = activeVehicles[i];
            VehicleController vehicleController = vehicle.GetComponent<VehicleController>();

            // 確保車輛至少已經巡邏三個角落
            if (vehicleController != null && vehicleController.CornerVisitCount >= 2)
            {
                if (Vector3.Distance(vehicle.transform.position, player.position) > deleteDistance)
                {
                    Destroy(vehicle);
                    activeVehicles.RemoveAt(i);
                    cars -= 1;
                }
            }
        }
    }
}