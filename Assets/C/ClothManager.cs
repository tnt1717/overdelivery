using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class Model
{
    public int price;            // ģ�̓r��
    public GameObject modelObject; // ģ�͌��������
}
[System.Serializable]
public class TextureItem
{
    public string textureName; // �N�D���Q
    public int price; // �N�D�r��
    public bool isUnlocked; // �Ƿ��ѽ��i
    public bool isPurchased; // �Ƿ���ُ�I

    // ���x������������Զ��x��������
    public Texture texture; // �N�D����

    // ������ʽ
    public TextureItem(string name, int price, Texture texture)
    {
        this.textureName = name;
        this.price = price;
        this.texture = texture;
        this.isUnlocked = false; // Ĭ�J��δ���i
        this.isPurchased = false; // Ĭ�J��δُ�I
    }
}
public class ClothManager : MonoBehaviour
{

    // UI Ԫ��
    public Button textureLeftButton, textureRightButton, modelLeftButton, modelRightButton;
    public Button buyTextureButton, buyModelButton; // ُ�I���o
    public Text textureInfoText, modelInfoText; // �@ʾ��ǰ��B
    public Text buyTextureButtonText; // �@ʾُ�I�N�D���o������

    // �N�D���P
    public Material targetMaterial;
    public TextureItem[] textureItems; // �����N�D�����
    private int currentTextureIndex = 0;

    // ģ�����P
    //public GameObject[] playerModels;
    public Model[] models; // ��������ģ�͵����

    private int currentModelIndex = 0;
    private GameObject activeModel;

    public Vector3 spawnPosition;
    private PlayerManager playerManager ;

    public Text coins;
    void Start()
    {

        // �@ȡ PlayerManager
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
        if (textureItems.Length > 0 && targetMaterial != null)
            UpdateMaterialTexture();

        if (models.Length > 0)
            SpawnCurrentModel();
        coins.text = playerManager.playerData.coins.ToString();
        UpdateModelUI();
        UpdateTextureUI();

        // �������o�¼�
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

        if (currentTextureIndex < 10) // ǰʮ���N�D
        {
            if (!textureItems[currentTextureIndex].isPurchased) // ���δ����ԓ�N�D
            {
                if (playerManager.playerData.coins >= texturePrice)
                {
                    playerManager.playerData.coins -= texturePrice; // �۳�����
                    textureItems[currentTextureIndex].isPurchased = true; // �O�Þ��ѓ���
                    Debug.Log($"��ُ�I�N�D: {textureName}");
                    UpdateTextureUI(); // ���°��o����
                }
                else
                {
                    Debug.Log("���Ų��㣬�o��ُ�Iԓ�N�D��");
                }
            }
        }
        else // ��ʮ���N�D��Ҫ���i
        {
            if (textureItems[currentTextureIndex].isUnlocked) // ����ѽ��i
            {
                Debug.Log("�N�D�ѽ��i��");
            }
            else
            {
                Debug.Log("�N�D��δ���i��");
            }
        }
    }

    //public void OnBuyTexture()
    //{
    //    string textureName = textures[currentTextureIndex].name; // ʹ�î�ǰ�N�D�����Q

    //    // ������δ����ԓ�N�D���������ţ��M��ُ�I
    //    if (!playerManager.playerData.expressions.ContainsKey(textureName))
    //    {
    //        if (playerManager.playerData.coins >= 100)
    //        {
    //            playerManager.playerData.coins -= 100; // �۳�����
    //            playerManager.playerData.expressions[textureName] = true; // �O��ԓ�N�D���ѓ���
    //            playerManager.playerData.currentExpression = textureName; // �O�Þ鮔ǰʹ�õ��N�D

    //            Debug.Log($"��ُ�I�K�O���N�D��ʹ����: {textureName}");
    //        }
    //        else
    //        {
    //            Debug.LogWarning("���Ų��㣬�o��ُ�Iԓ�N�D��");
    //            return; // ��ǰ�Y�����o���Mһ������
    //        }
    //    }
    //    // �������ѓ���ԓ�N�D��δ�b�䣬�����O�Þ�ʹ����
    //    else if (playerManager.playerData.currentExpression != textureName)
    //    {
    //        playerManager.playerData.currentExpression = textureName; // �O�Þ鮔ǰʹ�õ��N�D
    //        Debug.Log($"�N�D�ѓ��У��F���O�Þ�ʹ����: {textureName}");
    //    }
    //    else
    //    {
    //        Debug.Log($"�N�D�ѓ���������ʹ��: {textureName}");
    //    }

    //    UpdateTextureUI(); // �����N�DUI
    //}


    // ُ�Iģ��
    public void OnBuyModel()
    {
        string modelName = models[currentModelIndex].modelObject.name; // ʹ�î�ǰģ�͵����Q
        int modelPrice = models[currentModelIndex].price; // �@ȡ��ǰģ�͵ăr��

        if (playerManager.playerData.coins >= modelPrice && !playerManager.playerData.outfits.ContainsKey(modelName))
        {
            playerManager.playerData.coins -= modelPrice;
            playerManager.playerData.outfits[modelName] = true;
            playerManager.playerData.currentClothing = modelName;

            Debug.Log($"��ُ�I�K�O��ģ�͞�ʹ����: {modelName}");
            UpdateModelUI();
        }
        else if (playerManager.playerData.outfits.ContainsKey(modelName))
        {
            if (playerManager.playerData.currentClothing != modelName)
            {
                playerManager.playerData.currentClothing = modelName;
                Debug.Log($"ģ���ѓ��У��F���O�Þ�ʹ����: {modelName}");
            }
        }
        else
        {
            Debug.Log("���Ų��㣬�o��ُ�Iԓģ�ͣ�");
        }
        UpdateModelUI(); // ����ģ��UI
    }

    // �����N�D���|
    private void UpdateMaterialTexture()
    {
        targetMaterial.mainTexture = textureItems[currentTextureIndex].texture;
    }

    // ����ģ��
    private void SpawnCurrentModel()
    {
        //activeModel = Instantiate(playerModels[currentModelIndex], spawnPosition, Quaternion.Euler(0, 180, 0));
        activeModel = Instantiate(models[currentModelIndex].modelObject, spawnPosition, Quaternion.Euler(0, 180, 0));
    }

    // �����N�D UI
    // �����N�D UI
    private void UpdateTextureUI()
    {
        string textureName = textureItems[currentTextureIndex].textureName;
        int texturePrice = textureItems[currentTextureIndex].price;
        coins.text = playerManager.playerData.coins.ToString();

        // �Д��Ƿ����i���N�D
        if (currentTextureIndex < 10) // ǰʮ���N�D��Ҫُ�I
        {
            if (textureItems[currentTextureIndex].isPurchased) // ����ѽ�����ԓ�N�D
            {
                if (playerManager.playerData.currentExpression == textureName) // �Д��Ƿ��b����
                {
                    buyTextureButtonText.text = "�b����"; 

                }
                else
                {
                    buyTextureButtonText.text = "�ѓ���";
                }
            }
            else
            {
                buyTextureButtonText.text = "$" + texturePrice.ToString(); // �@ʾ�r��
            }
        }
        else // ��ʮ���N�D�ǽ��i�ͣ��������i��B�@ʾ
        {
            bool isUnlocked = textureItems[currentTextureIndex].isUnlocked; // �@ȡ���i��B
            if (isUnlocked)
            {
                if (playerManager.playerData.currentExpression == textureName) // �Д��Ƿ��b����
                {
                    buyTextureButtonText.text = "�b����";
                }
                else
                {
                    buyTextureButtonText.text = "�ѽ��i";
                }
            }
            else
            {
                buyTextureButtonText.text = "�i��";
            }
        }
    }


    // ����ģ�� UI
    private void UpdateModelUI()
    {
        string modelName = models[currentModelIndex].modelObject.name;
        int modelPrice = models[currentModelIndex].price; // �@ȡ��ǰģ�͵ăr��
        coins.text = playerManager.playerData.coins.ToString();


        // �Д�����Ƿ����ԓģ��
        if (playerManager.playerData.outfits.ContainsKey(modelName))
        {
            // �����ғ���ԓģ��
            if (playerManager.playerData.currentClothing == modelName)
            {
                modelInfoText.text = "�b����";
            }
            else
            {
                modelInfoText.text = "�ѓ���";
            }
        }
        else
        {
            // �����қ]�Г���ԓģ��
            modelInfoText.text = "$" + modelPrice.ToString();
        }
    }


    // �N�D���o�¼�
    public void OnTextureLeft()
    {
        currentTextureIndex = (currentTextureIndex - 1 + textureItems.Length) % textureItems.Length;
        UpdateMaterialTexture();
        UpdateTextureUI();
    }

    // �N�D�Ұ��o�¼�
    public void OnTextureRight()
    {
        currentTextureIndex = (currentTextureIndex + 1) % textureItems.Length;
        UpdateMaterialTexture();
        UpdateTextureUI();
    }

    // ģ�����o�¼�
    public void OnModelLeft()
    {
        Destroy(activeModel);  // �h����ǰ�@ʾ��ģ��
        currentModelIndex = (currentModelIndex - 1 + models.Length) % models.Length;
        SpawnCurrentModel();
        UpdateModelUI(); // ����ģ�͓��Р�B

    }

    // ģ���Ұ��o�¼�
    public void OnModelRight()
    {
        Destroy(activeModel);  // �h����ǰ�@ʾ��ģ��
        currentModelIndex = (currentModelIndex + 1) % models.Length;
        SpawnCurrentModel();
        UpdateModelUI(); // ����ģ�͓��Р�B
    }
}
