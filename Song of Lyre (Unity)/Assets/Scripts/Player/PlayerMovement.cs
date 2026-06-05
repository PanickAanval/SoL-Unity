using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public bool notDialogue = true;
    public GameObject LyreAnimator;
    private Animator animator;
    public Rigidbody2D rb;
    public Rigidbody2D rbRobot;
    [Header("Movement")]
    public float moveSpeed = 5f;
    public bool isWalking = false;

    float horizontalMovement;
    float horizontalMovementRobot;


    [Header("Jumping")]
    public float jumpPower = 10f;

    [Header("GroundCheck")]

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(2f, 2f);
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
    public UnityEvent swapToRobot;
    public UnityEvent swapToLyre;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = LyreAnimator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (animator.GetBool("Walking") == true)
        {
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed = 30f;
        }
        if (rb.linearVelocityX != 0)
        {
            ifMoving();
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        //animator.SetFloat("Speed", 2);
        dashDir = horizontalMovement;
        if (!isDashing)
        {
            Vector2 newVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = newVelocity;
        }
        Vector2 newVelocityRobot = new Vector2(horizontalMovementRobot * moveSpeed, rbRobot.linearVelocity.y);
        rbRobot.linearVelocity = newVelocityRobot;
        //Gravity();
        //if (Mouse.current.leftButton.wasPressedThisFrame)
        //{
        //    rb.position = Mouse.current.position.ReadValue();
        //}
    }

    public void Walky()
    {
        animator.SetBool("Walking", true);
    }
    public void NoWalky()
    {
        animator.SetBool("Walking", false);
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
            //animator.SetTrigger("Moving");
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
        if (notDialogue)
        {
            if (active)
            {

                if (isGrounded())
                {
                    //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                    rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
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
    }
    public void ifMoving()
    {
        if (active)
        {
            animator.SetBool("Moving", true);

            if (rb.linearVelocityX < 0)
            {
                //LyreAnimator.GetComponent<Transform>().localScale.z = LyreAnimator.GetComponent<Transform>().localScale.z * -1;
                LyreAnimator.GetComponent<Transform>().localScale = new Vector3(1, 1, -1);
            }
            else
            {
                LyreAnimator.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
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
        if (active == true)
        {
            swapToRobot.Invoke();
        }
        if (activeRobot == true)
        {
            swapToLyre.Invoke();
        }
        active = !active;
        activeRobot = !activeRobot;
    }
    public bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer) && Physics2D.OverlapCircle(groundCheckPos.position, 10, groundLayer, 0))
        {
            canDash = true;
            return true;
        }
        return false;
    }

    public void isInDialogue()
    {
        notDialogue =! notDialogue;
    }

    public void forceNonJumpable()
    {
        if (notDialogue == true)
        {
            notDialogue = false;
        }
    }
    public void forceJumpable()
    {
        if (notDialogue == false)
        {
            notDialogue = true;
        } 
            
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }
}
