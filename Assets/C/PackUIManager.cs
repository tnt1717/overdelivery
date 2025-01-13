using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PackUIManager : MonoBehaviour
{
    public GameObject uiPackCanvas; // 用於存儲 UI_pack_canvas
    private bool isPlayerInZone = false; // 用於檢測玩家是否在觸發區域內
    private bool isUIPackCanvasOpen = false; // 用於追蹤 UI 是否已開啟

    public GameObject tipui;

    private void Start()
    {
        tipui.gameObject.SetActive(false);
        uiPackCanvas.SetActive(false);

    }

    private void Update()
    {
        if (isPlayerInZone && Input.GetKey(KeyCode.F))
        {
            OpenUIPackCanvas();
        }
        else {
            //tipui.gameObject.SetActive(false);
            uiPackCanvas.SetActive(false);

        }

        // 如果 UI 已開啟，且玩家按下左鍵，延遲一秒關閉 UI
        if (isUIPackCanvasOpen && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CloseUIPackCanvasAfterDelay(1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果進入觸發區的物件是 "Player"
        if (other.CompareTag("cus"))
        {
            isPlayerInZone = true;
            tipui.gameObject.SetActive(true);
        }
        else { tipui.gameObject.SetActive(false); }
    }

    private void OnTriggerExit(Collider other)
    {
        // 如果離開觸發區的物件是 "Player"
        if (other.CompareTag("cus"))
        {
            isPlayerInZone = false;
            uiPackCanvas.SetActive(false);

            tipui.gameObject.SetActive(false);

        }
    }

    private void OpenUIPackCanvas()
    {
        if (uiPackCanvas != null)
        {
            uiPackCanvas.SetActive(true);
            isUIPackCanvasOpen = true;
        }
    }

    private IEnumerator CloseUIPackCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (uiPackCanvas != null)
        {
            uiPackCanvas.SetActive(false);
            isUIPackCanvasOpen = false;
        }
    }
}
