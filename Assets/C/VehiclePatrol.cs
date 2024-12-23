using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehiclePatrol : MonoBehaviour
{
    public Transform[] corners; // �Ă������λ��
    public GameObject[] vehiclePrefabs; // ܇�v���A�u�w���
    public Transform vehicleParent; // ܇�v��ĸ���
    public Transform player; // ���
    public float spawnInterval = 7f; // ܇�v�����g��
    public float deleteDistance = 25f; // ܇�v�h���ľ��x

    private List<GameObject> activeVehicles = new List<GameObject>(); // ��ǰ���ڵ�܇�v
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
        // �S�C�x��һ�l߅
        int edgeIndex = Random.Range(0, 4);
        Transform startCorner = corners[edgeIndex];
        Transform endCorner = corners[(edgeIndex + 1) % 4];

        // ��ԓ߅���S�C�x��һ��λ��
        float t = Random.Range(0f, 1f);
        Vector3 spawnPosition = Vector3.Lerp(startCorner.position, endCorner.position, t);

        // �S�C�x��һ��܇�vģ��
        GameObject selectedVehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];

        // ������܇�v�K�O�Þ�ĸ����������
        GameObject vehicle = Instantiate(selectedVehiclePrefab, spawnPosition, Quaternion.identity, vehicleParent);

        // ��ʼ��܇�v��Ŀ���c
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

            // �_��܇�v�����ѽ�Ѳ߉��������
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