using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float BaseDamage;
    public float Damage;
    public float Multiplier;
    public float range = 3;
    private AudioSource AS;
    private void OnEnable()
    {
        Destroy(this.gameObject, range);
        AS = GetComponent<AudioSource>();
        Damage = BaseDamage * Multiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.gameObject.activeSelf) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
           // Debug.Log("hit : " + other.gameObject.name);
            this.GetComponent<CircleCollider2D>().enabled = false;
            this.gameObject.SetActive(false);
            other.gameObject.GetComponent<_EnemyMainClass>().OnTakeDamage(Damage);
            //AS.Play();
        }
    }   

}