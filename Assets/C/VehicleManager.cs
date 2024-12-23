using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleManager : MonoBehaviour
{
    [System.Serializable]
    public class Vehicle
    {
        public string vehicleName;  // ܇�v���Q
        public int price;           // ܇�v�r��
        public GameObject vehicleObject; // ܇�v���������
    }
    private PlayerManager playerManager; // �B�Y PlayerManager ��춴�ȡ PlayerData

    public Vehicle[] vehicles;      // ����܇�v�����
    public Text buttonText;         // ���o�ϵ�����
    public Button actionButton;     // ����ُ�I���ГQ�İ��o
    public Text playerCoinsText;    // �@ʾ��ҽ��ŵ��ı�
    private int currentIndex = 0;   // ��ǰ�x�е�܇�v����
    //private PlayerData playerData;

    //public PlayerData playerData;   // ��Ҕ������
    private string currentVehicleKey = "currentVehicle"; // ʹ���е�܇�v�惦�I

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

        
        LoadPlayerData(); // ��ʼ����Ҕ���
        SavePlayerData(); // �_��Ĭ�J��B������
        UpdateVehicleUI(); // ����UI
    }

    private void LoadPlayerData()
    {
        // �_��ÿ�v܇���Р�B
        foreach (var vehicle in vehicles)
        {
            if (!playerManager.playerData.vehicleStates.ContainsKey(vehicle.vehicleName))
            {
                playerManager.playerData.vehicleStates[vehicle.vehicleName] = false; // Ĭ�J��δ����
            }
        }

        // ���δ�O�î�ǰ܇�v�����һ�v܇��B�����_���������һ�v܇
        var defaultVehicle = vehicles[0]; // �A�O���һ�v܇
        if (string.IsNullOrEmpty(playerManager.playerData.currentVehicle) ||
            !playerManager.playerData.vehicleStates[defaultVehicle.vehicleName])
        {
            playerManager.playerData.currentVehicle = defaultVehicle.vehicleName;
            playerManager.playerData.vehicleStates[defaultVehicle.vehicleName] = true; // �O�Þ��ѓ���

            Debug.Log($"������һ�v܇ {defaultVehicle.vehicleName} ���A�O܇�v�Ҡ�B���ѓ���");
        }
    }

    public void OnLeftButton()
    {
        currentIndex = (currentIndex - 1 + vehicles.Length) % vehicles.Length;
        Debug.Log($"�ГQ����߅܇�v��������{currentIndex}");
        UpdateVehicleUI();
    }

    public void OnRightButton()
    {
        currentIndex = (currentIndex + 1) % vehicles.Length;
        Debug.Log($"�ГQ����߅܇�v��������{currentIndex}");
        UpdateVehicleUI();
    }

    private void UpdateVehicleUI()
    {
        // �[������܇�v���
        foreach (var vehicle in vehicles)
        {
            vehicle.vehicleObject.SetActive(false);
        }

        // �@ʾ��ǰ�x��܇�v
        vehicles[currentIndex].vehicleObject.SetActive(true);

        // ���°��o��B
        var currentVehicle = vehicles[currentIndex];
        bool isOwned = playerManager.playerData.vehicleStates[currentVehicle.vehicleName];

        Debug.Log($"��ǰ�x��܇�v��{currentVehicle.vehicleName}���Ƿ��ѓ��У�{isOwned}");

        if (isOwned)
        {
            if (playerManager.playerData.currentVehicle == currentVehicle.vehicleName)
            {
                buttonText.text = "ʹ����";
                actionButton.interactable = false;
                Debug.Log($"{currentVehicle.vehicleName} ����ʹ����");
            }
            else
            {
                buttonText.text = "δʹ��";
                actionButton.interactable = true;
                Debug.Log($"{currentVehicle.vehicleName} �ѓ��е�δʹ��");
            }
        }
        else
        {
            buttonText.text = $"�r��: {currentVehicle.price}";
            actionButton.interactable = playerManager.playerData.coins >= currentVehicle.price;
            Debug.Log($"{currentVehicle.vehicleName} δ���У��r��� {currentVehicle.price}");
        }

        // ������ҽ����@ʾ
        playerCoinsText.text = $"����: {playerManager.playerData.coins}";
    }
    public void OnActionButton()
    {
        var currentVehicle = vehicles[currentIndex];
        bool isOwned = playerManager.playerData.vehicleStates[currentVehicle.vehicleName];

        if (isOwned)
        {
            // �ГQ�鮔ǰ܇�v
            playerManager.playerData.currentVehicle = currentVehicle.vehicleName;
            Debug.Log($"�ГQ܇�v�飺{currentVehicle.vehicleName}");

        }
        else
        {
            //int currentCoins = playerManager.playerData.coins;
            // ُ�I܇�v
            if (playerManager.playerData.coins >= currentVehicle.price)
            {
                playerManager.playerData.coins -= currentVehicle.price;
                playerManager.playerData.vehicleStates[currentVehicle.vehicleName] = true;
                playerManager.playerData.currentVehicle = currentVehicle.vehicleName;

                Debug.Log($"ُ�I܇�v {currentVehicle.vehicleName} �ɹ���ʣ�N���ţ�{playerManager.playerData.coins}");
            }
            else
            {
                Debug.Log($"ُ�Iʧ�������Ų��㣡��Ҫ��{currentVehicle.price}����ǰ���У�{playerManager.playerData.coins}");
                return;
            }
        }

        // ����UI�K���攵��
        UpdateVehicleUI();
        SavePlayerData();
    }

    private void SavePlayerData()
    {
        // ���F��������߉݋������ʹ��PlayerPrefs���������淽ʽ
        PlayerPrefs.SetInt("playerCoins", playerManager.playerData.coins);
        PlayerPrefs.SetString(currentVehicleKey, playerManager.playerData.currentVehicle);

        foreach (var state in playerManager.playerData.vehicleStates)
        {
            PlayerPrefs.SetInt(state.Key, state.Value ? 1 : 0);
        }

        PlayerPrefs.Save();
        Debug.Log("��Ҕ����ѱ���");
    }

    private void LoadPlayerDataFromPrefs()
    {
        playerManager.playerData.coins = PlayerPrefs.GetInt("playerCoins", 1000);
        playerManager.playerData.currentVehicle = PlayerPrefs.GetString(currentVehicleKey, vehicles[0].vehicleName);

        foreach (var vehicle in vehicles)
        {
            bool isOwned = PlayerPrefs.GetInt(vehicle.vehicleName, 0) == 1;
            playerManager.playerData.vehicleStates[vehicle.vehicleName] = isOwned;
        }

        // �_����һ�v܇���ѓ���
        var defaultVehicle = vehicles[0];
        if (!playerManager.playerData.vehicleStates[defaultVehicle.vehicleName])
        {
            playerManager.playerData.vehicleStates[defaultVehicle.vehicleName] = true;
            playerManager.playerData.currentVehicle = defaultVehicle.vehicleName;
            Debug.Log($"���d������������һ�v܇ {defaultVehicle.vehicleName} ���ѓ���");
        }
    }
}



