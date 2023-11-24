using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections), typeof(Damageable))]
public class KnightScripts : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    public detectionzone attackZone;
    public detectionzone cliffDetectionZone;
    Animator anim;
    Damageable damageable;
    public enum WalkableDirection
    {
        Right, Left
    }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.left;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set { 
            if(_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }    
            }                
            _walkDirection = value; }
    }
    private bool _hasTarget = false;
    

    public bool HasTarget {
        get
        {
            return _hasTarget;        
        }
        private set 
        {
            _hasTarget= value;
            anim.SetBool(AnimationString.hasTarget, value);
        }
    }
    public bool CanMove
    {
        get
        {
            return anim.GetBool(AnimationString.canMove);
        }
    }

    public float attackCooldown 
    { 
        get
        {
            return anim.GetFloat(AnimationString.attackCooldown);
        }
        private set
        {
            anim.SetFloat(AnimationString.attackCooldown, Mathf.Max(value, 0));
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        anim = GetComponent<Animator>();
        damageable= GetComponent<Damageable>();
        
    }
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if(attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if(!damageable.LockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded )
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
        
        
 
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection= WalkableDirection.Left;
        }else if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection= WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Hihihaha");
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        damageable.LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        Debug.Log("x:" + knockback.x + "/// rbvelo: " + rb.velocity.y + "knockback.y: " + knockback.y);
    }
    public void OnCliffDetected()
    {
        if(touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
