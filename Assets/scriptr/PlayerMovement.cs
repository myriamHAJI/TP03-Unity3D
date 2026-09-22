using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 6f;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 direction;
    private bool grounded;
    private bool jumpRequested;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        float x = 0f;
        float z = 0f;

        if (keyboard.aKey.isPressed || keyboard.qKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed) x += 1f;
        if (keyboard.wKey.isPressed || keyboard.zKey.isPressed) z += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;

        direction = new Vector3(x, 0f, z).normalized;

        if (keyboard.spaceKey.wasPressedThisFrame && grounded)
            jumpRequested = true;
    }

    void FixedUpdate()
    {
        Vector3 target = direction * speed;
        Vector3 velocity = rb.linearVelocity;

        Vector3 horizontal = Vector3.MoveTowards(
            new Vector3(velocity.x, 0f, velocity.z),
            target,
            20f * Time.fixedDeltaTime
        );

        float vertical = velocity.y;

        if (jumpRequested)
        {
            vertical = jumpSpeed;
            grounded = false;
            jumpRequested = false;
        }

        rb.linearVelocity = new Vector3(
            horizontal.x,
            vertical,
            horizontal.z
        );

        if (animator != null)
        {
            animator.SetFloat("Speed", horizontal.magnitude);
            animator.SetFloat("MotionSpeed", direction.magnitude);
            animator.SetBool("Grounded", grounded);
            animator.SetBool("Jump", !grounded && vertical > 0.1f);
            animator.SetBool("FreeFall", !grounded && vertical < -0.1f);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name != "Ground") return;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                grounded = true;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
            grounded = false;
    }
}