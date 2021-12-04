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
            Actor actor = enemy.GetComponent<Actor>();
            HealthComponent hc = enemy.GetComponent<HealthComponent>();
            if (hc == null) continue;
            if (hc.IsShielded)
            {
                Vector3 dir = actor.transform.position - transform.position;
                if(dir.x > 0 && actor.IsTurnToTheLeft())
                    continue;
                if (dir.x < 0 && !actor.IsTurnToTheLeft())
                    continue;
            }
            enemy.GetComponent<HealthComponent>()?.TakeDamage(1, transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
