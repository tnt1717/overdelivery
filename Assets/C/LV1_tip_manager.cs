using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LV1_tip_manager : MonoBehaviour
{
    public logueManager logueManager;
    public GameObject imageObject;

    // Start is called before the first frame update
    void Start()
    {
        // �z�� logueManager �Ƿ�� null
        if (logueManager == null)
        {
            // ����� null���@ʾ�e�`ӍϢ���Kֹͣ�^�m����
            Debug.LogError("logueManager ��δ�O�ã�Ո�� logueManager �ϵ� Inspector �еę�λ��");
            return; // �@���Kֹ Start �����Ĉ���
        }

        // �[�؈DƬ
        imageObject.SetActive(false);
        Debug.Log("logueManager�O�ã���");

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DisplayImageAndWait()
    {
        // �@ʾ�DƬ
        imageObject.gameObject.SetActive(true);

        // �ȴ���Ұ��������I
        yield return new WaitUntil(() => Input.anyKeyDown);

        // ��Ұ��������I�ᣬ�P�]�DƬ���
        imageObject.gameObject.SetActive(false);
    }
    IEnumerator WaitAndDoSomething()
    {
        // �ȴ�����
        yield return new WaitForSeconds(3f);

        // �ȴ��Y������еĲ���
        Debug.Log("�����ѽ��^ȥ�ˣ�");
        // ���������@�e�������m�Ĳ����������@ʾ�DƬ�򆢄���������
    }
}
