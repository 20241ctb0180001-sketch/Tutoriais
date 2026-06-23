using UnityEngine;
using UnityEngine.InputSystem;

public class Player_v : MonoBehaviour
{
    [Header("Unity")]
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] CharacterController playerCharContrl;
    [SerializeField] Transform playerCam;
    private InputAction MoveAct;
    private InputAction JumpAct;

    [Header("Action variables")]
    [SerializeField] private Vector2 MoveAmount;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float rotationDamp = 0.1f;
    [SerializeField] private float turnSmoothingVel;
    [SerializeField] private float VerticalVel = 0f;
    [SerializeField] private float gravidade = -9.8f;
    [SerializeField] private float JumpHeight = 1f;

    void Awake()
    {
        MoveAct = inputActions.FindAction("Move");
        JumpAct = inputActions.FindAction("Jump");
    }

    void Update()
    {
        MoveAmount = MoveAct.ReadValue<Vector2>();
        PMoveRotate();
        Jump();
    }

    private void PMoveRotate()
    {
        Vector3 playerDirection = new Vector3(MoveAmount.x, 0f, MoveAmount.y).normalized;
        Vector3 VerticalMove = new Vector3(0f, VerticalVel, 0f) * Time.deltaTime;
        if (playerDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(playerDirection.x, playerDirection.y) * Mathf.Rad2Deg + playerCam.eulerAngles.y;
            float smoothTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVel, rotationDamp);
            transform.rotation = Quaternion.Euler(0f, smoothTargetAngle, 0f);

            /*Vector3 MoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;*/
            Vector3 MoveDirection = Quaternion.Euler(0f, playerCam.eulerAngles.y, 0f) * new Vector3(MoveAmount.x, 0f, MoveAmount.y).normalized;

            playerCharContrl.Move(MoveDirection.normalized * walkSpeed * Time.deltaTime + VerticalMove);
        }
        else
        {
            playerCharContrl.Move(VerticalMove);
        }
    }

    private void Jump()
    {
        if (playerCharContrl.isGrounded)
        {
            VerticalVel = -1f;
            if (JumpAct.WasPressedThisFrame())
            {
                VerticalVel = JumpHeight;
            }
        }
        else
        {
            VerticalVel += gravidade * Time.deltaTime;
        }
    }
}
