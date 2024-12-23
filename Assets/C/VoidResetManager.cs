using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidResetManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 檢測是否為玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家掉入虛空，重新載入場景...");

            // 獲取當前啟用的場景名稱
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 重新載入當前場景
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
