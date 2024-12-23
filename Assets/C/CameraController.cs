using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class GuideStep
    {
        public Camera guideCamera; // �����zӰ�C
        public GameObject tipCanvas; // ��ʾ����
        public float duration = 3f; // �������m�r�g
        public float targetFOV = 40f; // Ŀ��ҕ��
    }

    public List<GuideStep> guideSteps; // �������E�б�
    public Camera mainCamera; // ���zӰ�C

    private int currentStepIndex = 0;
    private bool skipGuide = false;
    private List<GameObject> activeUIs = new List<GameObject>();

    private void Start()
    {
        // ��ʼ�����������zӰ�C����ʾ�������[�ؠ�B
        foreach (var step in guideSteps)
        {
            if (step.guideCamera != null)
            {
                step.guideCamera.enabled = false; // ��ʼ�P�]�����zӰ�C
            }

            if (step.tipCanvas != null)
            {
                step.tipCanvas.SetActive(false); // ��ʼ�[����ʾ����
            }
        }

        mainCamera.enabled = true; // �_�����zӰ�C����
    }

    private void Update()
    {
        // �ɜy���^���I
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

        // ͣ������ UI
        DisableAllUI();

        // ���Ì����������zӰ�C����ʾ����
        if (step.guideCamera != null)
        {
            step.guideCamera.enabled = true;
        }

        if (step.tipCanvas != null)
        {
            step.tipCanvas.SetActive(true); // �@ʾ��ʾ����
        }

        mainCamera.enabled = false; // �������zӰ�C

        // �^�ɵ�Ŀ��ҕ��
        yield return StartCoroutine(FOVTransition(step.guideCamera, step.targetFOV, step.duration));

        float elapsed = 0f;
        while (elapsed < step.duration && !skipGuide)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // �Y����֏����zӰ�C�� UI
        if (step.guideCamera != null)
        {
            step.guideCamera.enabled = false; // ���������zӰ�C
        }

        if (step.tipCanvas != null)
        {
            step.tipCanvas.SetActive(false); // �[����ʾ����
        }

        mainCamera.enabled = true; // �֏����zӰ�C

        RestoreUI(); // �֏� UI
    }

    private IEnumerator FOVTransition(Camera camera, float targetFOV, float duration)
    {
        float startFOV = camera.fieldOfView;
        float elapsed = 0f;

        while (elapsed < duration && !skipGuide)
        {
            camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / duration); // ҕ���^��
            elapsed += Time.deltaTime;
            yield return null;
        }

        camera.fieldOfView = targetFOV; // ��Kҕ��
    }

    private void DisableAllUI()
    {
        activeUIs.Clear();
        foreach (var ui in FindObjectsOfType<Graphic>())
        {
            if (ui.gameObject.activeSelf)
            {
                activeUIs.Add(ui.gameObject);
                ui.gameObject.SetActive(false); // ���� UI Ԫ��
            }
        }
    }

    private void RestoreUI()
    {
        foreach (var ui in activeUIs)
        {
            if (ui != null)
            {
                ui.SetActive(true); // �֏� UI Ԫ��
            }
        }
        activeUIs.Clear();
    }
}
