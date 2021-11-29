using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _currentHealth = 3;//Hit Points
    [SerializeField] private int _maxHealth = 3;//1 Containers  = 2 Hit points
    [SerializeField] private int _healthPiecesCollected = 0;
    // Might change into an Actioif we move rendering to it's own component
    [SerializeField] private SpriteRenderer _sr;
    private Color _originalColor;
    private float _flashDuration = .08f;
    private bool _isStunned;

    public delegate void OnDamageTakenEvent(int hp);
    public event OnDamageTakenEvent OnDamageTaken;

    public delegate void HealthIncreassedEvent(int maxHP);
    public event HealthIncreassedEvent OnHealthIncreassed;
    public float StunDuration;

    public int Health { get => _currentHealth; set => _currentHealth = Mathf.Clamp(value, 0, _maxHealth); }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value;}
    public int HealthPiecesCollected { get => _healthPiecesCollected; set
    {
        _healthPiecesCollected = value;
        while (_healthPiecesCollected >= 3) {
        HealthIncreassed();
        _healthPiecesCollected -= 3;
        }
    } 
    }
    private void Awake()
    {
        _originalColor = _sr.color;
        OnHealthIncreassed?.Invoke(_maxHealth);
    }
    public void HealthIncreassed(){
        _maxHealth++;
        OnHealthIncreassed?.Invoke(_maxHealth);
        _currentHealth = _maxHealth * 2;
    }

    public void TakeDamage(int amount)
    {
        if (_isStunned) return;
        _currentHealth -= amount;
        StopCoroutine(Blink(0f));
        StartCoroutine(Blink(StunDuration));
        if(_currentHealth <= 0)
        {
            Kill();
        }
        OnDamageTaken?.Invoke(_currentHealth);
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