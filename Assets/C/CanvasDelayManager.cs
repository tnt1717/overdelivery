using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDelayManager : MonoBehaviour
{
    public Canvas[] canvases; // �������Ю��������

    void Start()
    {
        // �������Ю���
        foreach (Canvas canvas in canvases)
        {
            if (canvas != null)
                canvas.enabled = false;
        }

        // ���Ӆf�����t���î���
        StartCoroutine(EnableCanvasesAfterDelay(4f));
    }

    IEnumerator EnableCanvasesAfterDelay(float delay)
    {
        // �ȴ�ָ�����딵
        yield return new WaitForSeconds(delay);

        // �������Ю���
        foreach (Canvas canvas in canvases)
        {
            if (canvas != null)
                canvas.enabled = true;
        }
    }
}
