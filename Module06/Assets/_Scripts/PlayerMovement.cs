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
    private bool isWASD = false;
	[SerializeField] private CinemachineVirtualCamera fpsCamera;
    [SerializeField] private CinemachineFreeLook tpsCamera;
    [SerializeField] private Camera mainCamera;
	[SerializeField] private GameObject head;

    [SerializeField] private float moveSpeed = 5f;
    private AudioSource walkSound;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        walkSound = GetComponent<AudioSource>();
        SwitchToTPS();
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.IsPlaying())
            return;
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isFPS)
                SwitchToTPS();
            else
                SwitchToFPS();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            isWASD = !isWASD;
            if (isWASD && isFPS)
                UIManager.instance.SetViewText("FPS View (WASD)");
            else if (isFPS)
                UIManager.instance.SetViewText("FPS View");
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
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
        tpsCamera.GetComponent<CameraObstacleHandler>().SwitchToFPS();
        if (isWASD)
            UIManager.instance.SetViewText("FPS View (WASD)");
        else
            UIManager.instance.SetViewText("FPS View");
    }

    void SwitchToTPS()
    {
		tpsCamera.Priority = 1;
		fpsCamera.Priority = 0;
        isFPS = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("Player");
        tpsCamera.GetComponent<CameraObstacleHandler>().SwitchToTPS();
        UIManager.instance.SetViewText("TPS View");
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
            rb.MovePosition(rb.position + movement * animator.deltaPosition.magnitude);
            rb.MoveRotation(rotation);
            if (!walkSound.isPlaying)
                walkSound.Play();
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            walkSound.Stop();
        }
    }

    void HandleMovementFPS()
    {
		Vector3 cameraForward = fpsCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        if (cameraForward.magnitude > 0.1f)
        {
            rotation = Quaternion.LookRotation(cameraForward);
            rb.MoveRotation(rotation);
        }

        if (!isWASD)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                animator.SetBool("IsWalking", true);
                rb.velocity = cameraForward * moveSpeed;
                if (!walkSound.isPlaying)
                    walkSound.Play();
            }
            else if (Input.GetKey(KeyCode.X))
            {
                animator.SetBool("IsWalking", true);
                rb.velocity = -cameraForward * moveSpeed;
                if (!walkSound.isPlaying)
                    walkSound.Play();
            }
            else
            {
                walkSound.Stop();
                animator.SetBool("IsWalking", false);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else // for debugging purposes
        {
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("IsWalking", true);
                rb.velocity = cameraForward * moveSpeed;
                if (!walkSound.isPlaying)
                    walkSound.Play();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("IsWalking", true);
                rb.velocity = -cameraForward * moveSpeed;
                if (!walkSound.isPlaying)
                    walkSound.Play();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetBool("IsWalking", true);
                rb.velocity = -fpsCamera.transform.right * moveSpeed;
                if (!walkSound.isPlaying)
                    walkSound.Play();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetBool("IsWalking", true);
                rb.velocity = fpsCamera.transform.right * moveSpeed;
                if (!walkSound.isPlaying)
                    walkSound.Play();
            }
            else
            {
                animator.SetBool("IsWalking", false);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                walkSound.Stop();
            }
        }
        
    }

    void OnAnimatorMove()
    {
        if (!isFPS)
        {
            rb.MovePosition(rb.position + movement * animator.deltaPosition.magnitude);
            rb.MoveRotation(rotation);
        }
    }
}