using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [System.Serializable]
    
    public class Vehicle
    {
        public string vehicleName;       // ܇�v���Q�����x��
        public Mesh headMesh;            // ܇�^�� Mesh
        public Mesh bodyMesh;            // ܇��� Mesh
        public Material material;
    }
    public Vehicle[] vehicles;          // ܇�v�Y�����
    public MeshFilter headMeshFilter;   // ܇�^�� MeshFilter
    public MeshFilter bodyMeshFilter;   // ܇��� MeshFilter
    public GameObject headmaterialmain;
    public GameObject bodymaterialmain;
    private PlayerManager playerManager; // �B�Y PlayerManager ��춴�ȡ PlayerData


    private void Start()
    {
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

        
        InitializeVehicle();
    }
    private void InitializeVehicle()
    {
        string currentVehicleName = playerManager.playerData.currentVehicle;

        // �ҵ�������܇�v�Y��
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
            // ���d܇�v Mesh
            LoadVehicleMesh(selectedVehicle);
           // Debug.Log($"�ѳ�ʼ��܇�v��{selectedVehicle.vehicleName}");
        }
        else
        {
            Debug.LogError($"δ�ҵ����Q�� {currentVehicleName} ��܇�v��Ո�_�J�Y���Ƿ����_��");
        }
    }

    private void LoadVehicleMesh(Vehicle vehicle)
    {
        // ���� MeshFilter �� mesh
        Renderer renderera = headmaterialmain.GetComponent<Renderer>();
        Renderer rendererb = bodymaterialmain.GetComponent<Renderer>();

        headMeshFilter.mesh = vehicle.headMesh;
        bodyMeshFilter.mesh = vehicle.bodyMesh;
        renderera.material = vehicle.material;
        rendererb.material = vehicle.material;


        //Debug.Log($"܇�^��܇�����ГQ�飺{vehicle.vehicleName}");
    }

    public void UpdateVehicle(string newVehicleName)
    {
        // ���� PlayerData ��܇�v���Q
        playerManager.playerData.currentVehicle = newVehicleName;

        // �ٴ��ҵ��K���d������܇�v�Y��
        InitializeVehicle();

        // ���攵�������O�б��湦�ܣ�
        SavePlayerData();
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetString("currentVehicle", playerManager.playerData.currentVehicle);
        PlayerPrefs.Save();
        Debug.Log("���܇�v�����ѱ��棡");
    }
}
