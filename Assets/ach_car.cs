using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ach_car : MonoBehaviour
{
    private PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "npc")
        {
            //if (playerManager.playerData.achievements.TryGetValue("Achievement4", out bool isUnlocked))
            if (playerManager.playerData.achievements["Achievement4"] == false)
            {
                    //playerManager.playerData.achievements["Achievement4"] = true; // ���i�ɾ�
                    //AchievementManager.Instance.UnlockAchievement("Achievement4");
                    //AchievementUIManager.Instance.ShowAchievement("Achievement4");
                    playerManager.UnlockAchievement("Achievement4");
                    //SaveSystem.SavePlayerData(playerManager.playerData);
                
            }
        }
    }
 }
