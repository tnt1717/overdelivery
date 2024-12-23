using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidResetManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �z�y�Ƿ�����
        if (other.CompareTag("Player"))
        {
            Debug.Log("��ҵ���̓�գ������d�����...");

            // �@ȡ��ǰ���õĈ������Q
            string currentSceneName = SceneManager.GetActiveScene().name;

            // �����d�뮔ǰ����
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
