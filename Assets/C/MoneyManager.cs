using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int totalCompleted; // 完成總數
    public int totalErrors;    // 錯誤總數
    public int totalTimeouts;  // 超時總數
    public int total;

    public CanvasGroup canvasGroup;
    public Text coin;
    public int totalEarnings;  // 總收入

    // 每筆訂單的收益與扣分
    private const int completionReward = 100;
    private const int errorPenalty = 30;
    private const int timeoutPenalty = 50;

    public float fadeDuration = 1f; // 淡入/淡出的持續時間
    public float moveSpeed = 50f; // 移動的速度
    public float moveDistance = 100f; // 移動的距離
    public float delayBeforeFadeOut = 1f; // 淡入後等待的時間

    private bool isFadingIn = true; // 控制淡入/淡出
    private bool isMoving = false; // 是否開始移動
    private Vector3 initialPosition; // 初始位置
    private Vector3 targetPosition; // 目標位置
    private float timer = 0f;
    private StarRatingManager starRatingManager;

    private void Start()
    {
        starRatingManager = FindObjectOfType<StarRatingManager>();
        canvasGroup.alpha = 0f;
        initialPosition = coin.transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;
    }
    public void Update()
    {
        if (isMoving)
        {
            coin.transform.position = Vector3.MoveTowards(
                coin.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
        if (Input.GetKey(KeyCode.F1)) {
            CalculateFinalEarnings();

            StartCoroutine(FadeInAndMove());

        }
    }
    /// <summary>
    /// 完成總數加一，並增加收入。
    /// </summary>
    public void AddCompletion()
    {
        //AudioManager.Instance.PlaySound("coin");
        StartCoroutine(FadeInAndMove());

        total++;
        totalCompleted++;
        totalEarnings += completionReward;
        Debug.Log($"完成總數: {totalCompleted}, 總收入: {totalEarnings}");
    }

    /// <summary>
    /// 錯誤總數加一，並扣除相應金額。
    /// </summary>
    public void AddError()

    {
        //AudioManager.Instance.PlaySound("coin");
        StartCoroutine(FadeInAndMove());

        total++;

        totalErrors++;
        totalEarnings += errorPenalty;
        Debug.Log($"錯誤總數: {totalErrors}, 總收入: {totalEarnings}");
    }

    /// <summary>
    /// 超時總數加一，並扣除相應金額。
    /// </summary>
    public void AddTimeout()
    {
        //AudioManager.Instance.PlaySound("coin");
        StartCoroutine(FadeInAndMove());

        total++;

        totalTimeouts++;
        totalEarnings += timeoutPenalty;
        Debug.Log($"超時總數: {totalTimeouts}, 總收入: {totalEarnings}");
    }

    /// <summary>
    /// 結算總收入，計算並返回最終金額。
    /// </summary>
    /// <returns>最終收入金額</returns>
    public int CalculateFinalEarnings()
    {
        //(int totalOrders, int correctOrders, int errorOrders, int timeoutOrders, int income)
        Debug.Log($"結算總金額: {totalEarnings} (總數: {total},(完成: {totalCompleted}, 錯誤: {totalErrors}, 超時: {totalTimeouts})");
        starRatingManager.SetStarRating(total, totalCompleted, totalErrors, totalTimeouts, totalEarnings);
        return totalEarnings;
    }
    private System.Collections.IEnumerator FadeInAndMove()
    {
        // 淡入
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
        AudioManager.Instance.PlaySound("coin");

        canvasGroup.alpha = 1f;
        isMoving = true; // 開始移動
        yield return new WaitForSeconds(delayBeforeFadeOut);

        // 淡出
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        isMoving = false;
    }
}