using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTapFX : MonoBehaviour
{

    public GameObject effectPrefab; // 拖入要顯示的特效預製件                                 
    public Texture2D customCursorA;  // 用來存放自定義的滑鼠圖片
    public Texture2D customCursorB;
    public Vector2 hotspot = Vector2.zero;  // 定義滑鼠圖片熱點位置（相對於圖片左上角的偏移）

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 檢測滑鼠左鍵點擊
        {
            //SpawnEffect();        // 設定自定義滑鼠圖片
            Cursor.SetCursor(customCursorA, hotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnApplicationQuit();
        }
    }

    /*void SpawnEffect()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // 設定 z 軸，確保特效可見（視場深度）
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        GameObject effectInstance = Instantiate(effectPrefab, worldPosition, Quaternion.identity);
        Destroy(effectInstance, 0.5f); // 2秒後銷毀特效
    }*/

    void OnApplicationQuit()
    {
        // 程式結束時恢復預設滑鼠圖片
        Cursor.SetCursor(customCursorB, Vector2.zero, CursorMode.Auto);
    }
}
