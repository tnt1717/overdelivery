using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject player; // �������� Transform


    private void Start()
    {
        player = GameObject.Find("Player");
        
    }

    private void Update()
    {
        if (player != null)
        {
            // Ӌ���������ҵķ���
            Vector3 direction = player.transform.position - transform.position;

            // �Ƴ���ֱ���� (y �S) ��Ӱ푣�����ˮƽ�����D
            direction.y = 0;

            // �_���������������������e�`
            if (direction != Vector3.zero)
            {
                // ʹ����泯���
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
