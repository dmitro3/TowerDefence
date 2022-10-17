using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    private Vector3 oldPos;
    private Vector3 newPos;
    private bool isAttacking;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        newPos = transform.position;

        Vector2 delta = newPos - oldPos;
        if (!isAttacking)
        {
            //Debug.Log(delta.magnitude);
            if (delta.magnitude < 0.015f) return;
            if (delta.x > 0 && (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)))
            {
                _animator.SetFloat("ChaseH", 1);
                _animator.SetFloat("ChaseV", 0);
            }
            if (delta.x < 0 && (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)))
            {
                _animator.SetFloat("ChaseH", -1);
                _animator.SetFloat("ChaseV", 0);
            }
            if (delta.y > 0 && (Mathf.Abs(delta.x) < Mathf.Abs(delta.y)))
            {
                _animator.SetFloat("ChaseH", 0);
                _animator.SetFloat("ChaseV", 1);
            }
            if (delta.y < 0 && (Mathf.Abs(delta.x) < Mathf.Abs(delta.y)))
            {
                _animator.SetFloat("ChaseH", 0);
                _animator.SetFloat("ChaseV", -1);
            }
        }
    }

    private void FixedUpdate()
    {
        oldPos = transform.position;
    }
}
