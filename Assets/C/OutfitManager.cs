using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitManager : MonoBehaviour
{
    [System.Serializable]
    public class Outfit
    {
        public string outfitName;     // 服裝名稱
        public Mesh outfitMesh;       // 服裝的 Mesh
        public Material material;     // 服裝的材質（針對 Element1）
    }

    public Outfit[] outfits;            // 所有服裝資料
    //public MeshFilter bodyMeshFilter;   // 玩家服裝的 MeshFilter
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject bodyMaterialMain; // 玩家服裝的材質管理物件

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

        InitializeOutfit();
    }
    private void Update()
    {
        InitializeOutfit();
    }

    private void InitializeOutfit()
    {
        string currentOutfitName = playerManager.playerData.currentClothing;

        // 找到對應的服裝資料
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
            // 加載服裝 Mesh 和材質
            LoadOutfit(selectedOutfit);
            Debug.Log($"已初始化服裝：{selectedOutfit.outfitName}");
        }
        else
        {
            Debug.Log($"未找到名稱為 {currentOutfitName} 的服裝！請確認資料是否正確。");
        }
    }

    private void LoadOutfit(Outfit outfit)
    {
        // 更新 MeshFilter 的 mesh
        skinnedMeshRenderer.sharedMesh = outfit.outfitMesh;


        // 更新材質的 Element1
        Renderer renderer = bodyMaterialMain.GetComponent<Renderer>();
        Material[] materials = renderer.materials;

        if (materials.Length > 1)
        {
            materials[0] = outfit.material; // 更換 Element1
            renderer.materials = materials;
            Debug.Log($"服裝材質 Element1 已更換為：{outfit.material.name}");
        }
        else
        {
            Debug.Log("未找到材質 Element1，請確認材質數量是否正確！");
        }
    }

    public void UpdateOutfit(string newOutfitName)
    {
        // 更新 PlayerData 的服裝名稱
        playerManager.playerData.currentClothing = newOutfitName;

        // 再次找到並加載對應的服裝資料
        InitializeOutfit();

        // 保存數據（假設有保存功能）
        SavePlayerData();
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetString("currentOutfit", playerManager.playerData.currentClothing);
        PlayerPrefs.Save();
        Debug.Log("玩家服裝數據已保存！");
    }
}
