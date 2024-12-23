using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;          // �Ƅ��ٶ�
    public float rotationSpeed = 10f;    // ���D�ٶ�
    public Animator playerAnimator;      // ��҄Ӯ�������

    [Header("Camera Settings")]
    public Transform cameraTransform;     // �zӰ�C�� Transform
    public float mouseSensitivity = 100f; // �����`����
    public float cameraDistance = 3f;     // ��ʼ�zӰ�C���x
    public float cameraHeight = 1f;     // �zӰ�C�߶�
    public float cameraSmoothTime = 0.1f; // �zӰ�Cƽ���^�ɕr�g
    public float autoResetDelay = 2f;     // �o���ƕr�Ԅ�����ҕ�ǵ����t�r�g

    [Header("Camera Distance Settings")]
    public float minCameraDistance = 1.5f; // �zӰ�C��С���x
    public float maxCameraDistance = 6f;   // �zӰ�C�����x
    public float zoomSpeed = 2f;           // �L݆�{�����x�ٶ�

    private Vector3 cameraOffset;         // �zӰ�C������ҵ�ƫ��
    private float autoResetTimer = 0f;    // �Ԅ�����ҕ�ǵ�Ӌ�r��
    private float mouseX, mouseY;         // ����ݔ��



    public GameObject seat;               // ��λ�Ŀ���� (����Ħ��?�ĸ����)
    public Camera playerCamera;          // ��ҵ�һ�˷Q�zӰ�C
    public Camera bikeCamera;            // �C?�zӰ�C
    public GameObject player;             // ������
    static public bool isRiding = false;       // �Д��Ƿ������T��
    public GameObject BIKEObject;
    private Rigidbody rb;
    public GameObject BikeUI;
    public Animator PlayerAnimator;
    public Rigidbody bikeRigidbody;      // Ħ��܇���w (�������[�����)
    private void Start()
    {
        BIKEObject.GetComponent<BIKE>().enabled = false;
        rb = GetComponent<Rigidbody>();
        bikeRigidbody.isKinematic = true;
        //Cursor.lockState = CursorLockMode.Locked; // �[�ػ���K�i��
        cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F ))
        {
            if (isRiding && SpeedCalculator.isNearGasStation != true)
            {
                //AudioManager.Instance.StopMotoSound();

                DismountBike(); // ��܇

                BikeUI.SetActive(true);
            }
            else
            {
                // �z�y�Ƿ񿿽�Ħ��܇
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // �돽2�Ĺ����z�y
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Bike"))
                    {
                        MountBike(); // ��܇
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
    //        MountBike();  // ��?
    //        BikeUI.SetActive(false);
    //    }
    //}

    void MountBike()
    {
        PlayerAnimator.SetBool("isMoved", false);

        playerAnimator.SetBool("isRiding", true);

        bikeRigidbody.isKinematic = false; // ���Ħ��܇���oֹ��B
        rb.isKinematic = true;
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;


        player.transform.SetParent(seat.transform);

        // �����λ�ú����D�O�Þ���λ�ĳ�ʼλ���c���D
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

        bikeRigidbody.isKinematic = true; // Ħ��܇�oֹ��B
        rb.isKinematic = false; // �֏���ҵ������B
        // ȡ����ҵĸ����P�S
        player.transform.SetParent(null);
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;

        // �ГQ�zӰ�C���_����һ�˷Q���P�]�C?�zӰ�C
        playerCamera.gameObject.SetActive(true);
        bikeCamera.gameObject.SetActive(false);

        BIKEObject.GetComponent<BIKE>().enabled = false;

        isRiding = false;
    }

    private void PlayerMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // �����Ƅ�
        float moveZ = Input.GetAxis("Vertical");   // ǰ���Ƅ�

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Ӌ��Ŀ�����D�Ƕ�
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // ƽ�����D���
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            // �Ƅ����
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

            playerAnimator.SetBool("isMoved", true);
            autoResetTimer = 0f; // ����в���������Ӌ�r��
        }
        else
        {
            playerAnimator.SetBool("isMoved", false);
        }
    }

    private void HandleCameraControl()
    {
        // ����ݔ�����ҕ��
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -35f, 60f); // ���ƴ�ֱҕ��

        // �ӑB���zӰ�Cƫ��
        cameraOffset = new Vector3(0,- cameraHeight, -cameraDistance);

        Vector3 desiredPosition = transform.position + Quaternion.Euler(0, mouseX, 0) * cameraOffset;
        desiredPosition.y = transform.position.y + cameraHeight;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothTime);

        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f); // �i���zӰ�Cעҕ����^��
    }

    private void HandleCameraZoom()
    {
        // ʹ�ÝL݆�{���zӰ�C���x
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            cameraDistance -= scrollInput * zoomSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, minCameraDistance, maxCameraDistance); // ���ƾ��x����
        }
    }
}
