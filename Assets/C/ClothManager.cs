using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class Model
{
    public int price;            // 模型價格
    public GameObject modelObject; // 模型對應的物件
}
[System.Serializable]
public class TextureItem
{
    public string textureName; // 貼圖名稱
    public int price; // 貼圖價格
    public bool isUnlocked; // 是否已解鎖
    public bool isPurchased; // 是否已購買

    // 可選，根據需求可以定義更多屬性
    public Texture texture; // 貼圖本身

    // 建構函式
    public TextureItem(string name, int price, Texture texture)
    {
        this.textureName = name;
        this.price = price;
        this.texture = texture;
        this.isUnlocked = false; // 默認為未解鎖
        this.isPurchased = false; // 默認為未購買
    }
}
public class ClothManager : MonoBehaviour
{

    // UI 元素
    public Button textureLeftButton, textureRightButton, modelLeftButton, modelRightButton;
    public Button buyTextureButton, buyModelButton; // 購買按鈕
    public Text textureInfoText, modelInfoText; // 顯示當前狀態
    public Text buyTextureButtonText; // 顯示購買貼圖按鈕的文字

    // 貼圖相關
    public Material targetMaterial;
    public TextureItem[] textureItems; // 儲存貼圖的陣列
    private int currentTextureIndex = 0;

    // 模型相關
    //public GameObject[] playerModels;
    public Model[] models; // 儲存所有模型的陣列

    private int currentModelIndex = 0;
    private GameObject activeModel;

    public Vector3 spawnPosition;
    private PlayerManager playerManager ;

    public Text coins;
    void Start()
    {

        // 獲取 PlayerManager
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
        if (textureItems.Length > 0 && targetMaterial != null)
            UpdateMaterialTexture();

        if (models.Length > 0)
            SpawnCurrentModel();
        coins.text = playerManager.playerData.coins.ToString();
        UpdateModelUI();
        UpdateTextureUI();

        // 綁定按鈕事件
        textureLeftButton.onClick.AddListener(OnTextureLeft);
        textureRightButton.onClick.AddListener(OnTextureRight);
        modelLeftButton.onClick.AddListener(OnModelLeft);
        modelRightButton.onClick.AddListener(OnModelRight);
        buyTextureButton.onClick.AddListener(OnBuyTexture);
        buyModelButton.onClick.AddListener(OnBuyModel);
    }
    public void OnBuyTexture()
    {
        string textureName = textureItems[currentTextureIndex].textureName;
        int texturePrice = textureItems[currentTextureIndex].price;

        if (currentTextureIndex < 10) // 前十個貼圖
        {
            if (!textureItems[currentTextureIndex].isPurchased) // 玩家未擁有該貼圖
            {
                if (playerManager.playerData.coins >= texturePrice)
                {
                    playerManager.playerData.coins -= texturePrice; // 扣除金幣
                    textureItems[currentTextureIndex].isPurchased = true; // 設置為已擁有
                    Debug.Log($"已購買貼圖: {textureName}");
                    UpdateTextureUI(); // 更新按鈕文字
                }
                else
                {
                    Debug.Log("金幣不足，無法購買該貼圖！");
                }
            }
        }
        else // 後十個貼圖需要解鎖
        {
            if (textureItems[currentTextureIndex].isUnlocked) // 如果已解鎖
            {
                Debug.Log("貼圖已解鎖！");
            }
            else
            {
                Debug.Log("貼圖尚未解鎖！");
            }
        }
    }

    //public void OnBuyTexture()
    //{
    //    string textureName = textures[currentTextureIndex].name; // 使用當前貼圖的名稱

    //    // 如果玩家未擁有該貼圖且有足夠金幣，進行購買
    //    if (!playerManager.playerData.expressions.ContainsKey(textureName))
    //    {
    //        if (playerManager.playerData.coins >= 100)
    //        {
    //            playerManager.playerData.coins -= 100; // 扣除金幣
    //            playerManager.playerData.expressions[textureName] = true; // 設置該貼圖為已擁有
    //            playerManager.playerData.currentExpression = textureName; // 設置為當前使用的貼圖

    //            Debug.Log($"已購買並設置貼圖為使用中: {textureName}");
    //        }
    //        else
    //        {
    //            Debug.LogWarning("金幣不足，無法購買該貼圖！");
    //            return; // 提前結束，無法進一步操作
    //        }
    //    }
    //    // 如果玩家已擁有該貼圖但未裝備，將其設置為使用中
    //    else if (playerManager.playerData.currentExpression != textureName)
    //    {
    //        playerManager.playerData.currentExpression = textureName; // 設置為當前使用的貼圖
    //        Debug.Log($"貼圖已擁有，現在設置為使用中: {textureName}");
    //    }
    //    else
    //    {
    //        Debug.Log($"貼圖已擁有且正在使用: {textureName}");
    //    }

    //    UpdateTextureUI(); // 更新貼圖UI
    //}


    // 購買模型
    public void OnBuyModel()
    {
        string modelName = models[currentModelIndex].modelObject.name; // 使用當前模型的名稱
        int modelPrice = models[currentModelIndex].price; // 獲取當前模型的價格

        if (playerManager.playerData.coins >= modelPrice && !playerManager.playerData.outfits.ContainsKey(modelName))
        {
            playerManager.playerData.coins -= modelPrice;
            playerManager.playerData.outfits[modelName] = true;
            playerManager.playerData.currentClothing = modelName;

            Debug.Log($"已購買並設置模型為使用中: {modelName}");
            UpdateModelUI();
        }
        else if (playerManager.playerData.outfits.ContainsKey(modelName))
        {
            if (playerManager.playerData.currentClothing != modelName)
            {
                playerManager.playerData.currentClothing = modelName;
                Debug.Log($"模型已擁有，現在設置為使用中: {modelName}");
            }
        }
        else
        {
            Debug.Log("金幣不足，無法購買該模型！");
        }
        UpdateModelUI(); // 更新模型UI
    }

    // 更新貼圖材質
    private void UpdateMaterialTexture()
    {
        targetMaterial.mainTexture = textureItems[currentTextureIndex].texture;
    }

    // 生成模型
    private void SpawnCurrentModel()
    {
        //activeModel = Instantiate(playerModels[currentModelIndex], spawnPosition, Quaternion.Euler(0, 180, 0));
        activeModel = Instantiate(models[currentModelIndex].modelObject, spawnPosition, Quaternion.Euler(0, 180, 0));
    }

    // 更新貼圖 UI
    // 更新貼圖 UI
    private void UpdateTextureUI()
    {
        string textureName = textureItems[currentTextureIndex].textureName;
        int texturePrice = textureItems[currentTextureIndex].price;
        coins.text = playerManager.playerData.coins.ToString();

        // 判斷是否為解鎖的貼圖
        if (currentTextureIndex < 10) // 前十個貼圖需要購買
        {
            if (textureItems[currentTextureIndex].isPurchased) // 玩家已經擁有該貼圖
            {
                if (playerManager.playerData.currentExpression == textureName) // 判斷是否裝備中
                {
                    buyTextureButtonText.text = "裝備中"; 

                }
                else
                {
                    buyTextureButtonText.text = "已擁有";
                }
            }
            else
            {
                buyTextureButtonText.text = "$" + texturePrice.ToString(); // 顯示價格
            }
        }
        else // 後十個貼圖是解鎖型，根據解鎖狀態顯示
        {
            bool isUnlocked = textureItems[currentTextureIndex].isUnlocked; // 獲取解鎖狀態
            if (isUnlocked)
            {
                if (playerManager.playerData.currentExpression == textureName) // 判斷是否裝備中
                {
                    buyTextureButtonText.text = "裝備中";
                }
                else
                {
                    buyTextureButtonText.text = "已解鎖";
                }
            }
            else
            {
                buyTextureButtonText.text = "鎖定";
            }
        }
    }


    // 更新模型 UI
    private void UpdateModelUI()
    {
        string modelName = models[currentModelIndex].modelObject.name;
        int modelPrice = models[currentModelIndex].price; // 獲取當前模型的價格
        coins.text = playerManager.playerData.coins.ToString();


        // 判斷玩家是否擁有該模型
        if (playerManager.playerData.outfits.ContainsKey(modelName))
        {
            // 如果玩家擁有該模型
            if (playerManager.playerData.currentClothing == modelName)
            {
                modelInfoText.text = "裝備中";
            }
            else
            {
                modelInfoText.text = "已擁有";
            }
        }
        else
        {
            // 如果玩家沒有擁有該模型
            modelInfoText.text = "$" + modelPrice.ToString();
        }
    }


    // 貼圖左按鈕事件
    public void OnTextureLeft()
    {
        currentTextureIndex = (currentTextureIndex - 1 + textureItems.Length) % textureItems.Length;
        UpdateMaterialTexture();
        UpdateTextureUI();
    }

    // 貼圖右按鈕事件
    public void OnTextureRight()
    {
        currentTextureIndex = (currentTextureIndex + 1) % textureItems.Length;
        UpdateMaterialTexture();
        UpdateTextureUI();
    }

    // 模型左按鈕事件
    public void OnModelLeft()
    {
        Destroy(activeModel);  // 刪除當前顯示的模型
        currentModelIndex = (currentModelIndex - 1 + models.Length) % models.Length;
        SpawnCurrentModel();
        UpdateModelUI(); // 更新模型擁有狀態

    }

    // 模型右按鈕事件
    public void OnModelRight()
    {
        Destroy(activeModel);  // 刪除當前顯示的模型
        currentModelIndex = (currentModelIndex + 1) % models.Length;
        SpawnCurrentModel();
        UpdateModelUI(); // 更新模型擁有狀態
    }
}
