using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBtnManager : MonoBehaviour
{
    public GameObject settingsCanvas;
    public GameObject tipCanvas;
    private PlayerManager playerManager;

    // 設定存檔路徑：執行檔所在目錄下的 "test_Data" 資料夾
    private static string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "test_Data");
    private static string filePath = Path.Combine(folderPath, "playerData.json");
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
    }

    public void OnStartButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // 檢查玩家是否已完成角色創建
        if (PlayerManager.instance.playerData.isCharacterCreated)
        {
            // 如果已創建角色，載入關卡選擇場景
            SceneManager.LoadScene("fristscenes");
        }
        else
        {
            // 如果未創建角色，載入角色創建場景
            SceneManager.LoadScene("CharacterCreationScene");
        }
    }
    public void OnNewButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // 刪除舊的玩家資料
        DeletePlayerData();

        SceneManager.LoadScene("CharacterCreationScene");
    }


    public void OnTIPButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // 顯示設定畫布
        tipCanvas.SetActive(true);
    }

    public void OnCloseTIPButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // 關閉設定畫布
        tipCanvas.SetActive(false);
    }

    public void OnExitButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // 保存玩家資料
        SaveSystem.SavePlayerData(PlayerManager.instance.playerData);
        Debug.Log("Player data saved.");

        // 退出遊戲
        Application.Quit();
    }
    public void SettingsBtnClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // 顯示設定畫布
        settingsCanvas.SetActive(true);
    }

    public void CloseSettingsBtnClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // 關閉設定畫布
        settingsCanvas.SetActive(false);
    }

    public static void DeletePlayerData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("SaveSystem: Deleted player data at " + filePath);
        }
        else
        {
            Debug.LogWarning("SaveSystem: No player data file found to delete.");
        }
    }

}
