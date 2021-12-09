using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI: MonoBehaviour
{
    [SerializeField]
    private Slider _bossHealthSlider;
    [SerializeField]
    private HealthComponent _trackedHealth;

    private void Start()
    {
        Init(_trackedHealth.Health, _trackedHealth.MaxHealth);
        _trackedHealth.OnDamageTaken += OnDamageTaken;
    }

    private void OnDestroy()
    {
        _trackedHealth.OnDamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(int hp, Vector3 attackOrigin)
    {
        _bossHealthSlider.value = hp;
    }

    public void Init(int current, int max)
    {
        _bossHealthSlider.maxValue = max;
        _bossHealthSlider.minValue = 0;
        _bossHealthSlider.value = current;
    }
}

