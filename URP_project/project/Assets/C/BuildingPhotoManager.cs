using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuildingPhotoManager : MonoBehaviour
{
    [System.Serializable]
    public class BuildingPhotoPair
    {
        public Transform building;      // 建築物的 Transform
        public GameObject photoObject;  // 對應的照片物件
        public string sceneName;        // 關卡名稱（對應的 Unity 場景）
    }

    public GameObject tip;              // 提示 UI 物件
    public Text tipText;                // 提示文本元件
    public List<BuildingPhotoPair> buildingPhotoPairs; // 存儲建築與照片的配對
    public Transform player;            // 玩家物件的 Transform
    public float detectionRange = 8f;   // 檢測距離範圍

    private void Update()
    {
        bool isTipActive = false;       // 追蹤提示顯示狀態

        foreach (var pair in buildingPhotoPairs)
        {
            // 計算玩家與每個建築物之間的距離
            float distanceToPlayer = Vector3.Distance(pair.building.position, player.position);

            // 如果玩家在檢測範圍內，顯示對應的照片；否則隱藏
            if (distanceToPlayer <= detectionRange)
            {
                pair.photoObject.SetActive(true);  // 顯示照片

                // 獲取該關卡的名稱和關卡數字（例如 "LV1"）
                string currentLevel = pair.sceneName;

                // 檢查前一關卡的星星數量
                bool canEnterLevel = true;
                int previousLevelStars = 0;

                // 設置例外情況，若關卡名稱為 "clothshop" 或 "motoshop"，允許進入
                if (currentLevel == "clothshop" || currentLevel == "motoshop")
                {
                    canEnterLevel = true;
                }
                else if (currentLevel != "LV1") // LV2 及以後的關卡需要檢查
                {
                    string previousLevel = "LV" + (int.Parse(currentLevel.Substring(2)) - 1);
                    if (PlayerManager.instance.playerData.levelStars.TryGetValue(previousLevel, out previousLevelStars))
                    {
                        canEnterLevel = previousLevelStars > 0;
                    }
                    else
                    {
                        canEnterLevel = false; // 如果找不到前一關的星星數據，則視為未解鎖
                    }
                }

                // 根據是否解鎖來顯示提示
                if (canEnterLevel)
                {
                    tipText.text = "按下F進入";
                    isTipActive = true;

                    // 檢測 F 鍵是否被按下並且可以進入關卡
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // 加載場景
                        Debug.Log($"Loading level: {pair.sceneName}");
                        AudioManager.Instance.PlaySound("tap");
                        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
                        transitionManager.StartSceneTransition();
                        StartCoroutine(Delay());
                        SceneManager.LoadScene(pair.sceneName);
                    }
                }
                else
                {
                    tipText.text = "未解鎖";
                    isTipActive = true;
                }
            }
            else
            {
                pair.photoObject.SetActive(false); // 隱藏照片
            }
        }

        // 最後統一設置提示的顯示狀態
        tip.SetActive(isTipActive);
    }
    IEnumerator Delay()
    {
        // 延遲兩秒
        yield return new WaitForSeconds(2f);

        // 執行延遲後的動作
        
    }

}
