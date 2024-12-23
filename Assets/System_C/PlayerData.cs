using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//GameObject playerSys = GameObject.Find("PlayerSys");
//if (playerSys != null)
//{
//    playerManager = playerSys.GetComponent<PlayerManager>();
//    if (playerManager == null)
//    {
//        Debug.LogError("�� 'PlayerSys' ��δ�ҵ� PlayerManager �M����");
//        return;
//    }
//}
//else
//{
//    Debug.LogError("δ�ҵ����Q�� 'PlayerSys' �������");
//    return;
//}


[System.Serializable]
public class PlayerData 
{
    public int coins=1000;
    public Dictionary<string, bool> achievements; // �ɾ����Q�ͽ��i��B

    // ���b�Y��
    public Dictionary<string, bool> outfits; // ���b���Q�͓��Р�B
    public List<string> unlockedClothings; // �ѽ��i���b���Q


    // �����Y��
    public Dictionary<string, bool> expressions; // �������Q�ͽ��i��B
    public List<string> unlockedExpressions; // �ѽ��i�������Q


    public Dictionary<string, int> levelStars; // �P�����Q�����ǔ���
    public Dictionary<string, int> itemLevels; // ��Ʒ���Q�͵ȼ�

    public bool isCharacterCreated = false;
    public string Playermodle,PlayerName;

    //public Dictionary<string, string> vehicleStatus = new Dictionary<string, string>(); // ܇�v��B

    public Dictionary<string, bool> vehicleStates = new Dictionary<string, bool>(); 
    public string currentVehicle; //ʹ���е�܇�v
    public List<int> unlockedVehicles;         // �ѽ��i܇�v����
    public List<int> unlockedTextures;         



    public string currentClothing; // ��ǰʹ���еķ��b���Q
    //ȱ���ѽ��i��������
    //ȱ���ѽ��i���b����
    public string currentExpression; // ��ǰʹ���еı������Q

    public float Volume;
    public float MusicVolume;

    public PlayerData()
    {
        coins = 1000;
        Volume = 0.5f;
        MusicVolume = 0.5f;
        achievements = new Dictionary<string, bool>
        {
            { "Achievement1", false },
            { "Achievement2", false},
            { "Achievement3", false },
            { "Achievement4", false },
            { "Achievement5", false },
            // �����^�m��������ɾ�...
        };

        outfits = new Dictionary<string, bool>
        {
            { "a", false },
            { "b", false },
            // �����^�m����������b...
        };
        unlockedClothings = new List<string>();
        currentClothing = null;
        // ��ʼ������
        expressions = new Dictionary<string, bool>
        {
            { "Expression1", false },
            { "Expression2", false },
        };
        unlockedExpressions = new List<string>();
        currentExpression = null;


        levelStars = new Dictionary<string, int>
        {
            { "LV1", 1},
            { "LV2", 0},
            { "LV3", 0 },
            { "LV4", 0},
            { "LV5",0},
            { "LV6",0}



        };
        itemLevels = new Dictionary<string, int>();
        { 
        
        
        };

        Debug.Log("PlayerData initialized with default values.");
        itemLevels = new Dictionary<string, int>();
        unlockedVehicles = new List<int>();
        //currentVehicle = -1; // �A�O��o܇�v
    }

}
