using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController instance;

    [SerializeField] public Animator muzzleFlash;
    [HideInInspector] public float speed;
    [SerializeField] Transform GunArm;
    [SerializeField] GameObject BulletPrefab;


    private PlayerInputs _input;
    private Animator _animator;
    private Rigidbody2D _rb2d;
    private AudioSource _AS;

    int IdleH = Animator.StringToHash("IdleH");
    int IdleV = Animator.StringToHash("IdleV");
    private bool shooting;
    private bool running;

    #region Debuff
    [HideInInspector] public float stuckImmunity = 7;
    [HideInInspector] public float stuckDuration = 2.5f;
    [HideInInspector] public bool _CanBeStuckAgain = true;
    [HideInInspector] private bool isstuck;
    public bool isStuck 
    {
        get { return isstuck; }
        set
        {
            if (value)
            {
                Invoke(nameof(notStuck), stuckDuration);
                Invoke(nameof(CanBeStuckAgain), stuckImmunity);
            }
            isstuck = value;
        }
    }
    private void notStuck()
    {
        isStuck = false;
    }
    private void CanBeStuckAgain()
    {
        _CanBeStuckAgain = true;
    }
    #endregion
    #region Health
    [SerializeField] GameObject healthbar;
    private HeadQuarters HQ;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public float MaxHealth = 10;
    private float health = 10;
    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            isAlive = health > 0;
            var scale = healthbar.transform.localScale;
            scale.x = health / MaxHealth;
            healthbar.transform.localScale = scale;
            healthbar.transform.parent.gameObject.SetActive(health < MaxHealth && health > 0);
            //UIManager.Instance.HealthCount.text = health.ToString() + "/" + MaxHealth.ToString();
        }
    }

    #endregion
    #region Ammo
    [HideInInspector] public int MaxAmmo = 30;

    private int ammo = 30;
    public int Ammo
    {
        get { return ammo; }
        set
        {
            ammo = value;

            if (UIManager.Instance)
            {
                UIManager.Instance.AmmoCount.gameObject.SetActive(ammo <= MaxAmmo);
                UIManager.Instance.AmmoCount.text = ammo.ToString() + "/" + MaxAmmo.ToString();
            }

        }
    }
    #endregion


    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInputs>();
        _animator = GetComponent<Animator>();
        _AS = GetComponent<AudioSource>();

        
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Health = MaxHealth;
        Ammo = 99999;
    }

    private void OnEnable()
    {
        ResetGame();
    }
    public void ResetGame()
    {
        
        if (DatabaseManager.Instance != null)
        {
            LocalData data = DatabaseManager.Instance.GetLocalData();
            MaxHealth = 10 + data.upgraded_hp;
            MaxAmmo = 30 + data.upgraded_ammocount;
            BulletPrefab.GetComponent<Projectile>().Multiplier = data.upgraded_damage;
            if(HQ == null)
            {
                HQ = FindObjectOfType<HeadQuarters>();
            }
            HQ.ResetValues();
            Health = MaxHealth;
            Ammo = MaxAmmo;
        }
    }
   
    private void Update()
    {
        if (isAlive)
        {
            Move();
        }

        if (isAlive && !_animator.GetBool("isChilling"))
        {
            HandleFacing();
            HandleAiming();
        }


    }

    private void HandleFacing()
    {
        if (_input.GetPlayerMovement().x > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _animator.SetFloat(IdleH, 1);
            _animator.SetFloat(IdleV, 0);
        }
        else if (_input.GetPlayerMovement().x < 0)
        {
            transform.eulerAngles = Vector3.zero;
            _animator.SetFloat(IdleH, -1);
            _animator.SetFloat(IdleV, 0);
        }
        else if (_input.GetPlayerMovement().y > 0)
        {
            _animator.SetFloat(IdleH, 0);
            _animator.SetFloat(IdleV, 1);

        }
        else if (_input.GetPlayerMovement().y < 0)
        {
            _animator.SetFloat(IdleH, 0);
            _animator.SetFloat(IdleV, -1);
        }
    }

    public void OnTakeDamage(float damage = 1, float Multiplier = 1)
    {
        Health -= damage;
        //Debug.Log("hit");
        
        if (Health <= 0)
        {
            //Destroy(this.gameObject);
            _animator.SetTrigger("isDead");
        }
        else
        {
            _animator.SetTrigger("isHit");
        }

        _AS.Play();
    }

    private void HandleAiming()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = ((Vector2)GunArm.position - mousePos).normalized;

       
        shooting = !running;
       


        Vector3 localscale = Vector3.one;
        if (shooting)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            GunArm.eulerAngles = new Vector3(0, 0, angle);

            if (angle > 100 || angle < -100)
            {
                localscale.y = -1;
                //Debug.Log("Right");
                _animator.SetFloat("AimH", 1);
                _animator.SetFloat("AimV", 0);
            }
            else if (angle > -80 && angle < 80)
            {
                localscale.y = 1;
                //Debug.Log("Left");
                _animator.SetFloat("AimH", -1);
                _animator.SetFloat("AimV", 0);
            }
            else if (angle < -80 && angle > -100)
            {
               // Debug.Log("Up");
                _animator.SetFloat("AimH", 0);
                _animator.SetFloat("AimV", 1);
            }
            else if (angle > 80 && angle < 100)
            {
             //   Debug.Log("Down");
                _animator.SetFloat("AimH", 0);
                _animator.SetFloat("AimV", -1);
            }

            if(_input.IsShooting() && !running && Ammo > 0)
            {
                if (ifUIItemIsHit()) return;
                muzzleFlash.SetTrigger("fire");
                //Debug.Log(aimDirection);
                GameObject bullet = Instantiate(BulletPrefab, (Vector2)muzzleFlash.transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = -aimDirection * 10;
                Ammo--;
                if (Ammo == 0)
                {
                    MessaeBox.insta.ShowInformationMsg(Color.white, "Collect Box To Reload");
                }
            }
            GunArm.localScale = localscale;
        }

        if (!running)
        {
            var Direction = Vector3.Cross((Vector3)(mousePos) - transform.position, transform.up);
            if (Direction.z > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                
                _animator.SetFloat(IdleH, 1);
                _animator.SetFloat(IdleV, 0);
            }
            else if (Direction.z < 0)
            {
                transform.eulerAngles = Vector3.zero;
                _animator.SetFloat(IdleH, -1);
                _animator.SetFloat(IdleV, 0);
            }
        }


        healthbar.transform.parent.eulerAngles = Vector3.zero;
        GunArm.gameObject.SetActive(shooting && !running);
        if (!GunArm.gameObject.activeSelf)
        {
            muzzleFlash.GetComponent<SpriteRenderer>().sprite = null;
        }
        _animator.SetBool("isShooting", shooting && !running);
    }

    private bool ifUIItemIsHit()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.layer == 5)
            {
                return true;
            }
        }
        return false;
    }

    private void FixedUpdate2()
    {
        if (isAlive)
        {
            Move();
        }
    }

    private void Move()
    {

        if (!isStuck)
        {
            _rb2d.MovePosition(_rb2d.position + _input.GetPlayerMovement() * Time.fixedDeltaTime * speed);
        }
        if (_input.GetPlayerMovement().magnitude > Mathf.Epsilon)
        {
            if(_animator.GetBool("isChilling")) _animator.SetBool("isChilling", false);
            _animator.SetBool("isRunning", true);
            running = true;
        }
        else
        {
            _animator.SetBool("isRunning", false);
            running = false;
        }
        _animator.SetFloat("MoveH", _input.GetPlayerMovement().x);
        _animator.SetFloat("MoveV", _input.GetPlayerMovement().y);
    }

    public void DisableThis()
    {
        EnemySpawner.Instance.enabled = false;
        gameObject.SetActive(false);
        UIManager.Instance.OpenGameOverPanel();
    }
}
