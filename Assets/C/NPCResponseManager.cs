using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCResponseManager : MonoBehaviour
{
    [System.Serializable]
    public class NPCDialogue
    {
        public string npcName; // NPC名稱
        public List<string> normalResponses; // 正常回應
        public List<string> delayResponses; // 延遲回應
        public List<string> errorResponses; // 錯誤回應
        public GameObject normalPrefab; // 正常狀態 Prefab
        public GameObject delayPrefab; // 延遲狀態 Prefab
        public GameObject errorPrefab; // 錯誤狀態 Prefab
    }

    public LevelManager levelManager;
    public List<NPCDialogue> npcDialogues = new List<NPCDialogue>(); // NPC對話資料
    public Canvas responseCanvas; // 回應畫布
    public Text responseText; // 對話顯示文本
    public Camera closeUpCamera; // 特寫攝影機

    public Transform spawnPosition; // 位置設定：空物件（Transform）來決定生成點

    private List<GameObject> activeUIs = new List<GameObject>(); // 追蹤啟用的UI
    private bool isUIDisabled = false; // UI是否已被關閉
    public Camera mainCamera;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        responseCanvas.enabled = false; // 確保初始畫布為關閉
        closeUpCamera.enabled = false; // 初始關閉特寫攝影機
        InitializeNPCDialogues(); // 初始化NPC對話資料
    }

    public void ShowNPCResponse(int npcIndex, float completionTime, float thresholdTime, int errors)
    {
        DisableAllUI();
        mainCamera.enabled = false;
        closeUpCamera.enabled = true;
        closeUpCamera.depth = 2;


        if (npcIndex < 0 || npcIndex >= npcDialogues.Count)
        {
            Debug.LogError("NPC索引無效！");
            return;
        }

        NPCDialogue npc = npcDialogues[npcIndex];
        string response = "";
        GameObject npcPrefab = null;

        if (errors > 0)
        {
            response = GetRandomResponse(npc.errorResponses);
            npcPrefab = npc.errorPrefab;
        }
        else if (completionTime <= thresholdTime)
        {
            response = GetRandomResponse(npc.normalResponses);
            npcPrefab = npc.normalPrefab;
        }
        else
        {
            response = GetRandomResponse(npc.delayResponses);
            npcPrefab = npc.delayPrefab;
        }

        responseCanvas.enabled = true;
        responseText.text = response;

        // 生成Prefab並放置於設定的位置
        if (npcPrefab != null && spawnPosition != null)
        {
            Instantiate(npcPrefab, spawnPosition.position, Quaternion.Euler(0f, -90f, 0f));//Quaternion.identity
        }
        StartCoroutine(wait());

        // 開始延遲後隱藏對話和關閉攝影機
        StartCoroutine(HideResultAfterDelay(5f));
    }
    IEnumerator wait()
    {
        while (Input.anyKey)
        {
            yield return null; // 每帧检查一次
        }
    }

        private IEnumerator HideResultAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        levelManager.ShowResultUI("1/1", "0/0", "10/10", 300); // 播放結算 UI
        closeUpCamera.enabled = false; // 關閉特寫攝影機
        closeUpCamera.depth = 0;

        mainCamera.enabled = true;
    }

    private string GetRandomResponse(List<string> responses)
    {
        return responses[Random.Range(0, responses.Count)];
    }

    private void DisableAllUI()
    {
        if (isUIDisabled) return;

        activeUIs.Clear();
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.gameObject.activeSelf && canvas != responseCanvas)
            {
                activeUIs.Add(canvas.gameObject);
                canvas.gameObject.SetActive(false);
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

    private void InitializeNPCDialogues()
    {
        npcDialogues.Add(new NPCDialogue
        {
            npcName = "雞哥",
            normalResponses = new List<string>
            {
                "「東西快給我吧，我這邊還有點急事，真的沒時間等太久了。」",
                "「咯咯，太好了！正好出門丟垃圾，結果外送就送來了，真是太方便了，感謝你！」",
                "「你們的外送速度也太慢了吧？\n我等了這麼久，真的是讓人有些不滿，可能你們的配送安排需要改進一下了。」"
            },
            delayResponses = new List<string>
            {
                "「這也太晚了吧？\n這麼晚才送到，怎麼搞的，還是我自己去餐廳吃算了，省得浪費時間。」",
                "「我都準備睡覺了，你才送來，你是要我在夢裡吃飯嗎。」",
                "「這速度可真不敢恭維，咯咯！我等飯的耐心可沒這麼好！\n下次務必更快點！」"
            },
            errorResponses = new List<string>
            {
                "「這個餐點好像不是我點的吧？\n怎麼會發生這麼低級的錯誤？是不是你們搞錯了？」",
                "「這是送給「隔壁老王」的吧？\n拜託再認真一點，下次別搞烏龍了。」",
                "「這個餐點不對吧！\n哥的品味可是獨一無二，別給我亂來。」"
            },
            normalPrefab = null, // 您可以在編輯器中分配 Prefab
            delayPrefab = null,
            errorPrefab = null
        });

        // 這裡可以繼續新增其他NPC資料
    }
}
