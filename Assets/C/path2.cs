using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class path2 : MonoBehaviour
{
    public Transform player;               // 玩家的位置
    private Transform target;              // 目標物件
    private LineRenderer lineRenderer;     // 用於渲染引導線
    private NavMeshPath navPath;           // NavMesh 路徑

    public float updateFrequency = 0.5f;   // 更新路徑的頻率 (秒)
                                           // Start is called before the first frame update

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        navPath = new NavMeshPath();

        // 設置 LineRenderer 的屬性
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.positionCount = 0;

        // 啟動協程等待目標物件生成
        StartCoroutine(WaitForTarget());
    }

    // 協程：等待目標物件生成並開始更新路徑
    private IEnumerator WaitForTarget()
    {
        while (target == null)
        {
            GameObject targetObject = GameObject.FindWithTag("order_end");


            if (targetObject != null)
            {
                target = targetObject.transform;
                StartCoroutine(UpdatePathRoutine()); // 開始更新路徑
            }
            else
            {

                yield return new WaitForSeconds(0.3f); // 每0.5秒重複檢測
            }
        }
    }

    // 使用協程定期更新路徑
    private IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            UpdatePath();
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    // 計算路徑並更新 LineRenderer
    private void UpdatePath()
    {
        if (target != null && NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, navPath))
        {
            // 設置 LineRenderer 的點數
            lineRenderer.positionCount = navPath.corners.Length;

            // 將每個路徑點設置到 LineRenderer
            for (int i = 0; i < navPath.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, navPath.corners[i]);
            }
        }
        else
        {
            // 如果無法計算路徑，則隱藏引導線
            lineRenderer.positionCount = 0;
        }
    }
}
