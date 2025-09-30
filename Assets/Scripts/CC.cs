using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public int dashBloodCost = 10;

    private float lastDashTime;
    private float dashTimeRemaining;
    private bool isDashing;
    private Vector3 dashDirection;
    public float rotationSpeed = 10f;
    [HideInInspector] public StaminaController _staminaController;

    private Vector3 _input;
    private Rigidbody rb;

    

    private bool isControlHeld;

    private void Awake()
    {
        _staminaController = GetComponent<StaminaController>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        GatherInput();
        TryDash();
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    private void GatherInput()
    {
        // Check movement input
        Vector2 input = Keyboard.current != null ? new Vector2(
            (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
            (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
        ).normalized : Vector2.zero;

        _input = new Vector3(input.x, 0, input.y);

        isControlHeld = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
    }

    public void PlayerDash()
    {
        _staminaController.Dashing();
    }

    private void TryDash()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            Time.time >= lastDashTime + dashCooldown &&
            _input != Vector3.zero &&
         
            !isControlHeld)
        {
            isDashing = true;
            dashTimeRemaining = dashDuration;
            lastDashTime = Time.time;
            dashDirection = _input.normalized;
   
        }
    }

    private void Move()
    {
        if (isControlHeld) return;
        if (_input == Vector3.zero && !isDashing) return;

        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        Vector3 rotatedInput = isDashing ? isoMatrix.MultiplyPoint3x4(dashDirection) : isoMatrix.MultiplyPoint3x4(_input);
        Vector3 moveDirection = rotatedInput;

        if (isDashing)
        {
            rb.MovePosition(rb.position + moveDirection * dashSpeed * Time.fixedDeltaTime);
            dashTimeRemaining -= Time.fixedDeltaTime;
            if (dashTimeRemaining <= 0f)
                isDashing = false;
        }
        else
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        Vector3 rotatedInput = isoMatrix.MultiplyPoint3x4(_input);
        Quaternion targetRotation = Quaternion.LookRotation(rotatedInput, Vector3.up);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            isDashing = false;
            dashTimeRemaining = 0f;
        }
    }
}
