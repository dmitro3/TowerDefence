using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemyAttack : MonoBehaviour, IAttack
{
    [SerializeField] GameObject StickyGlue;
    [SerializeField] float Damage;
    [SerializeField] bool canDoSpecial;
    private Vector3 GluePos;
    private void OnEnable()
    {
        canDoSpecial = true;
    }
    public void Attack(Vector2 TargetInitPos)
    {
        GluePos = TargetInitPos;
        if (PlayerController.instance._CanBeStuckAgain && canDoSpecial)
        {
            GameObject glue = Instantiate(StickyGlue, GluePos, Quaternion.identity);
            canDoSpecial = false;
        }
        else
        {
            

            Vector2 attackpoint = (GluePos - transform.position).normalized * 2 + transform.position;
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackpoint, 2);
            //List<Transform> hitObjects = new List<Transform>();
            foreach (var item in hitPlayer)
            {
                if(item.transform == PlayerController.instance.transform)
                {
                    PlayerController.instance.OnTakeDamage(Damage);
                    return;
                }
                //hitObjects.Add(item.transform);
            }
            /*if(hitObjects.Contains(PlayerController.instance.transform))
            {
                PlayerController.instance.OnTakeDamage(Damage);
                return;
            }*/
        }
    }

    public void OnDrawGizmos()
    {
        Vector2 attackpoint = (GluePos - transform.position).normalized * 2 + transform.position;
        Gizmos.DrawWireSphere(attackpoint, 2);
    }
}
