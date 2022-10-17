using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleEnemyAttack : MonoBehaviour, IAttack
{
    [SerializeField] float Damage;
    [SerializeField] float LungeDistance;
    [SerializeField] Collider2D myCollider;
    [SerializeField] AudioClip FlyBy;
    [SerializeField] AudioClip OnHitClip;
    private AudioSource AS;
    private bool canDamage;

    private void Awake()
    {
        AS = GetComponent<AudioSource>();
    }

    public void Attack(Vector2 targetInitPos)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 delta = (targetInitPos - (Vector2)transform.position).normalized;
        LeanTween.value(this.gameObject, rb.position, rb.position + delta * LungeDistance, 1f).setEaseOutExpo()
        .setOnStart(() =>
        {
            canDamage = true;
            //myCollider.enabled = true;
        }).setOnUpdate((Vector2 value) =>
        {
            rb.MovePosition(value);
        })
        .setOnComplete(() =>
        {
            canDamage = false;
            AS.Stop();
            AS.clip = OnHitClip;
            //myCollider.enabled = false;
        });
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && canDamage)
        {
            canDamage = false;
            PlayerController.instance.OnTakeDamage();
            AS.clip = FlyBy;
            AS.Play();
        }
    }
}
