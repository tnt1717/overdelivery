using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;          // 移動速度
    public float rotationSpeed = 10f;    // 旋轉速度
    public Animator playerAnimator;      // 玩家動畫控制器

    [Header("Camera Settings")]
    public Transform cameraTransform;     // 攝影機的 Transform
    public float mouseSensitivity = 100f; // 滑鼠靈敏度
    public float cameraDistance = 3f;     // 初始攝影機距離
    public float cameraHeight = 1f;     // 攝影機高度
    public float cameraSmoothTime = 0.1f; // 攝影機平滑過渡時間
    public float autoResetDelay = 2f;     // 無控制時自動重置視角的延遲時間

    [Header("Camera Distance Settings")]
    public float minCameraDistance = 1.5f; // 攝影機最小距離
    public float maxCameraDistance = 6f;   // 攝影機最大距離
    public float zoomSpeed = 2f;           // 滾輪調整距離速度

    private Vector3 cameraOffset;         // 攝影機相對玩家的偏移
    private float autoResetTimer = 0f;    // 自動重置視角的計時器
    private float mouseX, mouseY;         // 滑鼠輸入



    public GameObject seat;               // 座位的空物件 (作為摩托?的父物件)
    public Camera playerCamera;          // 玩家第一人稱攝影機
    public Camera bikeCamera;            // 機?攝影機
    public GameObject player;             // 玩家物件
    static public bool isRiding = false;       // 判斷是否正在騎乘
    public GameObject BIKEObject;
    private Rigidbody rb;
    public GameObject BikeUI;
    public Animator PlayerAnimator;
    public Rigidbody bikeRigidbody;      // 摩托車剛體 (新增的遊戲物件)
    private void Start()
    {
        BIKEObject.GetComponent<BIKE>().enabled = false;
        rb = GetComponent<Rigidbody>();
        bikeRigidbody.isKinematic = true;
        //Cursor.lockState = CursorLockMode.Locked; // 隱藏滑鼠並鎖定
        cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F ))
        {
            if (isRiding && SpeedCalculator.isNearGasStation != true)
            {
                //AudioManager.Instance.StopMotoSound();

                DismountBike(); // 下車

                BikeUI.SetActive(true);
            }
            else
            {
                // 檢測是否靠近摩托車
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // 半徑2的範圍檢測
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Bike"))
                    {
                        MountBike(); // 上車
                        //AudioManager.Instance.PlayMotoSound();

                        BikeUI.SetActive(false);

                        break;
                    }
                }
            }
        }

        if (!isRiding)
        {
            PlayerMovement();
            HandleCameraControl();
            HandleCameraZoom();
        }

    }
    //void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Bike") && Input.GetKeyDown(KeyCode.F) && !isRiding)
    //    {
    //        rb.isKinematic = false;
    //        MountBike();  // 上?
    //        BikeUI.SetActive(false);
    //    }
    //}

    void MountBike()
    {
        PlayerAnimator.SetBool("isMoved", false);

        playerAnimator.SetBool("isRiding", true);

        bikeRigidbody.isKinematic = false; // 解除摩托車的靜止狀態
        rb.isKinematic = true;
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;


        player.transform.SetParent(seat.transform);

        // 將玩家位置和旋轉設置為座位的初始位置與旋轉
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
        player.transform.localPosition=seat.transform.localPosition;
        player.transform.localRotation = seat.transform.localRotation;


        playerCamera.gameObject.SetActive(false);
        bikeCamera.gameObject.SetActive(true);
        BIKEObject.GetComponent<BIKE>().enabled = true;

        isRiding = true;
    }

    void DismountBike()
    {
        playerAnimator.SetBool("isRiding",false);
        isRiding = false;

        bikeRigidbody.isKinematic = true; // 摩托車靜止狀態
        rb.isKinematic = false; // 恢復玩家的物理狀態
        // 取消玩家的父子關係
        player.transform.SetParent(null);
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;

        // 切換攝影機：開啟第一人稱，關閉機?攝影機
        playerCamera.gameObject.SetActive(true);
        bikeCamera.gameObject.SetActive(false);

        BIKEObject.GetComponent<BIKE>().enabled = false;

        isRiding = false;
    }

    private void PlayerMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // 左右移動
        float moveZ = Input.GetAxis("Vertical");   // 前後移動

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // 計算目標旋轉角度
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // 平滑旋轉玩家
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            // 移動玩家
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

            playerAnimator.SetBool("isMoved", true);
            autoResetTimer = 0f; // 玩家有操作，重置計時器
        }
        else
        {
            playerAnimator.SetBool("isMoved", false);
        }
    }

    private void HandleCameraControl()
    {
        // 滑鼠輸入控制視角
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -35f, 60f); // 限制垂直視角

        // 動態更新攝影機偏移
        cameraOffset = new Vector3(0,- cameraHeight, -cameraDistance);

        Vector3 desiredPosition = transform.position + Quaternion.Euler(0, mouseX, 0) * cameraOffset;
        desiredPosition.y = transform.position.y + cameraHeight;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothTime);

        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f); // 鎖定攝影機注視玩家頭部
    }

    private void HandleCameraZoom()
    {
        // 使用滾輪調整攝影機距離
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            cameraDistance -= scrollInput * zoomSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, minCameraDistance, maxCameraDistance); // 限制距離範圍
        }
    }
}
