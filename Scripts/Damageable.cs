using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;
    Animator animator;
    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    { 
        get 
        {
            return _maxHealth;        
        }
        private set 
        {
            _maxHealth = value;        
        } 
    }
    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);
            if(Health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    public bool _isAlive = true;
    
    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    [SerializeField]
    private float invincibilityTimer = 1f;

    public bool IsAlive
    {
        get 
        {
            return _isAlive;
        }
        private set 
        {
            _isAlive = value;
            animator.SetBool(AnimationString.isAlive, value); 
            if(value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationString.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationString.lockVelocity, value);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        animator= GetComponent<Animator>();

    }
    private void Update()
    {
        if(isInvincible) 
        {
            if(timeSinceHit > invincibilityTimer)
            {
                isInvincible= false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
        //if(Health <= 0)
        //{
        //    Destroy(gameObject);
        //}
        //Hit(10);
    }
    public bool Hit(int damage, Vector2 knockback)
    {
        if(IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible= true;
            animator.SetTrigger(AnimationString.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            //Invoke để gọi event thực hiện, ở đây là characterDamaged
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            return true;
            
        }
        return false;
    }    
    public bool Heal(int healthRestore)
    {
        if(IsAlive && Health < MaxHealth)
        {
            //tìm ra giá trị máu có thể hồi tối đa, máu hiện tại đang là 100 thì số máu có thể hồi tối đa = 0, máu = 50 thì có thể hồi max 50 máu
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            //tìm ra giá trị máu có thể hồi thực tế, vì vật phẩm chỉ có thể hồi tối đa theo chỉ số healthRestore, nên nếu máu có thể hồi > giá trị hồi máu của vật phẩm thì lấy giá trị min là số máu có thể hồi của vật phẩm
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
