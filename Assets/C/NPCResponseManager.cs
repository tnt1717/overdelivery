using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCResponseManager : MonoBehaviour
{
    [System.Serializable]
    public class NPCDialogue
    {
        public string npcName; // NPC���Q
        public List<string> normalResponses; // �����ؑ�
        public List<string> delayResponses; // ���t�ؑ�
        public List<string> errorResponses; // �e�`�ؑ�
        public GameObject normalPrefab; // ������B Prefab
        public GameObject delayPrefab; // ���t��B Prefab
        public GameObject errorPrefab; // �e�`��B Prefab
    }

    public LevelManager levelManager;
    public List<NPCDialogue> npcDialogues = new List<NPCDialogue>(); // NPC��Ԓ�Y��
    public Canvas responseCanvas; // �ؑ�����
    public Text responseText; // ��Ԓ�@ʾ�ı�
    public Camera closeUpCamera; // �،��zӰ�C

    public Transform spawnPosition; // λ���O�����������Transform����Q�������c

    private List<GameObject> activeUIs = new List<GameObject>(); // ׷ۙ���õ�UI
    private bool isUIDisabled = false; // UI�Ƿ��ѱ��P�]
    public Camera mainCamera;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        responseCanvas.enabled = false; // �_����ʼ�������P�]
        closeUpCamera.enabled = false; // ��ʼ�P�]�،��zӰ�C
        InitializeNPCDialogues(); // ��ʼ��NPC��Ԓ�Y��
    }

    public void ShowNPCResponse(int npcIndex, float completionTime, float thresholdTime, int errors)
    {
        DisableAllUI();
        mainCamera.enabled = false;
        closeUpCamera.enabled = true;
        closeUpCamera.depth = 2;


        if (npcIndex < 0 || npcIndex >= npcDialogues.Count)
        {
            Debug.LogError("NPC�����oЧ��");
            return;
        }

        NPCDialogue npc = npcDialogues[npcIndex];
        string response = "";
        GameObject npcPrefab = null;

        if (errors > 0)
        {
            response = GetRandomResponse(npc.errorResponses);
            npcPrefab = npc.errorPrefab;
        }
        else if (completionTime <= thresholdTime)
        {
            response = GetRandomResponse(npc.normalResponses);
            npcPrefab = npc.normalPrefab;
        }
        else
        {
            response = GetRandomResponse(npc.delayResponses);
            npcPrefab = npc.delayPrefab;
        }

        responseCanvas.enabled = true;
        responseText.text = response;

        // ����Prefab�K������O����λ��
        if (npcPrefab != null && spawnPosition != null)
        {
            Instantiate(npcPrefab, spawnPosition.position, Quaternion.Euler(0f, -90f, 0f));//Quaternion.identity
        }
        StartCoroutine(wait());

        // �_ʼ���t���[�،�Ԓ���P�]�zӰ�C
        StartCoroutine(HideResultAfterDelay(5f));
    }
    IEnumerator wait()
    {
        while (Input.anyKey)
        {
            yield return null; // ÿ֡���һ��
        }
    }

        private IEnumerator HideResultAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        levelManager.ShowResultUI("1/1", "0/0", "10/10", 300); // ���ŽY�� UI
        closeUpCamera.enabled = false; // �P�]�،��zӰ�C
        closeUpCamera.depth = 0;

        mainCamera.enabled = true;
    }

    private string GetRandomResponse(List<string> responses)
    {
        return responses[Random.Range(0, responses.Count)];
    }

    private void DisableAllUI()
    {
        if (isUIDisabled) return;

        activeUIs.Clear();
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.gameObject.activeSelf && canvas != responseCanvas)
            {
                activeUIs.Add(canvas.gameObject);
                canvas.gameObject.SetActive(false);
            }
        }
        isUIDisabled = true;
    }

    private void RestoreUI()
    {
        foreach (GameObject ui in activeUIs)
        {
            if (ui != null)
            {
                ui.SetActive(true);
            }
        }
        activeUIs.Clear();
        isUIDisabled = false;
    }

    private void InitializeNPCDialogues()
    {
        npcDialogues.Add(new NPCDialogue
        {
            npcName = "�u��",
            normalResponses = new List<string>
            {
                "���|����o�Ұɣ����@߅߀���c���£���ě]�r�g��̫���ˡ���",
                "��������̫���ˣ����ó��T�G�������Y�����;��́��ˣ�����̫�����ˣ����x�㣡��",
                "���ゃ�������ٶ�Ҳ̫���˰ɣ�\n�ҵ����@�N�ã������׌����Щ���M�������ゃ�����Ͱ�����Ҫ���Mһ���ˡ���"
            },
            delayResponses = new List<string>
            {
                "���@Ҳ̫���˰ɣ�\n�@�N����͵������N��ģ�߀�����Լ�ȥ�͏d�����ˣ�ʡ�����M�r�g����",
                "���Ҷ��ʂ�˯�X�ˣ�����́�����Ҫ���ډ��e��ᡣ��",
                "���@�ٶȿ��治�ҹ��S���������ҵ�����Ŀɛ]�@�N�ã�\n�´΄ձظ����c����"
            },
            errorResponses = new List<string>
            {
                "���@�����c���������c�İɣ�\n���N���l���@�N�ͼ����e�`���ǲ����ゃ���e�ˣ���",
                "���@���ͽo�������������İɣ�\n��Ӛ���J��һ�c���´΄e������ˡ���",
                "���@�����c�����ɣ�\n���Ʒζ���Ǫ�һ�o�����e�o�ҁy����"
            },
            normalPrefab = null, // �������ھ�݋���з��� Prefab
            delayPrefab = null,
            errorPrefab = null
        });

        // �@�e�����^�m��������NPC�Y��
    }
}
