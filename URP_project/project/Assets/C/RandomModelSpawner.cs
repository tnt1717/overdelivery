using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] models; // 存儲模型的陣列
    [SerializeField] private Transform[] spawnPoints; // 指定的四個位置

    void Start()
    {
        if (models.Length == 0 || spawnPoints.Length < 4)
        {
            Debug.LogError("請確保模型和生成點正確設置！");
            return;
        }

        //SpawnRandomModels();
    }

    /// <summary>
    /// 在指定位置隨機生成模型
    /// </summary>
    public void SpawnRandomModels()
    {
        // 用於確保不重複生成的模型列表
        var availableModels = new System.Collections.Generic.List<GameObject>(models);

        // 隨機選擇並實例化模型
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (availableModels.Count == 0)
            {
                Debug.LogWarning("模型數量不足以滿足生成需求！");
                break;
            }

            // 隨機選擇模型
            int randomIndex = Random.Range(0, availableModels.Count);
            GameObject selectedModel = availableModels[randomIndex];

            // 在對應生成點生成模型
            Instantiate(selectedModel, spawnPoints[i].position, Quaternion.identity);

            // 移除已使用的模型，避免重複
            availableModels.RemoveAt(randomIndex);
        }
    }
}
