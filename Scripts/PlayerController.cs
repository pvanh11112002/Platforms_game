using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    TouchingDirections touchingDirections;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    public float airWalkSpeed = 3f;
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving 
    { 
        get 
        {
            return _isMoving;        
        }
        private set
        {
            _isMoving= value;
            animator.SetBool(AnimationString.isMoving, value);
        }
    }
    //Bool IsMoving và AnimationString.IsMoving là 2 biến khác nhau
    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;

        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationString.isRunning, value);
        }
    }
    public float CurrentMoveSpeed { 
        get
        {
            if(IsMoving /*&& !touchingDirections.IsOnWall*/)
            {
                if (touchingDirections.IsGrounded)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    return airWalkSpeed;
                }    
            }
            else { return 0; }
        } 
    }
    public bool _isFacingRight = true;   
    public bool IsFacingRight {
        get 
        {
            return _isFacingRight;    
        }
        private set 
        {
            if(_isFacingRight != value) 
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight= value;
        } 
    }
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, moveInput.y);
        animator.SetFloat(AnimationString.yVelocity, rb.velocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);

    }    
    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        } 
    }    
    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning= true;
        }else if(context.canceled)
        {
            IsRunning = false;
        }    
    }   
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationString.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);           
            Debug.Log(transform.position);
        }
    }
}
