using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDemoScript : MonoBehaviour
{

    public static GunDemoScript Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            transform.parent.gameObject.SetActive(false);
        }
    }
    public Transform Gun;
    public Transform MuzzleFlash;
    public SpriteRenderer GunSprite;
    public Sprite _BulletSprite;
    public List<GunType> _GunType = new List<GunType>();

    public Projectile bullet;
    [System.Serializable]
    public class GunType
    {
        public Transform _Slot;
        public Vector3 _MuzzlePos;
        public Sprite _Sprite;
        public float _BaseDamage;
        public float _Range;
        public float Speed;
    }

    private void Start()
    {
        SelectGun(0);
    }
    public void SelectGun(int ID)
    {

        Gun.parent = _GunType[ID]._Slot;
        Gun.localPosition = Vector3.zero;
        Gun.localEulerAngles = Vector3.zero;
        MuzzleFlash.parent = Gun;
        MuzzleFlash.localPosition = _GunType[ID]._MuzzlePos;
        MuzzleFlash.localEulerAngles = _GunType[ID]._Slot.localEulerAngles;
        GunSprite.sprite = _GunType[ID]._Sprite;
        bullet.BaseDamage = _GunType[ID]._BaseDamage;
        bullet.range = _GunType[ID]._Range;
        PlayerController.instance.speed = _GunType[ID].Speed;
    }

}
