using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsUIUpdater : MonoBehaviour
{
    public Sprite noStarSprite; // �]�����ǵĈDƬ
    public Sprite starSprite;   // �����ǵĈDƬ

    private void Start()
    {
        UpdateStarsUI("LV1");
        UpdateStarsUI("LV2");
        UpdateStarsUI("LV3");
        // ���и����P���������{��
    }

    private void UpdateStarsUI(string level)
    {
        // �_�JPlayerManager�Ѽ��d��ȡ����Ҕ���
        PlayerData playerData = PlayerManager.instance.playerData;

        // ȡ��ԓ�P�������ǔ���
        if (playerData.levelStars.TryGetValue(level, out int stars))
        {
            // ����ԓ�P��������UI�����������ʽ�� "LV1_1", "LV1_2", "LV1_3" ��
            for (int i = 1; i <= 3; i++)
            {
                string starObjectName = level + "_" + i;
                GameObject starObject = GameObject.Find(starObjectName);

                if (starObject != null)
                {
                    Image starImage = starObject.GetComponent<Image>();

                    if (starImage != null)
                    {
                        // �������ǔ���Q�DƬ
                        starImage.sprite = i <= stars ? starSprite : noStarSprite;
                    }
                    else
                    {
                        Debug.LogWarning($"{starObjectName} �]�� Image �M����");
                    }
                }
                else
                {
                    Debug.LogWarning($"{starObjectName} δ�ҵ�춈����С�");
                }
            }
        }
        else
        {
            Debug.LogWarning($"�P�� {level} �����ǔ��������������Y���С�");
        }
    }
}
