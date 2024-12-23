using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    //�{��:
    //SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
    //transitionManager.StartSceneTransition();

    public static SceneTransitionManager Instance { get; private set; } // ����ģʽ

    public Animator animator;        // ���� Animator �M��
    public Text tipText;            // �Á��@ʾ��ʾ���ֵ� UI Text Ԫ��
    public string[] tips;           // �A�O����ʾ�������
    public GameObject UI;
    public CanvasGroup fadeCanvasGroup; // ��춵��뵭��Ч���� CanvasGroup

    private void Awake()
    {
        // �O�Æ���ģʽ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �ГQ�����r���N��
        }
        else
        {
            Destroy(gameObject); // ����ѽ��Ќ������N���µČ���
        }
    }

    private void Start()
    {
        UI.SetActive(false); // ��ʼ�� UI ���[�ؠ�B
        fadeCanvasGroup.alpha = 0; // �_ʼ�r��ȫ͸��
    }

    // �������ГQ�r�{�ô˷���
    public void StartSceneTransition()
    {
        StartCoroutine(PlayTransition());
    }

    private IEnumerator PlayTransition()
    {
        // �@ʾ UI �K�����O��͸���Ȟ� 1����ȫ��͸����
        UI.SetActive(true);
        fadeCanvasGroup.alpha = 1;  // �����@ʾ���ʂ��^��Ч��

        // �O�� Animator ����ֵ�� true
        animator.SetBool("play", true);

        // �S�C�xȡһ����ʾ���ցK�@ʾ
        if (tips.Length > 0)
        {
            int randomIndex = Random.Range(0, tips.Length);
            tipText.text = tips[randomIndex];
        }

        // �ȴ�һ�Εr�g���@���^�ɄӮ����@ʾ�r�g
        yield return new WaitForSeconds(3.5f); // �{���������ʾ�����@ʾ�r�g

        // ����Ч�������� CanvasGroup �� Alpha ֵ
        float fadeDuration = 1.5f; // �O�������ĕr�g�L��
        float startAlpha = fadeCanvasGroup.alpha;
        float endAlpha = 0f;

        // ʹ�þ��Բ�ֵ��Lerp�����F����
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // ����_��͸�����O�Þ� 0
        fadeCanvasGroup.alpha = 0;

        // �����ꮅ���O�� Animator ����ֵ�� false �K�[�� UI
        animator.SetBool("play", false);
        UI.SetActive(false);
    }
}
