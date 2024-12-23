using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBtnManager : MonoBehaviour
{
    public GameObject settingsCanvas;
    public GameObject tipCanvas;
    private PlayerManager playerManager;

    // �O����n·�������Йn����Ŀ��µ� "test_Data" �Y�ϊA
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

    public void OnStartButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // �z������Ƿ�����ɽ�ɫ����
        if (PlayerManager.instance.playerData.isCharacterCreated)
        {
            // ����ф�����ɫ���d���P���x�����
            SceneManager.LoadScene("fristscenes");
        }
        else
        {
            // ���δ������ɫ���d���ɫ��������
            SceneManager.LoadScene("CharacterCreationScene");
        }
    }
    public void OnNewButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // �h���f������Y��
        DeletePlayerData();

        SceneManager.LoadScene("CharacterCreationScene");
    }


    public void OnTIPButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // �@ʾ�O������
        tipCanvas.SetActive(true);
    }

    public void OnCloseTIPButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // �P�]�O������
        tipCanvas.SetActive(false);
    }

    public void OnExitButtonClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // ��������Y��
        SaveSystem.SavePlayerData(PlayerManager.instance.playerData);
        Debug.Log("Player data saved.");

        // �˳��[��
        Application.Quit();
    }
    public void SettingsBtnClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // �@ʾ�O������
        settingsCanvas.SetActive(true);
    }

    public void CloseSettingsBtnClick()
    {
        AudioManager.Instance.PlaySound("tap2");
        // �P�]�O������
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
