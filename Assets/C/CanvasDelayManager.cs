using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDelayManager : MonoBehaviour
{
    public Canvas[] canvases; // 儲存所有畫布的陣列

    void Start()
    {
        // 禁用所有畫布
        foreach (Canvas canvas in canvases)
        {
            if (canvas != null)
                canvas.enabled = false;
        }

        // 啟動協程延遲啟用畫布
        StartCoroutine(EnableCanvasesAfterDelay(4f));
    }

    IEnumerator EnableCanvasesAfterDelay(float delay)
    {
        // 等待指定的秒數
        yield return new WaitForSeconds(delay);

        // 啟用所有畫布
        foreach (Canvas canvas in canvases)
        {
            if (canvas != null)
                canvas.enabled = true;
        }
    }
}
