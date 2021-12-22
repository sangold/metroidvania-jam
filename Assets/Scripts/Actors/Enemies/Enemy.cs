using Reapling.SaveLoad;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public abstract class Enemy : Actor, ISaveable
{

    [System.Serializable]
    public struct EnemyData
    {
        public bool isKilled;
        public Vector3 position;
    }

    public Animator Animator;
    public Rigidbody2D Rb;
    [HideInInspector]
    public float lastAttackTimer;
    [HideInInspector]
    public Coroutine AttackCoroutine;
    [HideInInspector]
    public bool CanMove = true;

    protected HealthComponent _healthComponent;

    [SerializeField] protected Transform[] _patrolGizmos;
    protected Vector3[] _patrolPoints;
    

    protected float _moveSpeed, _jumpForce;
    protected AIStateMachine _stateMachine;
    protected IState _previousState;
    protected UniqueID UUID;

    public object CaptureState()
    {
        return new EnemyData
        {
            position = transform.position,
            isKilled = _healthComponent.Health <= 0
        };
    }

    public void RestoreState(object state)
    {
        EnemyData saveData = (EnemyData)state;

        if (saveData.isKilled)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = saveData.position;
    }

    public EnemyData Save()
    {
        var saveData = new EnemyData();
        saveData.position = transform.position;
        saveData.isKilled = _healthComponent.Health <= 0;

        return saveData;
    }

    public void Load(EnemyData data)
    {
        if (data.isKilled)
            gameObject.SetActive(false);
    }
    public void ReturnToPreviousState()
    {
        if(_previousState != null)
        {
            _stateMachine.SetState(_previousState);
            _previousState = null;
        }
    }

    protected virtual void Awake()
    {
        UUID = new UniqueID();
        _healthComponent = GetComponent<HealthComponent>();
        Rb = GetComponent<Rigidbody2D>();
        _patrolPoints = new Vector3[_patrolGizmos.Length];
        for (int i = 0; i < _patrolGizmos.Length; i++)
        {
            _patrolPoints[i] = _patrolGizmos[i].position;
            _patrolGizmos[i].gameObject.SetActive(false);
        }

        _stateMachine = new AIStateMachine();
        UUID.Init(gameObject);
    }

    protected virtual void Start()
    {

        _healthComponent.OnDamageTaken += OnDamageTaken;
        CanMove = true;
    }

    private void OnDestroy()
    {
        _healthComponent.OnDamageTaken -= OnDamageTaken;
        if (AttackCoroutine != null)
            StopCoroutine(AttackCoroutine);
    }

    protected virtual void OnDamageTaken(int hp, Vector3 attackOrigin)
    {
        if (hp <= 0)
            gameObject.SetActive(false);
    }

    protected virtual void FixedUpdate()
    {
        if (Rb.velocity.x < 0 && CanMove)
        {
            TurnLeft();
        }
        else if (Rb.velocity.x > 0 && CanMove)
        {
            TurnRight();
        }
        _stateMachine.Tick();
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
