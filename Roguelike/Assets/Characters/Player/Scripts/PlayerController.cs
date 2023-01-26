using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 150f;
    public float maxSpeed = 8f;
    public float idleFriction = 0.9f;
    public string currentState;
    public bool canMove = true;
    public bool isAttackPressed;
    public bool isAttacking;
    public enum CurrentDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public CurrentDirection currentDirection;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 moveInput = Vector2.zero;

    // Animation states
    Dictionary<CurrentDirection, string> current_animation = new Dictionary<CurrentDirection, string>();

    Dictionary<CurrentDirection, string> idle_animations = new Dictionary<CurrentDirection, string>()
    {
        {CurrentDirection.UP, "idle_up"},
        {CurrentDirection.DOWN, "idle_down"},
        {CurrentDirection.LEFT, "idle_left"},
        {CurrentDirection.RIGHT, "idle_right"}
    };

    Dictionary<CurrentDirection, string> walk_animations = new Dictionary<CurrentDirection, string>()
    {
        {CurrentDirection.UP, "walk_up"},
        {CurrentDirection.DOWN, "walk_down"},
        {CurrentDirection.LEFT, "walk_left"},
        {CurrentDirection.RIGHT, "walk_right"}
    };
    Dictionary<CurrentDirection, string> attack_animations = new Dictionary<CurrentDirection, string>()
    {
        {CurrentDirection.UP, "attack_up"},
        {CurrentDirection.DOWN, "attack_down"},
        {CurrentDirection.LEFT, "attack_left"},
        {CurrentDirection.RIGHT, "attack_right"}
    };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAttacking = false;
        isAttackPressed = false;
        currentDirection = CurrentDirection.DOWN;
        current_animation = idle_animations;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(canMove == true && moveInput != Vector2.zero)
        {
            /*
            Accelerate player while run direction is pressed
            Do not exceed max velocity
            */
            rb.velocity = Vector2.ClampMagnitude(rb.velocity + (moveInput * moveSpeed * Time.deltaTime), maxSpeed);

            // Change animation state to movement
            if(moveInput.y < 0)
            {
                currentDirection = CurrentDirection.DOWN;
            }
            else if(moveInput.y > 0)
            {
                currentDirection = CurrentDirection.UP;
            }
            else if(moveInput.x < 0)
            {
                currentDirection = CurrentDirection.LEFT;
            }
            else if(moveInput.x > 0)
            {
                currentDirection = CurrentDirection.RIGHT;
            }
            current_animation = walk_animations;
        }
        else
        {
            // Linearly interpolate between current velocity and zero
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);

            // Change animation state to idle
            if(!isAttacking)
            {
                current_animation = idle_animations;
            }
        }

        if(isAttackPressed)
        {
            isAttackPressed = false;

            if(!isAttacking)
            {
                isAttacking = true;
                current_animation = attack_animations;
            }
        }

        ChangeAnimationState(current_animation[currentDirection]);
    }

    void OnMove(InputValue moveValue)
    {
        moveInput = moveValue.Get<Vector2>();
    }

    void OnPrimary()
    {
        isAttackPressed = true;
    }

    void LockMovement()
    {
        canMove = false;
    }

    void AttackComplete()
    {
        isAttacking = false;
        UnlockMovement();
    }

    void UnlockMovement()
    {
        canMove = true;
    }

    void ChangeAnimationState(string newState)
    {
        // Stop animation interrupting itself
        if (currentState == newState) return;

        // Play animation
        animator.Play(newState);

        currentState = newState;
    }
}
