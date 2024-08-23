using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float turnSpeed = 20f;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 movement;
    private Quaternion rotation = Quaternion.identity;
    private bool isFPS = false;
	[SerializeField] private CinemachineVirtualCamera fpsCamera;
    [SerializeField] private CinemachineFreeLook tpsCamera;
	[SerializeField] private GameObject head;

    private float xRotation = 0f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float moveSpeed = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        SwitchToTPS();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isFPS)
                SwitchToTPS();
            else
                SwitchToFPS();
        }

        if (isFPS)
            HandleMovementFPS();
        else
            HandleMovementTPS();
    }

    void SwitchToFPS()
    {
		fpsCamera.Priority = 1;
		tpsCamera.Priority = 0;
        isFPS = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void SwitchToTPS()
    {
		tpsCamera.Priority = 1;
		fpsCamera.Priority = 0;
        isFPS = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HandleMovementTPS()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
        rotation = Quaternion.LookRotation(desiredForward);

        if (isWalking)
        {
            // 루트 모션에 따라 이동
            rb.MovePosition(rb.position + movement * animator.deltaPosition.magnitude);
            rb.MoveRotation(rotation);
        }
    }

    void HandleMovementFPS()
    {
		Vector3 cameraForward = fpsCamera.transform.forward;
        cameraForward.y = 0f; // Y축 회전을 제외하여 수평 회전만 적용
        cameraForward.Normalize();

        if (cameraForward.magnitude > 0.1f)
        {
            rotation = Quaternion.LookRotation(cameraForward);
            rb.MoveRotation(rotation);
        }

        // Z 키를 눌러 전진
        if (Input.GetKey(KeyCode.Z))
        {
            animator.SetBool("IsWalking", true);
            rb.MovePosition(transform.position + cameraForward * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    void OnAnimatorMove()
    {
        // TPS 모드에서만 루트 모션 적용
        // if (!isFPS)
        // {
            rb.MovePosition(rb.position + movement * animator.deltaPosition.magnitude);
            rb.MoveRotation(rotation);
        // }
    }
}