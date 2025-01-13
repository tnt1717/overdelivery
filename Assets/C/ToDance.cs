using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToDance : MonoBehaviour
{
    private Animator animator;
    private bool isMouseOver = false;
    static public bool hasSelectedModel = false; // 防止重複選擇物件的旗標
    public CanvasGroup canvasGroup; // 控制畫布透明度
    public InputField playerNameInput; // 玩家名稱輸入框
    public Button confirmButton; // 確認按鈕
    public Text tip;
    private bool isCanvasVisible = false; // 判斷畫布是否已顯示
    private Camera mainCamera; // 主攝影機
    public Image modelImage; // 用於顯示模型對應圖片的 Image 組件
    public Sprite imageA; // A 對應的圖片
    public Sprite imageB; // B 對應的圖片
    public GameObject p;

    void Start()
    {
        p.active = false;
        mainCamera = Camera.main; // 獲取主攝影機
        tip.text = "請選擇角色";
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator 未找到，請確認物件上是否有 Animator 組件。");
        }

        // 初始化畫布透明度為 0，隱藏
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        // 設置按鈕事件
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmPlayerName);
        }
    }

    void Update()
    {
        if (animator != null)
        {
            animator.SetBool("dance", isMouseOver);
        }

        if (!hasSelectedModel) // 僅在未選擇模型時執行射線檢測
        {
            CheckMouseHover();
        }
    }

    private void CheckMouseHover()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject == gameObject)
            {
                if (!isMouseOver) // 防止重複觸發
                {
                    isMouseOver = true;
                    animator.SetBool("dance", true);
                }

                // 偵測點擊事件
                if (Input.GetMouseButtonDown(0)) // 滑鼠左鍵
                {
                    AudioManager.Instance.PlaySound("tap");

                    RecordModelName(hitObject.name);
                }
            }
            else
            {
                ResetHoverState();
            }
        }
        else
        {
            ResetHoverState();
        }
    }

    private void ResetHoverState()
    {
        if (isMouseOver)
        {
            isMouseOver = false;
            animator.SetBool("dance", false);
        }
    }

    private void RecordModelName(string name)
    {
        if (hasSelectedModel) return; // 若已選擇模型則不再執行

        hasSelectedModel = true; // 設置旗標防止重複選擇

        if (PlayerManager.instance != null && PlayerManager.instance.playerData != null)
        {
            PlayerManager.instance.playerData.Playermodle = name; // 記錄到 playerData
            Debug.Log($"已記錄玩家模型名稱: {name}");
            StartCoroutine(FadeInCanvas()); // 漸漸顯示畫布
            tip.text = "請輸入玩家名稱";

            // 根據名稱切換圖片
            if (name == "JP_Boy_01" && imageA != null)
            {
                modelImage.sprite = imageA;
            }
            else if (name == "JP_Girl_01" && imageB != null)
            {
                modelImage.sprite = imageB;
            }
            else
            {
                Debug.LogWarning("未找到對應的圖片，請確認設置是否正確。");
            }
        }
        else
        {
            Debug.LogError("PlayerManager 或 PlayerData 未初始化，無法記錄模型名稱。");
        }
    }

    private IEnumerator FadeInCanvas()
    {
        if (canvasGroup == null) yield break;

        isCanvasVisible = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
    }

    private void OnConfirmPlayerName()
    {
        if (PlayerManager.instance != null && PlayerManager.instance.playerData != null)
        {
            string playerName = playerNameInput.text; // 獲取輸入框內容
            if (!string.IsNullOrEmpty(playerName))
            {
                PlayerManager.instance.playerData.isCharacterCreated = true;
                PlayerManager.instance.playerData.PlayerName = playerName;
                Debug.Log($"玩家名稱已存儲: {playerName}");
                SaveSystem.SavePlayerData(PlayerManager.instance.playerData);
                p.active = true;

                StartCoroutine(DelayAction());
                StartCoroutine(DelayActionB());



                //SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
                //transitionManager.StartSceneTransition();

                //SceneManager.LoadScene("LV1");
                //AudioManager.Instance.PlaySound("tap");
            }
            else
            {
                Debug.LogWarning("玩家名稱為空，請輸入有效名稱。");
                tip.text = "玩家名稱不能為空。";
            }
        }
        else
        {
            Debug.LogError("PlayerManager 或 PlayerData 未初始化，無法存儲玩家名稱。");
            tip.text = " PlayerData 未初始化，無法存儲玩家名稱。";
        }
    }

    IEnumerator DelayAction()
    {
        // 延遲兩秒
        yield return new WaitForSeconds(5f);
    }
    IEnumerator DelayActionB()
    {
        // 延遲兩秒
        yield return new WaitForSeconds(3f);
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        SceneManager.LoadScene("LV1");
        AudioManager.Instance.PlaySound("tap");

    }

}
