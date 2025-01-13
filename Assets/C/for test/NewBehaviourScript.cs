using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [System.Serializable]
    
    public class Vehicle
    {
        public string vehicleName;       // 車輛名稱（可選）
        public Mesh headMesh;            // 車頭的 Mesh
        public Mesh bodyMesh;            // 車身的 Mesh
        public Material material;
    }
    public Vehicle[] vehicles;          // 車輛資料陣列
    public MeshFilter headMeshFilter;   // 車頭的 MeshFilter
    public MeshFilter bodyMeshFilter;   // 車身的 MeshFilter
    public GameObject headmaterialmain;
    public GameObject bodymaterialmain;
    private PlayerManager playerManager; // 連結 PlayerManager 用於存取 PlayerData


    private void Start()
    {
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

        
        InitializeVehicle();
    }
    private void InitializeVehicle()
    {
        string currentVehicleName = playerManager.playerData.currentVehicle;

        // 找到對應的車輛資料
        Vehicle selectedVehicle = null;
        foreach (var vehicle in vehicles)
        {
            if (vehicle.vehicleName == currentVehicleName)
            {
                selectedVehicle = vehicle;
                break;
            }
        }

        if (selectedVehicle != null)
        {
            // 加載車輛 Mesh
            LoadVehicleMesh(selectedVehicle);
           // Debug.Log($"已初始化車輛：{selectedVehicle.vehicleName}");
        }
        else
        {
            Debug.LogError($"未找到名稱為 {currentVehicleName} 的車輛！請確認資料是否正確。");
        }
    }

    private void LoadVehicleMesh(Vehicle vehicle)
    {
        // 更新 MeshFilter 的 mesh
        Renderer renderera = headmaterialmain.GetComponent<Renderer>();
        Renderer rendererb = bodymaterialmain.GetComponent<Renderer>();

        headMeshFilter.mesh = vehicle.headMesh;
        bodyMeshFilter.mesh = vehicle.bodyMesh;
        renderera.material = vehicle.material;
        rendererb.material = vehicle.material;


        //Debug.Log($"車頭和車身已切換為：{vehicle.vehicleName}");
    }

    public void UpdateVehicle(string newVehicleName)
    {
        // 更新 PlayerData 的車輛名稱
        playerManager.playerData.currentVehicle = newVehicleName;

        // 再次找到並加載對應的車輛資料
        InitializeVehicle();

        // 保存數據（假設有保存功能）
        SavePlayerData();
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetString("currentVehicle", playerManager.playerData.currentVehicle);
        PlayerPrefs.Save();
        Debug.Log("玩家車輛數據已保存！");
    }
}
