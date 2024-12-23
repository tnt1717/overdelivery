using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class GuideStep
    {
        public Camera guideCamera; // 引導攝影機
        public GameObject tipCanvas; // 提示畫布
        public float duration = 3f; // 引導持續時間
        public float targetFOV = 40f; // 目標視角
    }

    public List<GuideStep> guideSteps; // 引導步驟列表
    public Camera mainCamera; // 主攝影機

    private int currentStepIndex = 0;
    private bool skipGuide = false;
    private List<GameObject> activeUIs = new List<GameObject>();

    private void Start()
    {
        // 初始化所有引導攝影機和提示畫布為隱藏狀態
        foreach (var step in guideSteps)
        {
            if (step.guideCamera != null)
            {
                step.guideCamera.enabled = false; // 初始關閉引導攝影機
            }

            if (step.tipCanvas != null)
            {
                step.tipCanvas.SetActive(false); // 初始隱藏提示畫布
            }
        }

        mainCamera.enabled = true; // 確保主攝影機啟用
    }

    private void Update()
    {
        // 偵測跳過按鍵
        if (Input.anyKeyDown && currentStepIndex < guideSteps.Count)
        {
            skipGuide = true;
        }
    }

    public void StartGuide(int stepIndex)
    {
        if (stepIndex >= 0 && stepIndex < guideSteps.Count)
        {
            currentStepIndex = stepIndex;
            StartCoroutine(GuideCoroutine());
        }
    }

    private IEnumerator GuideCoroutine()
    {
        skipGuide = false;

        var step = guideSteps[currentStepIndex];

        // 停用所有 UI
        DisableAllUI();

        // 啟用對應的引導攝影機和提示畫布
        if (step.guideCamera != null)
        {
            step.guideCamera.enabled = true;
        }

        if (step.tipCanvas != null)
        {
            step.tipCanvas.SetActive(true); // 顯示提示畫布
        }

        mainCamera.enabled = false; // 禁用主攝影機

        // 過渡到目標視角
        yield return StartCoroutine(FOVTransition(step.guideCamera, step.targetFOV, step.duration));

        float elapsed = 0f;
        while (elapsed < step.duration && !skipGuide)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 結束後恢復主攝影機和 UI
        if (step.guideCamera != null)
        {
            step.guideCamera.enabled = false; // 禁用引導攝影機
        }

        if (step.tipCanvas != null)
        {
            step.tipCanvas.SetActive(false); // 隱藏提示畫布
        }

        mainCamera.enabled = true; // 恢復主攝影機

        RestoreUI(); // 恢復 UI
    }

    private IEnumerator FOVTransition(Camera camera, float targetFOV, float duration)
    {
        float startFOV = camera.fieldOfView;
        float elapsed = 0f;

        while (elapsed < duration && !skipGuide)
        {
            camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / duration); // 視角過渡
            elapsed += Time.deltaTime;
            yield return null;
        }

        camera.fieldOfView = targetFOV; // 最終視角
    }

    private void DisableAllUI()
    {
        activeUIs.Clear();
        foreach (var ui in FindObjectsOfType<Graphic>())
        {
            if (ui.gameObject.activeSelf)
            {
                activeUIs.Add(ui.gameObject);
                ui.gameObject.SetActive(false); // 禁用 UI 元件
            }
        }
    }

    private void RestoreUI()
    {
        foreach (var ui in activeUIs)
        {
            if (ui != null)
            {
                ui.SetActive(true); // 恢復 UI 元件
            }
        }
        activeUIs.Clear();
    }
}
