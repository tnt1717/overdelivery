using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToDance : MonoBehaviour
{
    private Animator animator;
    private bool isMouseOver = false;
    static public bool hasSelectedModel = false; // ��ֹ���}�x����������
    public CanvasGroup canvasGroup; // ���Ʈ���͸����
    public InputField playerNameInput; // ������Qݔ���
    public Button confirmButton; // �_�J���o
    public Text tip;
    private bool isCanvasVisible = false; // �Д஋���Ƿ����@ʾ
    private Camera mainCamera; // ���zӰ�C
    public Image modelImage; // ����@ʾģ�͌����DƬ�� Image �M��
    public Sprite imageA; // A �����ĈDƬ
    public Sprite imageB; // B �����ĈDƬ
    public GameObject p;

    void Start()
    {
        p.active = false;
        mainCamera = Camera.main; // �@ȡ���zӰ�C
        tip.text = "Ո�x���ɫ";
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator δ�ҵ���Ո�_�J������Ƿ��� Animator �M����");
        }

        // ��ʼ������͸���Ȟ� 0���[��
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        // �O�ð��o�¼�
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

        if (!hasSelectedModel) // �H��δ�x��ģ�͕r�����侀�z�y
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
                if (!isMouseOver) // ��ֹ���}�|�l
                {
                    isMouseOver = true;
                    animator.SetBool("dance", true);
                }

                // �ɜy�c���¼�
                if (Input.GetMouseButtonDown(0)) // �������I
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
        if (hasSelectedModel) return; // �����x��ģ�̈́t���و���

        hasSelectedModel = true; // �O����˷�ֹ���}�x��

        if (PlayerManager.instance != null && PlayerManager.instance.playerData != null)
        {
            PlayerManager.instance.playerData.Playermodle = name; // ӛ䛵� playerData
            Debug.Log($"��ӛ����ģ�����Q: {name}");
            StartCoroutine(FadeInCanvas()); // �u�u�@ʾ����
            tip.text = "Ոݔ��������Q";

            // �������Q�ГQ�DƬ
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
                Debug.LogWarning("δ�ҵ������ĈDƬ��Ո�_�J�O���Ƿ����_��");
            }
        }
        else
        {
            Debug.LogError("PlayerManager �� PlayerData δ��ʼ�����o��ӛ�ģ�����Q��");
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
            string playerName = playerNameInput.text; // �@ȡݔ������
            if (!string.IsNullOrEmpty(playerName))
            {
                PlayerManager.instance.playerData.isCharacterCreated = true;
                PlayerManager.instance.playerData.PlayerName = playerName;
                Debug.Log($"������Q�Ѵ惦: {playerName}");
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
                Debug.LogWarning("������Q��գ�Ոݔ����Ч���Q��");
                tip.text = "������Q���ܞ�ա�";
            }
        }
        else
        {
            Debug.LogError("PlayerManager �� PlayerData δ��ʼ�����o���惦������Q��");
            tip.text = " PlayerData δ��ʼ�����o���惦������Q��";
        }
    }

    IEnumerator DelayAction()
    {
        // ���t����
        yield return new WaitForSeconds(5f);
    }
    IEnumerator DelayActionB()
    {
        // ���t����
        yield return new WaitForSeconds(3f);
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.StartSceneTransition();

        SceneManager.LoadScene("LV1");
        AudioManager.Instance.PlaySound("tap");

    }

}
