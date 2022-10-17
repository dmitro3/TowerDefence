using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossEnemyAttack : MonoBehaviour, IAttack
{
    [SerializeField] float Damage;
    private AIDestinationSetter Targetsetter;

    private void Awake()
    {
        Targetsetter = GetComponent<AIDestinationSetter>();
    }
    public void Attack(Vector2 target)
    {
        Vector2 attackpoint = (target - (Vector2)transform.position).normalized * 2 + (Vector2)transform.position;
        Collider2D[] hitBase = Physics2D.OverlapCircleAll(attackpoint, 2);
        List<Transform> hitObjects = new List<Transform>();
        foreach (var item in hitBase)
        {
            hitObjects.Add(item.transform);
        }
        if (hitObjects.Contains(Targetsetter.target))
        {
            Debug.Log("should slap here");
            var HeadQuarters = Targetsetter.target.GetComponent<IDamageable>();
            HeadQuarters.OnTakeDamage(Damage, 1);
            return;
        }

       /* if (DelayedDamageCO == null)
        {
            var TargetCollider = Targetsetter.target.GetComponent<Collider2D>();
            DelayedDamageCO = StartCoroutine(Damagewall(TargetCollider));
        }*/
    }
    Coroutine DelayedDamageCO;
   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Walls")) return;
       
        if (path.reachedEndOfPath && !path.reachedDestination && !Targetsetter.target.CompareTag("Walls"))
        {
            TempTarget = Targetsetter.target;
            Targetsetter.target = collision.transform;
        }

        if (!colliders.Contains(collision)) colliders.Add(collision);
        if(DelayedDamageCO == null)
            DelayedDamageCO = StartCoroutine(Damagewall(collision));
    }

    private IEnumerator Damagewall(Collider2D collider)
    {

        List<Collider2D> newList = new List<Collider2D>();
        yield return new WaitForSeconds(DamageFrequency);
        if (DelayedDamageCO != null)
        {
            StopCoroutine(DelayedDamageCO);
            DelayedDamageCO = null;
        }

        if (colliders.Contains(collider))
        {
            foreach (var item in colliders)
            {
                newList.Add(item);
            }
            foreach (var item in newList)
            {
                //Debug.Log("damaging" + item.name);
                item.GetComponent<IDamageable>().OnTakeDamage(Damage, 1);
            }
            DelayedDamageCO = StartCoroutine(Damagewall(collider));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(colliders.Contains(collision))
        {
            colliders.Remove(collision);
            if (DelayedDamageCO != null)
            {
                StopCoroutine(DelayedDamageCO);
                DelayedDamageCO = null;
            }
        }

        if(Targetsetter.target.CompareTag("Walls") && Targetsetter.target.gameObject.activeSelf)
        {
            Targetsetter.target = TempTarget;
        }
    }
*/

}
