using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] private HealthComponent _playerHealthComponent;
    [SerializeField] private Sprite _emptyContainer;
    [SerializeField] private Sprite _halfContainer;
    [SerializeField] private Sprite _fullContainer;

    [SerializeField] private List<GameObject> _healthContainers;
    [SerializeField] private int _maxHealthContainers{get => _playerHealthComponent.MaxHealth;}
    [SerializeField] private int _health{get => _playerHealthComponent.Health;}
    // Start is called before the first frame update
    void Start()
    {
        GetTargetHealth();
        UpdateHealthContainers();
    }
    void OnDamageTaken(int currentHealth, Vector3 attackOrigin){
        UpdateHealthContainers();
    }
    void OnHealthIncreassed(int currentMaxHealth){
        SetMaxHealthContainers();
        UpdateHealthContainers();
    }
    void GetTargetHealth(){
        _playerHealthComponent = GameObject.Find("Player").GetComponent<HealthComponent>();
        if (_playerHealthComponent != null){
            _playerHealthComponent.OnDamageTaken += OnDamageTaken;
            _playerHealthComponent.OnHealthIncreased += OnHealthIncreassed;
        }
    }
    void SetMaxHealthContainers(){
        for (int i = 0; i < _healthContainers.Count; i++)
        {
            GameObject healthContainer = _healthContainers[i];
            if (i < _maxHealthContainers){
                healthContainer.SetActive(true);
            } else {
                healthContainer.SetActive(false);
            }
        }
    }
    void UpdateHealthContainers(){
        int maxHealthPerContainers = 2;
        int currentHealth = 0;
        GameObject healthContainer;
        for (int i = 0; i < _healthContainers.Count; i++)
        {
            healthContainer = _healthContainers[i];
            SetHealthContainer(healthContainer,0);
        }
        if (_health == 0) return;
        while (currentHealth != _health){
            healthContainer = _healthContainers[currentHealth / maxHealthPerContainers];
            currentHealth += 1;
            if (GetHealthContainer(healthContainer) == 0){
                SetHealthContainer(healthContainer,1);
                continue;
            }
            if (GetHealthContainer(healthContainer) == 1){
                SetHealthContainer(healthContainer,2);
                continue;
            }
        }
    }
    void SetHealthContainer(GameObject healthContainer, int amount){
        SpriteRenderer spriteRenderer = healthContainer.GetComponent<SpriteRenderer>();
        if (amount >= 2){
            spriteRenderer.sprite = _fullContainer;
        }
        if (amount == 1){
            spriteRenderer.sprite = _halfContainer;
        }
        if (amount == 0){
            spriteRenderer.sprite = _emptyContainer;
        }
    }
    int GetHealthContainer(GameObject healthContainer){
        SpriteRenderer spriteRenderer = healthContainer.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == _fullContainer){
            return 2;
        }
        if (spriteRenderer.sprite == _halfContainer){
            return 1;
        }
        if (spriteRenderer.sprite == _emptyContainer){
            return 0;
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        SetMaxHealthContainers();
        UpdateHealthContainers();
    }
}
