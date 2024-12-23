using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] models; // �惦ģ�͵����
    [SerializeField] private Transform[] spawnPoints; // ָ�����Ă�λ��

    void Start()
    {
        if (models.Length == 0 || spawnPoints.Length < 4)
        {
            Debug.LogError("Ո�_��ģ�ͺ������c���_�O�ã�");
            return;
        }

        //SpawnRandomModels();
    }

    /// <summary>
    /// ��ָ��λ���S�C����ģ��
    /// </summary>
    public void SpawnRandomModels()
    {
        // ��춴_�������}���ɵ�ģ���б�
        var availableModels = new System.Collections.Generic.List<GameObject>(models);

        // �S�C�x��K������ģ��
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (availableModels.Count == 0)
            {
                Debug.LogWarning("ģ�͔��������ԝM����������");
                break;
            }

            // �S�C�x��ģ��
            int randomIndex = Random.Range(0, availableModels.Count);
            GameObject selectedModel = availableModels[randomIndex];

            // �ڌ��������c����ģ��
            Instantiate(selectedModel, spawnPoints[i].position, Quaternion.identity);

            // �Ƴ���ʹ�õ�ģ�ͣ��������}
            availableModels.RemoveAt(randomIndex);
        }
    }
}
