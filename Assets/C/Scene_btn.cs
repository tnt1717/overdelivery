using System.Collections;

using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_btn : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void backtoMain() {
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();
        AudioManager.Instance.PlaySound("tap2");
        // 保存玩家資料
        SaveSystem.SavePlayerData(PlayerManager.instance.playerData);


        SceneManager.LoadScene("fristscenes");

        GameObject coinSysObject = GameObject.Find("coin_Sys");
        if (coinSysObject != null && coinSysObject.activeInHierarchy) coinSysObject.SetActive(false);



    }
    public void mainmenu() {
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        GameObject coinSysObject = GameObject.Find("coin_Sys");
        if (coinSysObject != null && coinSysObject.activeInHierarchy) coinSysObject.SetActive(false);

        SceneManager.LoadScene("MainMenu");
        AudioManager.Instance.PlaySound("tap");



    }
    public void restart()
    {
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        GameObject coinSysObject = GameObject.Find("coin_Sys");
        if (coinSysObject != null && coinSysObject.activeInHierarchy) coinSysObject.SetActive(false);


        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        AudioManager.Instance.PlaySound("tap");

    }
    public void leve()
    {
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        GameObject coinSysObject = GameObject.Find("coin_Sys");
        if (coinSysObject != null && coinSysObject.activeInHierarchy) coinSysObject.SetActive(false);

        AudioManager.Instance.PlaySound("tap2");
        // 保存玩家資料
        SaveSystem.SavePlayerData(PlayerManager.instance.playerData);
        Debug.Log("Player data saved.");

        // 退出遊戲
        Application.Quit();


    }
}
