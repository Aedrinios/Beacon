using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FpInput))]
public class FpMovement : MonoBehaviour
{
    #region User Defined properties
    [SerializeField] private float walkSpeed = 2;
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private float jumpSpeed = 5;
    [SerializeField] private float stickToGroundForce = 1;
    [SerializeField] private bool customGravity = false;
    [SerializeField] private Vector3 gravityVector;
    #endregion

    #region Data
    private FpLook mouseLook;
    private CharacterController characterController;
    private FpInput input;
    private float speed = 0f;
    private Vector3 moveDir = Vector3.zero;
    private bool scheduleJump;
    #endregion

    #region Public Properties
    public CollisionFlags CollisionFlags { get; private set; }

    #endregion

    private void OnEnable()
    {
        Init();
        RegisterInputEvents();
    }

    private void OnDisable()
    {
        UnregisterInputEvents();
    }

    private void Update()
    {
        MoveCharacter();
        scheduleJump = false; //Reset
    }

    void Init()
    {
        characterController = GetComponent<CharacterController>();
        mouseLook = GetComponent<FpLook>();

        speed = walkSpeed;

        if (!customGravity)
            gravityVector = Physics.gravity;
    }

    private void MoveCharacter()
    {
        Vector3 desiredMove = transform.forward * input.Vertical + transform.right * input.Horizontal;

        desiredMove = ProjectOnGround(desiredMove);
        desiredMove = Vector3.ClampMagnitude(desiredMove, 1);

        moveDir.x = desiredMove.x * speed;
        moveDir.z = desiredMove.z * speed; //@TODO wont work with different gravity!

        if (characterController.isGrounded)
        {
            moveDir.y = -stickToGroundForce;

            if (scheduleJump)
            {
                moveDir.y = jumpSpeed;
            }
        }
        else //Free fall
        {
            moveDir += gravityVector * Time.deltaTime;
        }

        CollisionFlags = characterController.Move(moveDir * Time.deltaTime);
    }

    Vector3 ProjectOnGround(Vector3 direction)
    {
        Physics.SphereCast(transform.position, characterController.radius,
                            gravityVector.normalized, out RaycastHit hitInfo,
                            characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        return Vector3.ProjectOnPlane(direction, hitInfo.normal);
    }

    #region Input
    void RegisterInputEvents()
    {
        input = GetComponent<FpInput>();
        input.OnJumpButtonDown += Jump;
        input.OnRunButtonDown += StartRunning;
        input.OnRunButtonUp += StopRunning;
    }

    void UnregisterInputEvents()
    {
        input.OnJumpButtonDown -= Jump;
        input.OnRunButtonDown -= StartRunning;
        input.OnRunButtonUp -= StopRunning;
    }

    void Jump()
    {
        scheduleJump = true;
    }

    void StartRunning()
    {
        speed = runSpeed;
    }

    void StopRunning()
    {
        speed = walkSpeed;
    }
    #endregion
}


