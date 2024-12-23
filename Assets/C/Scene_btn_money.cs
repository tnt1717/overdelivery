using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_btn_money : MonoBehaviour
{
    public Text coins;
    private PlayerManager playerManager;
    public GameObject ui;
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
        ui.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (coins != null) {

            coins.text = playerManager.playerData.coins.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            AudioManager.Instance.PlaySound("tap2");
            ui.gameObject.active = !ui.active;

        }
    }
    public void leve()
    {
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        AudioManager.Instance.PlaySound("tap2");
        // 保存玩家資料
        SaveSystem.SavePlayerData(PlayerManager.instance.playerData);
        Debug.Log("Player data saved.");

        // 退出遊戲
        Application.Quit();


    }
    public void mainmenu()
    {
        AudioManager.Instance.PlaySound("tap2");

        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        SceneManager.LoadScene("MainMenu");

    }
    public void restart()
    {
        AudioManager.Instance.PlaySound("tap2");

        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());

    }
    public void backtofrist()
    {
        AudioManager.Instance.PlaySound("tap2");

        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        SceneManager.LoadScene("fristscenes");

    }
}
