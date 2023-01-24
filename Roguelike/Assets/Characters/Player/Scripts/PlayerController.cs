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
    CurrentDirection currentDirection;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 moveInput = Vector2.zero;

    // Animation states
    const string IDLE_UP = "idle_up";
    const string IDLE_DOWN = "idle_down";
    const string IDLE_LEFT = "idle_left";
    const string IDLE_RIGHT = "idle_right";
    const string WALK_UP = "walk_up";
    const string WALK_DOWN = "walk_down";
    const string WALK_LEFT = "walk_left";
    const string WALK_RIGHT = "walk_right";
    const string ATTACK_UP = "attack_up";
    const string ATTACK_DOWN = "attack_down";
    const string ATTACK_LEFT = "attack_left";
    const string ATTACK_RIGHT = "attack_right";

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAttacking = false;
        isAttackPressed = false;
        currentDirection = CurrentDirection.DOWN;
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
                ChangeAnimationState(WALK_DOWN);
                currentDirection = CurrentDirection.DOWN;
            }
            else if(moveInput.y > 0)
            {
                ChangeAnimationState(WALK_UP);
                currentDirection = CurrentDirection.UP;
            }
            else if(moveInput.x < 0)
            {
                ChangeAnimationState(WALK_LEFT);
                currentDirection = CurrentDirection.LEFT;
            }
            else if(moveInput.x > 0)
            {
                ChangeAnimationState(WALK_RIGHT);
                currentDirection = CurrentDirection.RIGHT;
            }
        }
        else
        {
            // Linearly interpolate between current velocity and zero
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);

            // Change animation state to idle
            if(!isAttacking)
            {
                if(currentDirection == CurrentDirection.UP)
                {
                    ChangeAnimationState(IDLE_UP);
                }
                else if(currentDirection == CurrentDirection.DOWN)
                {
                    ChangeAnimationState(IDLE_DOWN);
                }
                else if(currentDirection == CurrentDirection.RIGHT)
                {
                    ChangeAnimationState(IDLE_RIGHT);
                }
                else if(currentDirection == CurrentDirection.LEFT)
                {
                    ChangeAnimationState(IDLE_LEFT);
                }
            }
        }

        if(isAttackPressed)
        {
            isAttackPressed = false;

            if(!isAttacking)
            {
                isAttacking = true;
                if(currentDirection == CurrentDirection.UP)
                {
                    ChangeAnimationState(ATTACK_UP);
                }
                else if(currentDirection == CurrentDirection.DOWN)
                {
                    ChangeAnimationState(ATTACK_DOWN);
                }
                else if(currentDirection == CurrentDirection.LEFT)
                {
                    ChangeAnimationState(ATTACK_LEFT);
                }
                else if(currentDirection == CurrentDirection.RIGHT)
                {
                    ChangeAnimationState(ATTACK_RIGHT);
                }
            }
        }
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
