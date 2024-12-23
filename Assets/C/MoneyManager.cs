using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int totalCompleted; // ��ɿ���
    public int totalErrors;    // �e�`����
    public int totalTimeouts;  // ���r����
    public int total;

    public CanvasGroup canvasGroup;
    public Text coin;
    public int totalEarnings;  // ������

    // ÿ�Pӆ�ε������c�۷�
    private const int completionReward = 100;
    private const int errorPenalty = 30;
    private const int timeoutPenalty = 50;

    public float fadeDuration = 1f; // ����/�����ĳ��m�r�g
    public float moveSpeed = 50f; // �Ƅӵ��ٶ�
    public float moveDistance = 100f; // �Ƅӵľ��x
    public float delayBeforeFadeOut = 1f; // ������ȴ��ĕr�g

    private bool isFadingIn = true; // ���Ƶ���/����
    private bool isMoving = false; // �Ƿ��_ʼ�Ƅ�
    private Vector3 initialPosition; // ��ʼλ��
    private Vector3 targetPosition; // Ŀ��λ��
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
    /// ��ɿ�����һ���K�������롣
    /// </summary>
    public void AddCompletion()
    {
        //AudioManager.Instance.PlaySound("coin");
        StartCoroutine(FadeInAndMove());

        total++;
        totalCompleted++;
        totalEarnings += completionReward;
        Debug.Log($"��ɿ���: {totalCompleted}, ������: {totalEarnings}");
    }

    /// <summary>
    /// �e�`������һ���K�۳��������~��
    /// </summary>
    public void AddError()

    {
        //AudioManager.Instance.PlaySound("coin");
        StartCoroutine(FadeInAndMove());

        total++;

        totalErrors++;
        totalEarnings += errorPenalty;
        Debug.Log($"�e�`����: {totalErrors}, ������: {totalEarnings}");
    }

    /// <summary>
    /// ���r������һ���K�۳��������~��
    /// </summary>
    public void AddTimeout()
    {
        //AudioManager.Instance.PlaySound("coin");
        StartCoroutine(FadeInAndMove());

        total++;

        totalTimeouts++;
        totalEarnings += timeoutPenalty;
        Debug.Log($"���r����: {totalTimeouts}, ������: {totalEarnings}");
    }

    /// <summary>
    /// �Y�㿂���룬Ӌ��K������K���~��
    /// </summary>
    /// <returns>��K������~</returns>
    public int CalculateFinalEarnings()
    {
        //(int totalOrders, int correctOrders, int errorOrders, int timeoutOrders, int income)
        Debug.Log($"�Y�㿂���~: {totalEarnings} (����: {total},(���: {totalCompleted}, �e�`: {totalErrors}, ���r: {totalTimeouts})");
        starRatingManager.SetStarRating(total, totalCompleted, totalErrors, totalTimeouts, totalEarnings);
        return totalEarnings;
    }
    private System.Collections.IEnumerator FadeInAndMove()
    {
        // ����
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
        AudioManager.Instance.PlaySound("coin");

        canvasGroup.alpha = 1f;
        isMoving = true; // �_ʼ�Ƅ�
        yield return new WaitForSeconds(delayBeforeFadeOut);

        // ����
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        isMoving = false;
    }
}