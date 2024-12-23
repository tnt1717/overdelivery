using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public Material skyboxMaterial;  // �Á텢��������պв��|
    public float transitionSpeed = 2f; // ׃���ٶ� (ÿ���׃���ĴΔ�)

    private float transitionValue = 0f; // ��ǰ���^��ֵ
    private bool isIncreasing = true; // �Á����������߀�ǜp��

    void Update()
    {
        // ÿ��Ӌ���^��ֵ
        if (isIncreasing)
        {
            transitionValue += Time.deltaTime * transitionSpeed / 60f; // ���r�g�D�Q����
            if (transitionValue >= 0.8f)
            {
                transitionValue = 0.8f; // �_�������^0.6
                isIncreasing = false; // ��׃��B��p��
            }
        }
        else
        {
            transitionValue -= Time.deltaTime * transitionSpeed / 60f; // ���r�g�D�Q����
            if (transitionValue <= 0.4f)
            {
                transitionValue = 0.4f; // �_�������0
                isIncreasing = true; // ��׃��B������
            }
        }

        // ������պе� CubemapTransition ֵ
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_CubemapTransition", transitionValue);
        }
    }
}
