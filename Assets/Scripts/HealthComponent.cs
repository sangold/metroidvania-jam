using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    // Might change into an Actioif we move rendering to it's own component
    [SerializeField] private SpriteRenderer _sr;
    private Color _originalColor;
    private float _flashDuration = .08f;
    private bool _isStunned;

    public event Action<int> OnDamageTaken;
    public float StunDuration;

    public int Health { get => _currentHealth; set => _currentHealth = Mathf.Clamp(value, 0, _maxHealth); }

    private void Awake()
    {
        _originalColor = _sr.color;
        _currentHealth = _maxHealth;
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
            return;
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
