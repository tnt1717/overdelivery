using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtPlayer : MonoBehaviour
{
    public Transform player; // 玩家的位置

    void Update()
    {
        if (player != null)
        {
            // 使畫布面向玩家
            transform.LookAt(player);
        }
    }
}
