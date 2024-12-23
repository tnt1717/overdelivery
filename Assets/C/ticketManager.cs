using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ticketManager : MonoBehaviour
{
    // ���� UI ������Ԫ��
    public Text sceneNameText;
    public GameObject ui;
    public GameObject pass;
    private PlayerManager playerManager;
    void Start()
    {

        ui.SetActive(false);
        pass.gameObject. SetActive(false);

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
        // ��ʼ���r�������֞鮔ǰ�������Q
        UpdateSceneName();
    }

    void Update()
    {
        // �z�鰴�I F7 �Ƿ񱻰���
        if (Input.GetKeyDown(KeyCode.F7))
        {
            TriggerActionF7();
        }

        // �z�鰴�I F8 �Ƿ񱻰���
        if (Input.GetKeyDown(KeyCode.F8))
        {
            TriggerActionF8();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            ui.SetActive(false);
            pass.gameObject.SetActive(false);


        }
    }

        // �������֞鮔ǰ�������Q
        private void UpdateSceneName()
    {
        if (sceneNameText != null)
        {
            sceneNameText.text = SceneManager.GetActiveScene().name;
        }
        
    }

    // F7 ���I�|�l�ķ���
    private void TriggerActionF7()
    {
        Debug.Log("F7 triggered: Perform your action here.");
        ui.SetActive(true);
        AudioManager.Instance.PlaySound("coin");
        playerManager.playerData.coins -= 1500;
        
        // �������@�e������ϣ�����е��О�
    }

    // F8 ���I�|�l�ķ���
    private void TriggerActionF8()
    {
        ui.active = true;
        pass.gameObject.SetActive(true);
        
        
        Debug.Log("F8 triggered: Perform your action here.");
        // �������@�e������һ�N�О�
    }
}
