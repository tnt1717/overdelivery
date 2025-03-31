using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    //調用:
    SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
    //transitionManager.StartSceneTransition();

    public static SceneTransitionManager Instance { get; private set; } // 單例模式

    public UI_Loading_Fade loadingEffect; // 新增引用 UI_Loading_Fade

    public Animator animator;        // 引用 Animator 組件
    public Text tipText;            // 用來顯示提示文字的 UI Text 元件
    public string[] tips;           // 預設的提示文字陣列
    public GameObject UI;
    public CanvasGroup fadeCanvasGroup; // 用於淡入淡出效果的 CanvasGroup

    private void Awake()
    {
        // 設置單例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切換場景時不銷毀
        }
        else
        {
            Destroy(gameObject); // 如果已經有實例，銷毀新的實例
        }
    }

    private void Start()
    {
        UI.SetActive(false); // 初始化 UI 為隱藏狀態
        fadeCanvasGroup.alpha = 0; // 開始時完全透明
    }

    // 當場景切換時調用此方法
    public void StartSceneTransition()
    {
        StartCoroutine(PlayTransition());
    }



    private IEnumerator PlayTransition()
    {
        //// 顯示 UI 並立即設定透明度為 1（完全不透明）
        UI.SetActive(true);
        //fadeCanvasGroup.alpha = 1;  // 立即顯示，準備過渡效果
        loadingEffect.StartScalingUp();
        //yield return new WaitUntil(() => loadingEffect.loading_0.transform.localScale == loadingEffect.targetScale_0);
        yield return new WaitUntil(() => Vector3.Distance(loadingEffect.loading_0.transform.localScale, loadingEffect.targetScale_0) < 0.1f);
        // 設置 Animator 布林值為 true
        animator.SetBool("play", true);

        // 隨機選取一段提示文字並顯示
        if (tips.Length > 0)
        {
            int randomIndex = Random.Range(0, tips.Length);
            tipText.text = tips[randomIndex];
        }

        // 等待一段時間，這是過渡動畫的顯示時間
        yield return new WaitForSeconds(2.5f); // 調整為你的提示文字顯示時間

        //// 淡出效果，控制 CanvasGroup 的 Alpha 值
        //float fadeDuration = 1.5f; // 設定淡出的時間長度
        //float startAlpha = fadeCanvasGroup.alpha;
        //float endAlpha = 0f;

        //// 使用線性插值（Lerp）實現淡出
        //float timeElapsed = 0f;
        //while (timeElapsed < fadeDuration)
        //{
        //    fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);
        //    timeElapsed += Time.deltaTime;
        //    yield return null;
        //}

        //// 最後確保透明度設置為 0
        //fadeCanvasGroup.alpha = 0;

        // 觸發縮小動畫
        loadingEffect.StartScalingDown();

        // 等待縮小完成
        //yield return new WaitUntil(() => loadingEffect.loading_0.transform.localScale == loadingEffect.targetScale_1);
        yield return new WaitUntil(() => Vector3.Distance(loadingEffect.loading_0.transform.localScale, loadingEffect.targetScale_1) < 0.1f);
        loadingEffect.End();


        // 播放完畢後設置 Animator 布林值為 false 並隱藏 UI
        animator.SetBool("play", false);
        UI.SetActive(false);
    }
}
