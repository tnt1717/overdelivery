using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject targetObject; // �ݭn�������

    void Start()
    {
        // ��� targetObject �W�� TargetScript �ñN��T��
        targetObject.GetComponent<BIKE>().enabled = false;
    }

    void Update()
    {
        // ���U�ť���ҥ� TargetScript
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetObject.GetComponent<BIKE>().enabled = true;
            Debug.Log("TargetScript �w�ҥ�");
        }

        // ���U F ��T�� TargetScript
        if (Input.GetKeyDown(KeyCode.F))
        {
            targetObject.GetComponent<BIKE>().enabled = false;
            Debug.Log("TargetScript �w�T��");
        }
    }
}
