using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PackUIManager : MonoBehaviour
{
    public GameObject uiPackCanvas; // ��춴惦 UI_pack_canvas
    private bool isPlayerInZone = false; // ��춙z�y����Ƿ����|�l�^���
    private bool isUIPackCanvasOpen = false; // ���׷ۙ UI �Ƿ����_��

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

        // ��� UI ���_��������Ұ������I�����tһ���P�] UI
        if (isUIPackCanvasOpen && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CloseUIPackCanvasAfterDelay(1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ����M���|�l�^������� "Player"
        if (other.CompareTag("cus"))
        {
            isPlayerInZone = true;
            tipui.gameObject.SetActive(true);
        }
        else { tipui.gameObject.SetActive(false); }
    }

    private void OnTriggerExit(Collider other)
    {
        // ����x�_�|�l�^������� "Player"
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
