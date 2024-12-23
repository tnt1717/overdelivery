using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuildingPhotoManager : MonoBehaviour
{
    [System.Serializable]
    public class BuildingPhotoPair
    {
        public Transform building;      // ���B��� Transform
        public GameObject photoObject;  // ��������Ƭ���
        public string sceneName;        // �P�����Q�������� Unity ������
    }

    public GameObject tip;              // ��ʾ UI ���
    public Text tipText;                // ��ʾ�ı�Ԫ��
    public List<BuildingPhotoPair> buildingPhotoPairs; // �惦���B�c��Ƭ���䌦
    public Transform player;            // �������� Transform
    public float detectionRange = 8f;   // �z�y���x����

    private void Update()
    {
        bool isTipActive = false;       // ׷ۙ��ʾ�@ʾ��B

        foreach (var pair in buildingPhotoPairs)
        {
            // Ӌ������cÿ�����B��֮�g�ľ��x
            float distanceToPlayer = Vector3.Distance(pair.building.position, player.position);

            // �������ڙz�y�����ȣ��@ʾ��������Ƭ����t�[��
            if (distanceToPlayer <= detectionRange)
            {
                pair.photoObject.SetActive(true);  // �@ʾ��Ƭ

                // �@ȡԓ�P�������Q���P�����֣����� "LV1"��
                string currentLevel = pair.sceneName;

                // �z��ǰһ�P�������ǔ���
                bool canEnterLevel = true;
                int previousLevelStars = 0;

                // �O��������r�����P�����Q�� "clothshop" �� "motoshop"�����S�M��
                if (currentLevel == "clothshop" || currentLevel == "motoshop")
                {
                    canEnterLevel = true;
                }
                else if (currentLevel != "LV1") // LV2 ��������P����Ҫ�z��
                {
                    string previousLevel = "LV" + (int.Parse(currentLevel.Substring(2)) - 1);
                    if (PlayerManager.instance.playerData.levelStars.TryGetValue(previousLevel, out previousLevelStars))
                    {
                        canEnterLevel = previousLevelStars > 0;
                    }
                    else
                    {
                        canEnterLevel = false; // ����Ҳ���ǰһ�P�����ǔ������tҕ��δ���i
                    }
                }

                // �����Ƿ���i���@ʾ��ʾ
                if (canEnterLevel)
                {
                    tipText.text = "����F�M��";
                    isTipActive = true;

                    // �z�y F �I�Ƿ񱻰��K�ҿ����M���P��
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // ���d����
                        Debug.Log($"Loading level: {pair.sceneName}");
                        AudioManager.Instance.PlaySound("tap");
                        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
                        transitionManager.StartSceneTransition();
                        StartCoroutine(Delay());
                        SceneManager.LoadScene(pair.sceneName);
                    }
                }
                else
                {
                    tipText.text = "δ���i";
                    isTipActive = true;
                }
            }
            else
            {
                pair.photoObject.SetActive(false); // �[����Ƭ
            }
        }

        // ����yһ�O����ʾ���@ʾ��B
        tip.SetActive(isTipActive);
    }
    IEnumerator Delay()
    {
        // ���t����
        yield return new WaitForSeconds(2f);

        // �������t��Ą���
        
    }

}
