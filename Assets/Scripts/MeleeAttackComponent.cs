using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackComponent : MonoBehaviour
{
    [SerializeField]
    private Transform _attackPoint;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private LayerMask _opponentLayer;
    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _opponentLayer);

        foreach(Collider2D enemy in enemies)
        {
            enemy.GetComponent<HealthComponent>()?.TakeDamage(1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
