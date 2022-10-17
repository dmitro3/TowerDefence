using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class _EnemyMainClass : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyType EnemyType;
    [SerializeField] float AttackRate;
    [SerializeField] public Transform Target;
    [SerializeField] int CoinValue;
    


    private bool isAlive = true;
    private float nextShootTime;
    private EnemyState state;
    private IAttack AttackScirpt;
    private Animator animator;
    private AIDestinationSetter Setter;
    private AIPath Path;
    private AudioSource AS;


    #region health
    [SerializeField] float maxHealth = 4;
    [SerializeField] float health = 4;
    [SerializeField] GameObject healthbar;
    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            var scale = healthbar.transform.localScale;
            scale.x = health / maxHealth;
            healthbar.transform.localScale = scale;
        }
    }
    #endregion

    private void OnEnable()
    {
        ResetStats();
    }
    private void OnDisable()
    {
        isAlive = false;
    }

    public void ResetStats()
    {
        isAlive = true;
        Setter.target = Target;
        Health = maxHealth;
        healthbar.transform.parent.gameObject.SetActive(true);
        nextShootTime = AttackRate;
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var item in colliders)
        {
            item.enabled = true;
        }
    }

    private void Awake()
    {
        Setter = GetComponent<AIDestinationSetter>();
        Path = GetComponent<AIPath>();
        AttackScirpt = GetComponent<IAttack>();
        animator = GetComponent<Animator>();
        AS = GetComponent<AudioSource>();
        if (Target == null)
        {
            if (EnemyType != EnemyType.boss)
            {
                Target = PlayerController.instance.transform;
            }
            else
            {
                Target = GameObject.Find("PlaceHolder Office").transform;
            }
        }
    }
    internal void Spawn()
    {
        gameObject.SetActive(true);
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    private void Update()
    {
        if (Path == null || !PlayerController.instance.isAlive || !isAlive) return;
        StateBehaviour(); 
    }

    private void StateBehaviour()
    {
        switch(state)
        {
            case EnemyState.attacking:
                {
                    if (Time.time > nextShootTime)
                    {
                        state = EnemyState.isShooting;
                        Path.canMove = false;
                        TargetInitPos = Target.transform.position;
                        animator.SetTrigger("isAttacking");
                        nextShootTime = Time.time + AttackRate;
                    }
                    else
                    {
                        ChangeState();
                    }
                }
                break;
            case EnemyState.chasing:
                {
                    Path.canMove = true;
                    if (Path.reachedEndOfPath)
                    {
                        state = EnemyState.attacking;
                    }
                }
                break;
            case EnemyState.isShooting:
                break;
        }
    }
    Vector2 TargetInitPos;
    public void AttackMethod()
    {
        AttackScirpt.Attack(TargetInitPos);
    }

    public void ChangeState()
    {
        state = EnemyState.chasing;
    }
    public void OnTakeDamage(float damageAmount = 1, float DamageMultiplier = 1)
    {
        Health -= damageAmount * DamageMultiplier;
        if (Health <= 0)
        {
            AS.Play();
            isAlive = false;
            Path.canMove = false;
            healthbar.transform.parent.gameObject.SetActive(false);
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (var item in colliders)
            {
                item.enabled = false;
            }
            GameManager.Instance.EnemiesKilled++;
            GameManager.Instance.CoinsEarned+= CoinValue;
            animator.SetTrigger("isDead");
        }
    }

    public void DisableThis()
    {
        if (EnemyType == EnemyType.green)
            EnemySpawner.Instance.GreenPool.Despawn(this.gameObject);
        else if (EnemyType == EnemyType.flying)
            EnemySpawner.Instance.FlyingPool.Despawn(this.gameObject);
        else if (EnemyType == EnemyType.boss)
            EnemySpawner.Instance.BossPool.Despawn(this.gameObject);

    }
}


public enum EnemyType
{
    green,
    flying,
    boss
}
public enum EnemyState
{
    chasing,
    attacking,
    isShooting
}
