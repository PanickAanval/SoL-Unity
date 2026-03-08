using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Rigidbody2D rbRobot;
    [Header("Movement")]
    public float moveSpeed = 5f;

    float horizontalMovement;
    float horizontalMovementRobot;


    [Header("Jumping")]
    public float jumpPower = 10f;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("Dash")]
    public float dashSpeed = 10f;
    public bool isDashing = false;
    public float dashDuration = 10f;
    public bool canDash = true;
    public float dashDir = 1f;

    [Header("Active")]
    public bool active = true;
    public bool activeRobot = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dashDir = horizontalMovement;
        if (!isDashing)
        {
            Vector2 newVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = newVelocity;
        }
        Vector2 newVelocityRobot = new Vector2(horizontalMovementRobot * moveSpeed, rbRobot.linearVelocity.y);
        rbRobot.linearVelocity = newVelocityRobot;
        Gravity();
        //if (Mouse.current.leftButton.wasPressedThisFrame)
        //{
        //    rb.position = Mouse.current.position.ReadValue();
        //}
    }

    private void Gravity()
    {
        if (isDashing) return;
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (active)
        {
            horizontalMovement = context.ReadValue<Vector2>().x;
        }
    }

    public void Move_Robot(InputAction.CallbackContext context)
    {
        if (activeRobot)
        {
            horizontalMovementRobot = context.ReadValue<Vector2>().x;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (active)
        {
            
            if (isGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                //    if (context.performed)
                //    {
                //        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                //    }
                //}

                //else if (context.canceled)
                //{
                //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.8f);
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (active && canDash && !isDashing)
        {
            canDash = false;
            Debug.Log("dash ingeklikt");
            isDashing = true;
            var originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            rb.linearVelocityX = dashSpeed * dashDir;

            StartCoroutine(dashRoutine(originalGravity));

        }
    }

    IEnumerator dashRoutine(float originalGravity)
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.gravityScale = originalGravity;
    }
    //public void mouseMove(InputAction.CallbackContext context)
    //{
    //    if (Mouse.current.leftButton.wasPressedThisFrame)
    //    {
    //        rb.position = Mouse.current.position.ReadValue();
    //    }
    //}

    public void Swap(InputAction.CallbackContext context)
    {
        active = !active;
        activeRobot = !activeRobot;
    }
    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            canDash = true;
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }
}
