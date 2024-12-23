using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Color outlineColor = Color.green; // ��݆���ɫ
    public float outlineWidth = 0.01f;       // ��݆������

    private Transform lastHighlighted;       // ��һ�����������
    private Material originalMaterial;       // ԭʼ���|
    private Material outlineMaterial;        // ��݆�����|

    void Start()
    {
       
    }

    void Update()
    {
        
    }

    void RemoveOutline()
    {
        if (lastHighlighted != null)
        {
            Renderer renderer = lastHighlighted.GetComponent<Renderer>();
            if (renderer != null && originalMaterial != null)
            {
                renderer.material = originalMaterial;
            }
            lastHighlighted = null;
        }
    }
}
