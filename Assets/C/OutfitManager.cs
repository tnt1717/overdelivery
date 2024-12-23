using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitManager : MonoBehaviour
{
    [System.Serializable]
    public class Outfit
    {
        public string outfitName;     // ���b���Q
        public Mesh outfitMesh;       // ���b�� Mesh
        public Material material;     // ���b�Ĳ��|��ᘌ� Element1��
    }

    public Outfit[] outfits;            // ���з��b�Y��
    //public MeshFilter bodyMeshFilter;   // ��ҷ��b�� MeshFilter
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject bodyMaterialMain; // ��ҷ��b�Ĳ��|�������

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

        InitializeOutfit();
    }
    private void Update()
    {
        InitializeOutfit();
    }

    private void InitializeOutfit()
    {
        string currentOutfitName = playerManager.playerData.currentClothing;

        // �ҵ������ķ��b�Y��
        Outfit selectedOutfit = null;
        foreach (var outfit in outfits)
        {
            if (outfit.outfitName == currentOutfitName)
            {
                selectedOutfit = outfit;
                break;
            }
        }

        if (selectedOutfit != null)
        {
            // ���d���b Mesh �Ͳ��|
            LoadOutfit(selectedOutfit);
            Debug.Log($"�ѳ�ʼ�����b��{selectedOutfit.outfitName}");
        }
        else
        {
            Debug.Log($"δ�ҵ����Q�� {currentOutfitName} �ķ��b��Ո�_�J�Y���Ƿ����_��");
        }
    }

    private void LoadOutfit(Outfit outfit)
    {
        // ���� MeshFilter �� mesh
        skinnedMeshRenderer.sharedMesh = outfit.outfitMesh;


        // ���²��|�� Element1
        Renderer renderer = bodyMaterialMain.GetComponent<Renderer>();
        Material[] materials = renderer.materials;

        if (materials.Length > 1)
        {
            materials[0] = outfit.material; // ���Q Element1
            renderer.materials = materials;
            Debug.Log($"���b���| Element1 �Ѹ��Q�飺{outfit.material.name}");
        }
        else
        {
            Debug.Log("δ�ҵ����| Element1��Ո�_�J���|�����Ƿ����_��");
        }
    }

    public void UpdateOutfit(string newOutfitName)
    {
        // ���� PlayerData �ķ��b���Q
        playerManager.playerData.currentClothing = newOutfitName;

        // �ٴ��ҵ��K���d�����ķ��b�Y��
        InitializeOutfit();

        // ���攵�������O�б��湦�ܣ�
        SavePlayerData();
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetString("currentOutfit", playerManager.playerData.currentClothing);
        PlayerPrefs.Save();
        Debug.Log("��ҷ��b�����ѱ��棡");
    }
}
