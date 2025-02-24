using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;          // 移動速度
    public float rotationSpeed = 10f;    // 旋轉速度
    public Animator playerAnimator;      // 玩家動畫控制器

    [Header("Bike Settings")]
    public GameObject seat;               // 座位的空物件 (作為摩托的父物件)
    public Camera bikeCamera;            // 機車攝影機
    public GameObject player;             // 玩家物件
    static public bool isRiding = false;  // 判斷是否正在騎乘
    public GameObject BIKEObject;
    private Rigidbody rb;
    public GameObject BikeUI;
    public Animator PlayerAnimator;
    public Rigidbody bikeRigidbody;      // 摩托車剛體

    private void Start()
    {
        BIKEObject.GetComponent<BIKE>().enabled = false;
        rb = GetComponent<Rigidbody>();
        bikeRigidbody.isKinematic = true;
        Cursor.lockState = CursorLockMode.Locked; // 鎖定滑鼠在畫面中央
        Cursor.visible = false; // 隱藏滑鼠指標
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isRiding && SpeedCalculator.isNearGasStation != true)
            {
                DismountBike(); // 下車
                    Cursor.lockState = CursorLockMode.Locked; // 鎖定滑鼠在畫面中央
                    Cursor.visible = false; // 隱藏滑鼠指標

                BikeUI.SetActive(true);
            }
            else
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Bike"))
                    {
                        MountBike(); // 上車
                        Cursor.lockState = CursorLockMode.Confined; // 鎖定滑鼠在畫面中央
                        Cursor.visible = true; // 
                        BikeUI.SetActive(false);
                        break;
                    }
                }
            }
        }

        if (!isRiding)
        {
            PlayerMovement();
        }
    }

    void MountBike()
    {
        PlayerAnimator.SetBool("isMoved", false);
        playerAnimator.SetBool("isRiding", true);

        bikeRigidbody.isKinematic = false;
        rb.isKinematic = true;
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        player.transform.SetParent(seat.transform);
        player.transform.localPosition = seat.transform.localPosition;
        player.transform.localRotation = seat.transform.localRotation;

        bikeCamera.gameObject.SetActive(true);
        BIKEObject.GetComponent<BIKE>().enabled = true;

        isRiding = true;
    }

    void DismountBike()
    {
        playerAnimator.SetBool("isRiding", false);
        isRiding = false;

        bikeRigidbody.isKinematic = true;
        rb.isKinematic = false;
        player.transform.SetParent(null);
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;

        bikeCamera.gameObject.SetActive(false);
        BIKEObject.GetComponent<BIKE>().enabled = false;
    }

    private void PlayerMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Transform cameraTransform = Camera.main.transform; // 取得攝影機 Transform
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0; // 忽略 Y 軸，避免玩家受攝影機俯仰影響
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveZ + right * moveX).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            playerAnimator.SetBool("isMoved", true);
        }
        else
        {
            playerAnimator.SetBool("isMoved", false);
        }
    }
}
