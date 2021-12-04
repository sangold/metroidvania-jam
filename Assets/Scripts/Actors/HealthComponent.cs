using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _currentHealth = 8;//Hit Points
    [SerializeField] private int _maxHealth = 4;//1 Containers  = 2 Hit points
    [SerializeField] private int _healthPiecesCollected = 0;
    // Might change into an Actioif we move rendering to it's own component
    [SerializeField] private SpriteRenderer _sr;
    private Color _originalColor;
    private float _flashDuration = .08f;
    private bool _isStunned;

    public delegate void OnDamageTakenEvent(int hp, Vector3 attackOrigin);
    public event OnDamageTakenEvent OnDamageTaken;

    public delegate void HealthIncreasedEvent(int maxHP);
    public event HealthIncreasedEvent OnHealthIncreased;

    public delegate void HealthPieceCollectedEvent(int newCount);
    public event HealthPieceCollectedEvent OnHealthPieceCollected;

    public float StunDuration;
    public bool IsShielded;

    public int Health { get => _currentHealth; set => _currentHealth = Mathf.Clamp(value, 0, _maxHealth * 2); }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value;}
    public int HealthPiecesCollected
    { 
        get => _healthPiecesCollected;
        set
        {
            _healthPiecesCollected = value;
            while (_healthPiecesCollected >= 3)
            {
                HealthIncreased();
                _healthPiecesCollected -= 3;
            }
            OnHealthPieceCollected?.Invoke(_healthPiecesCollected);
        } 
    }
    private void Awake()
    {
        _originalColor = _sr.color;
        OnHealthIncreased?.Invoke(_maxHealth);
    }
    public void HealthIncreased(){
        _maxHealth++;
        OnHealthIncreased?.Invoke(_maxHealth);
        Health = _maxHealth * 2;
    }

    public void TakeDamage(int amount, Vector3 attackOrigin)
    {
        if (_isStunned) return;
        _currentHealth -= amount;
        StopCoroutine(Blink(0f));
        StartCoroutine(Blink(StunDuration));
        if(_currentHealth <= 0)
        {
            Kill();
        }
        OnDamageTaken?.Invoke(_currentHealth, attackOrigin);
    }

    private IEnumerator Blink(float duration)
    {
        _isStunned = true;
        float timer = 0;
        while(timer < duration)
        {
            _sr.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, .3f);
            yield return new WaitForSeconds(_flashDuration);
            _sr.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1f);
            yield return new WaitForSeconds(_flashDuration);
            timer += _flashDuration * 2;
        }
        _isStunned = false;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}