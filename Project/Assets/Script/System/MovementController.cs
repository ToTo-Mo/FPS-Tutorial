using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // 스피드
    [Header("Speed")]
    [SerializeField]
    [Tooltip("Drag & Drop player rigidbody component")]
    private Rigidbody rigidbody;

    [SerializeField]
    [Tooltip("Character walk speed, default = 5")]
    private float walkSpeed = 5f;

    [SerializeField]
    [Tooltip("Character run speed, default = 10")]
    private float runSpeed = 10f;
    [SerializeField]
    [Tooltip("Character crouch speed, default = 4")]
    private float crouchSpeed = 4f;
    private float currentSpeed;

    // 앉기
    [Header("Crouch")]
    [SerializeField]
    private Vector3 crouchPos;
    [SerializeField]
    private float crouchSmoohValue = 0.1f;
    private Vector3 originPos;
    private Vector3 currentCrouchPos;

    // 점프
    [Header("Jump")]
    [SerializeField]
    [Tooltip("Drag & Drop player capsuleCollider")]
    private CapsuleCollider capsuleCollider;
    [SerializeField]
    [Tooltip("player jump force")]
    private float jumpForce = 1f;
    [SerializeField]
    [Tooltip("미세하게 ground 조정하는 값")]
    private float distanceOffset = 0.1f;

    // 설정 값
    [Header("Move Setting")]
    [SerializeField]
    [Tooltip("True, hold running state")]
    private bool isHoldRunButton;

    // 상태 변수
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // 1인칭 카메라
    [Header("Camera")]
    [SerializeField]
    [Tooltip("First person Camera")]
    private Camera FPSCamera;
    [SerializeField]
    [Tooltip("Camera sensitivity")]
    private float sensitivity = 100f;
    [SerializeField]
    [Tooltip("Limit angle looking up")]
    private float topClamp = 90f;
    [SerializeField]
    [Tooltip("Limit angle looking down")]
    private float bottomClamp = -90f;
    private float rotationX = 0f;


    // Start is called before the first frame update
    void Start()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        Cursor.lockState = CursorLockMode.Locked;

        originPos = FPSCamera.transform.localPosition;
        currentCrouchPos = originPos;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        IsGround();
        LookAt();
        Crouch();
        Jump();
        Move();
    }

    private void Crouch()
    {
        if (Input.GetButton("Crouch") && !isRun && isGround)
        {
            Crouching();
        }

        if(Input.GetButtonUp("Crouch")){
            CrouchingCancel();
        }

        StartCoroutine(CrouchCorutine());
    }

     private void Crouching()
    {
        isCrouch = true;
        currentSpeed = crouchSpeed;
        currentCrouchPos = crouchPos;
    }

    private void CrouchingCancel()
    {
        isCrouch = false;
        currentSpeed = walkSpeed;
        currentCrouchPos = originPos;
    }

    IEnumerator CrouchCorutine(){
        Vector3 position = FPSCamera.transform.localPosition;

        while(position != currentCrouchPos + Vector3.one * Mathf.Epsilon){
            position  = Vector3.Lerp(position, currentCrouchPos, crouchSmoohValue);
            FPSCamera.transform.localPosition = position;

            yield return null;
        }

        FPSCamera.transform.localPosition = currentCrouchPos;
    }

    private void Run()
    {
        if (isHoldRunButton)
        {
            if (Input.GetButton("Run"))
            {
                Running();
            }
            if (Input.GetButtonUp("Run"))
            {
                RunningCancel();
            }

        }
        else
        {
            if (Input.GetButtonDown("Run") && !isRun)
            {
                Running();
            }
            else if (Input.GetButtonDown("Run") && isRun)
            {
                RunningCancel();
            }
        }
    }


    // 달리기
    private void Running()
    {
        if(isCrouch)
            CrouchingCancel();

        isRun = true;
        currentSpeed = runSpeed;
    }

    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        currentSpeed = walkSpeed;
    }

    // Ground 체크
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + distanceOffset);
    }

    // 점프
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            rigidbody.velocity = transform.up * jumpForce;
        }
    }

    // 캐릭터 움직임
    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Run();

        // forward direction + right direction
        // 정규화 시키지 않는다면, 대각선 방향으로 이동할 때 속도가 2배가 됨.
        Vector3 direction = (transform.forward * vertical + transform.right * horizontal).normalized;
        rigidbody.MovePosition(rigidbody.position + (direction * currentSpeed * Time.deltaTime));
    }

    // 1인칭 카메라 회전
    private void LookAt()
    {
        float x = Input.GetAxisRaw("Mouse Y");
        float y = Input.GetAxisRaw("Mouse X");

        rotationX -= x * sensitivity;
        rotationX = Mathf.Clamp(rotationX, bottomClamp, topClamp);

        Vector3 rotationY = new Vector3(0f, y, 0f) * sensitivity;

        FPSCamera.transform.localEulerAngles = new Vector3(rotationX, 0f, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotationY));
    }
}
