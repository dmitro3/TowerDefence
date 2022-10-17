using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadQuarters : MonoBehaviour, IDamageable
{

    [SerializeField] Slider healthbar;
    public float MaxHealth = 10;
    private float health = 10;

    private void Start()
    {
        Health = MaxHealth;
    }
    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            healthbar.value = health / MaxHealth * healthbar.maxValue;
        }
    }

    public void OnTakeDamage(float Damage, float Multiplier = 1)
    {
        Health -= Damage * Multiplier;

        if (Health <= 0)
        {
            PlayerController.instance.OnTakeDamage(10000);
        }
    }

    public void ResetValues()
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();
        MaxHealth = 10 + data.upgraded_hp;

        Health = MaxHealth;
    }
}
