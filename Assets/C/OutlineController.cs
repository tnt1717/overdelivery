using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Color outlineColor = Color.green; // 外輪廓顏色
    public float outlineWidth = 0.01f;       // 外輪廓寬度

    private Transform lastHighlighted;       // 上一個高亮的物件
    private Material originalMaterial;       // 原始材質
    private Material outlineMaterial;        // 外輪廓材質

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
