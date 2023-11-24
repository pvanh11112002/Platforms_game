using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    
    public detectionzone biteDetectionzone;
    Animator anim;
    Rigidbody2D rb;
    public bool _hasTarget = false;
    Damageable damageable;
    public List<Transform> waypoints;
    public float flightSpeed = 2f;
    int waypointNum = 0;
    Transform nextWaypoint;
    public float waypointReachDistance = 0.1f;
    public Collider2D deathCollider;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
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
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable= GetComponent<Damageable>();
        deathCollider = GetComponent<Collider2D>();
    }
    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }
    void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionzone.detectedColliders.Count > 0;
    }
    private void FixedUpdate()
    {
        if(damageable.IsAlive)
        {
            if(CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }        
    }

    private void Flight()
    {
        //lay huong' di chuyen
        Vector2 directionToWaypoints = (nextWaypoint.position - transform.position).normalized;
        //check xem obj da den duoc voi waypoint hay chua
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
        rb.velocity += directionToWaypoints * flightSpeed;
        UpdateDirection();
        if(distance <= waypointReachDistance)
        {
            waypointNum++;
            if(waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }
            nextWaypoint= waypoints[waypointNum];
        }
        
    }
    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }
    public void OnDeath()
    {   
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
