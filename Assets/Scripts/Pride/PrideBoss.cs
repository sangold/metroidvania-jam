using System;
using UnityEngine;

public class PrideBoss: MonoBehaviour
{
    public HealthComponent HealthComponent;
    private MeleeAttackComponent _meleeAttackComponent;
    [SerializeField]
    private CapsuleCollider2D _collider;
    [SerializeField]
    private Transform _attackPoint;
    public AnimatorEventHandler AnimatorEventHandler;
    public ParticleSystem _particleSystem;
    private Mirror.MirrorType _mt;

    private void Awake()
    {
        HealthComponent = GetComponent<HealthComponent>();
        _meleeAttackComponent = GetComponent<MeleeAttackComponent>();
        AnimatorEventHandler = GetComponentInChildren<AnimatorEventHandler>();
        _mt = Mirror.MirrorType.FRONT;
    }

    public void SetColliderActive(bool isActive)
    {
        _collider.enabled = isActive;
    }

    public void Attack()
    {
        _meleeAttackComponent.Attack();
        if(_mt == Mirror.MirrorType.FRONT)
            _particleSystem.Play();
    }

    private void OnDamageTaken(int hp, Vector3 attackOrigin)
    {
        
    }

    public void SetToType(Mirror.MirrorType type)
    {
        _mt = type;
        if (type == Mirror.MirrorType.SIDE)
        {
            _collider.offset = new Vector2(-.45f, -.81f);
            _collider.size = new Vector2(2.4f, 2.71f);
            _meleeAttackComponent.SetAttackPos(new Vector2(-1.4f, -1f), 1.24f);
        }
        else
        {
            _collider.offset = new Vector2(.12f, -.55f);
            _collider.size = new Vector2(1.69f, 3.37f);
            _meleeAttackComponent.SetAttackPos(new Vector2(.1f, -2f), 1.6f);

        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D target)
    {
        HealthComponent targetHealth = target.gameObject.GetComponent<HealthComponent>();
        if (target.gameObject.tag == "Player")
        {
            targetHealth.TakeDamage(1, transform.position);
        }
    }
}
