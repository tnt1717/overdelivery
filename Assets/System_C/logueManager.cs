using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class logueManager : MonoBehaviour
{
    public Text dialogueText;           // 用於顯示對白的 Text 組件
    public GameObject dialogueCanvas;   // 用於顯示對白的畫布
    public float textSpeed = 0.05f;     // 顯示文字的速度
    public GameObject imageObject;      // 顯示圖片用的物件
    private PlayerManager PlayerManager;       // 玩家數據
    private Dictionary<string, string[]> dialogues;  // 儲存對話的字典
    private List<GameObject> activeUIs = new List<GameObject>();  // 用於保存當前啟用的 UI
    private bool isUIDisabled = false;  // 確保 UI 只禁用一次
    private bool isDialoguePlaying = false;  // 防止重複播放對話的標誌


    void Start()
    {
        // 動態尋找 PlayerManager
        GameObject playerSys = GameObject.Find("PlayerSys");
        if (playerSys != null)
        {
            PlayerManager = playerSys.GetComponent<PlayerManager>();
            if (PlayerManager == null)
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
        // 初始化對話字典
        dialogues = new Dictionary<string, string[]>
        {
            { "LV1", new string[] {
                "Boss：「【用戶名】！今天是你第一天來送外送對吧？\n說實話，經理都沒安排什麼正式的培訓，也不知道他怎麼想的，真的是不負責任啊。\n不過沒關係，既然你已經來了，那我就直接跟你說幾條注意事項\n幫你快速了解這個工作的內容，免得你手忙腳亂。」",
                "Boss：「好，現在你先騎上摩托車，試著啟動一下，看看是否順手。\n騎行的時候一定要保持專心，不要低頭玩手機！\n不僅不安全，也可能導致一些不必要的意外喔。」",
                "Boss：「不錯嘛，【用戶名】，你真有騎車的天賦。\n現在停一下車，按E拿出手機，接取你的第一單吧！\n別害羞，第一單對你來說可是特別有意義的，快點接單開始行動吧！」",
                "Boss：「嘿！快點，快點！別磨蹭，別讓顧客等太久了！\n你也不想成為那種總是讓顧客久等的外送員吧？\n趕快去接下一個單，為你的未來累積口碑！」"
            }},

            { "LV2", new string[] { 
                "Boss：「做的不錯，【用戶名】，我越來越喜歡你了。\n不過你注意到了嗎，經過一天的勞累，你的摩托車的機油也所剩無幾了。\n還記得剛剛在路邊看到的加油站嗎？找到他，加滿油再回去吧。」" ,
                "如果油箱沒油了，你的機車就會變得很慢很慢，還有可能壞掉！\n這樣可就沒法成為我們的金牌外送員了。" ,"記住，客人要吃的東西可不能亂給，否則不會得到全額!"} 
            
            
            
            },
            { "LV3", new string[] {
                "Boss：「現在，【用戶名】，你迎來了新挑戰。\n嘗試注意小地圖的資訊。\n同時避免與路上的相撞。」" ,
                "切記，安全抵達。" }



            }





        };

        dialogueCanvas.SetActive(false);  // 初始化對話畫布關閉
        imageObject.SetActive(false);    // 確保圖片物件初始化為隱藏
        //StartCoroutine(Sequence_LV1());  // 啟動對話序列
        StartCoroutine(CheckCurrentSceneAndInvokeMethod());
    }

    private IEnumerator Sequence_LV1()
    {
        yield return new WaitForSeconds(5f);
        // 播放第一段對話
        yield return StartCoroutine(DisplayDialogue("LV1", 0));

        // 等待 1 秒後播放第二段對話
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(DisplayDialogue("LV1", 1));

        // 顯示圖片並等待玩家按鍵
        yield return StartCoroutine(DisplayImageAndWait());

        while (!PlayerController.isRiding)
        {
            yield return null; // 每帧检查一次
        }
        //yield return new WaitForSeconds(5f);
        yield return StartCoroutine(DisplayDialogue("LV1", 2));

            // 等待 1 秒後播放最後一段對話
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(DisplayDialogue("LV1", 3));

        
    }
    private IEnumerator Sequence_LV2() {
        yield return new WaitForSeconds(5f);
        // 播放第一段對話
        yield return StartCoroutine(DisplayDialogue("LV2", 0));

        // 等待 1 秒後播放第二段對話
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(DisplayDialogue("LV2", 1));

        



    }
    private IEnumerator Sequence_LV3()
    {
        yield return StartCoroutine(DisplayDialogue("LV3", 0));
        yield return StartCoroutine(DisplayDialogue("LV3", 1));
        //yield return StartCoroutine(DisplayDialogue("LV3", 1));

    }
    private IEnumerator Sequence_LV4()
    {
        yield return StartCoroutine(DisplayDialogue("LV4", 1));
    }
    private IEnumerator Sequence_LV5()
    {
        yield return StartCoroutine(DisplayDialogue("LV5", 1));
    }

    private IEnumerator CheckCurrentSceneAndInvokeMethod()
    {
        // 獲取當前啟用的場景名稱
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 根據當前場景決定呼叫哪一個方法
        if (currentSceneName == "LV1")
        {
            // 呼叫針對 "Scene1" 的方法
            StartCoroutine(Sequence_LV1());
        }
        else if (currentSceneName == "LV2")
        {
            // 呼叫針對 "Scene2" 的方法
            StartCoroutine(Sequence_LV2());
        }
        else if (currentSceneName == "LV3")
        {
            StartCoroutine(Sequence_LV3());

        }
        else if (currentSceneName == "LV4")
        { 
        
        }
        else if (currentSceneName == "LV5")

        {

        }
        else if (currentSceneName == "LV6")

        {

        }


        else
                        {
                            // 如果是其他場景，可以呼叫其他方法
                            //
                        }

        // 等待幾秒鐘再繼續檢測（如果需要），這裡設置為 1 秒
        yield return new WaitForSeconds(1f);

        // 這樣可以在協程中繼續執行其他邏輯
    }


    private IEnumerator DisplayDialogue(string level, int dialogueIndex)
    {
        // 確保前一個對話執行結束
        while (isDialoguePlaying)
        {
            yield return null;
        }

        // 標記對話正在播放
        isDialoguePlaying = true;

        // 禁用所有UI元素
        DisableAllUI();

        // 顯示對話畫布
        dialogueCanvas.SetActive(true);

        // 清空文本
        dialogueText.text = "";

        // 確保字典中有指定的 level 和 dialogueIndex
        if (!dialogues.ContainsKey(level) || dialogueIndex < 0 || dialogueIndex >= dialogues[level].Length)
        {
            Debug.LogError($"Invalid level or dialogueIndex: Level '{level}', Index '{dialogueIndex}'");
            dialogueCanvas.SetActive(false);
            RestoreUI();
            isDialoguePlaying = false;
            yield break;
        }

        // 獲取對應的對話文本
        string dialogue = dialogues[level][dialogueIndex];

        // 替換「【用戶名】」為 PlayerName
        PlayerManager.instance.playerData = SaveSystem.LoadPlayerData();
        //Debug.Log(playerData.PlayerName);
        Debug.LogWarning(PlayerManager.playerData.PlayerName);
        dialogue = dialogue.Replace("【用戶名】", PlayerManager.playerData.PlayerName);

        // 替換 "/n" 為真正的換行符 "\n"
        dialogue = dialogue.Replace("/n", "\n");

        // 分割多行對話
        string[] lines = dialogue.Split('\n');

        // 顯示每一行對話
        foreach (string line in lines)
        {
            // 清空當前的對話顯示
            dialogueText.text = "";

            // 按字母逐字顯示每一行
            foreach (char letter in line.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(textSpeed); // 設定顯示每個字母的速度
            }

            // 等待一段時間後顯示下一行
            yield return new WaitForSeconds(0.5f); // 行間等待時間
        }

        // 等待對話全部顯示完成後
        yield return new WaitForSeconds(1f);

        // 關閉對話畫布
        dialogueCanvas.SetActive(false);

        // 恢復UI
        RestoreUI();

        // 對話播放完成
        isDialoguePlaying = false;
    }

    private IEnumerator DisplayImageAndWait()
    {
        DisableAllUI();
        imageObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        //yield return new WaitForSeconds(3f);
        // 等待玩家按任意鍵
        yield return new WaitUntil(() => Input.anyKeyDown);

        imageObject.SetActive(false);
        RestoreUI();
    }

    private void DisableAllUI()
    {
        if (isUIDisabled) return;

        activeUIs.Clear();
        Graphic[] graphics = FindObjectsOfType<Graphic>();
        foreach (Graphic graphic in graphics)
        {
            if (graphic.gameObject.activeSelf)
            {
                activeUIs.Add(graphic.gameObject);
                graphic.gameObject.SetActive(false);
            }
        }
        isUIDisabled = true;
    }

    private void RestoreUI()
    {
        foreach (GameObject ui in activeUIs)
        {
            if (ui != null)
            {
                ui.SetActive(true);
            }
        }
        activeUIs.Clear();
        isUIDisabled = false;
    }
}
