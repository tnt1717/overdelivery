using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public RectTransform miniMapImage;  // 用于显示小地图的 UI 元素
    public Transform player;            // 玩家物体（玩家的 Transform）
    public Camera miniMapCamera;        // 小地图的摄像机
    public GameObject miniMapPlayer;    // 小地图中的玩家图标
    public float mapSizeX = 100f;        // 世界单位与小地图大小的比例X
    public float mapSizeY = 100f;        // 世界单位与小地图大小的比例Y

    public Vector2 miniMapOffset = new Vector2(50, 50);  // 小地图的偏移量

    void Update()
    {
        UpdateMiniMapPosition();
        UpdateMiniMapRotation();
    }

    // 更新小地图的显示位置
    void UpdateMiniMapPosition()
    {
        // 玩家在世界中的位置
        Vector3 playerPosition = player.position;

        // 計算玩家在小地圖上的比例位置
        float x = (playerPosition.x / mapSizeX) * miniMapImage.rect.width;
        float y = (playerPosition.z / mapSizeY) * miniMapImage.rect.height;

        // 更新小地圖圖標位置
        miniMapImage.anchoredPosition = new Vector2(x + miniMapOffset.x, y + miniMapOffset.y);

        // 如果需要更新玩家小地圖上的3D標記
        Vector3 miniMapPlayerPosition = new Vector3(x + miniMapOffset.x, y + miniMapOffset.y, 0);
        // miniMapPlayer.transform.position = miniMapPlayerPosition;
    }

    void UpdateMiniMapRotation()
    {
        // 获取玩家当前的旋转角度
        Quaternion playerRotation = player.rotation;

        // 将玩家旋转同步到小地图图标
        // 只需要同步 Y 轴旋转，但在小地图中对应 Z 轴（UI 2D 的平面）
        miniMapPlayer.transform.rotation = Quaternion.Euler(0, 0, -playerRotation.eulerAngles.y);
    }

}
