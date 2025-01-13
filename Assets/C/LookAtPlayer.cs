using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject player; // 玩家物件的 Transform


    private void Start()
    {
        player = GameObject.Find("Player");
        
    }

    private void Update()
    {
        if (player != null)
        {
            // 計算從自身到玩家的方向
            Vector3 direction = player.transform.position - transform.position;

            // 移除垂直方向 (y 軸) 的影響，保持水平面旋轉
            direction.y = 0;

            // 確保方向不是零向量，避免錯誤
            if (direction != Vector3.zero)
            {
                // 使物件面朝玩家
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
