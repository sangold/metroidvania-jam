using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    // Might change into an Actioif we move rendering to it's own component
    [SerializeField] private SpriteRenderer _sr;
    private Color _originalColor;
    private float _flashDuration = .08f;

    private void Awake()
    {
        _originalColor = _sr.color;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        StopCoroutine(Blink(0f));
        StartCoroutine(Blink(.75f));

        if(_health <= 0)
        {
            Kill();
        }
    }

    private IEnumerator Blink(float duration)
    {
        float timer = 0;
        while(timer < duration)
        {
            _sr.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, .3f);
            yield return new WaitForSeconds(_flashDuration);
            _sr.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1f);
            yield return new WaitForSeconds(_flashDuration);
            timer += _flashDuration * 2;
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
