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
                Debug.LogError("在 'PlayerSys' 上未找到 PlayerManager 組件！");
                return;
            }
        }
        else
        {
            Debug.LogError("未找到名稱為 'PlayerSys' 的物件！");
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
                    //playerManager.playerData.achievements["Achievement4"] = true; // 解鎖成就
                    //AchievementManager.Instance.UnlockAchievement("Achievement4");
                    //AchievementUIManager.Instance.ShowAchievement("Achievement4");
                    playerManager.UnlockAchievement("Achievement4");
                    //SaveSystem.SavePlayerData(playerManager.playerData);
                
            }
        }
    }
 }
