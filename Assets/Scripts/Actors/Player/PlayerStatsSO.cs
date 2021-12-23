using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Assets/Player/Stats")]
public class PlayerStatsSO: ScriptableObject
{
    public bool HasScythe;
    public bool HasDoubleJump;
    public bool HasWallJump;
    public bool HasGhostDash;
    public bool HasDash;
    public bool HasChargeAttack;
    public bool HasMirror;

    public int CurrentHealth;
    public int MaxHealth;

    public int HealthCollectible;
    public int ManaCollectible;

    private void OnEnable()
    {
        HasScythe = true;
    }
    private void OnDisable()
    {
        // Reset for later

        HasScythe = false;
        HasDoubleJump = false;
        HasWallJump = false;
        HasGhostDash = false;
        HasDash = false;
        HasChargeAttack = false;
        HasMirror = false;

        CurrentHealth = 8;
        MaxHealth = 4;

        HealthCollectible = 0;
        ManaCollectible = 0;
    }

    public void LoadData(int currentHealth, int maxHealth, bool hasScythe, bool hasMirror, bool hasDoubleJump, bool hasWallJump, bool hasGhostDash, bool hasDash, bool hasChargeAttack)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;

        HasScythe = hasScythe;
        HasMirror = hasMirror;
        HasDoubleJump = hasDoubleJump;
        HasWallJump = hasWallJump;
        HasGhostDash = hasGhostDash;
        HasDash = hasDash;
        HasChargeAttack = hasChargeAttack;
    }

    public void UnlockAbility(PowerUpType type)
    {
        switch(type)
        {
            case PowerUpType.MIRROR:
                HasMirror = true;
                break;
            case PowerUpType.SCYTHE:
                HasScythe = true;
                break;
            case PowerUpType.DOUBLEJUMP:
                HasDoubleJump = true;
                break;
            case PowerUpType.WALLJUMP:
                HasWallJump = true;
                break;
            case PowerUpType.GHOSTDASH:
                HasGhostDash = true;
                break;
            case PowerUpType.DASH:
                HasDash = true;
                break;
            case PowerUpType.CHARGEATTACK:
                HasChargeAttack = true;
                break;
        }
    }

    public bool CheckAbility(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.MIRROR:
                return HasMirror;
            case PowerUpType.SCYTHE:
                return HasScythe;
            case PowerUpType.DOUBLEJUMP:
                return HasDoubleJump;
            case PowerUpType.WALLJUMP:
                return HasWallJump;
            case PowerUpType.GHOSTDASH:
                return HasGhostDash;
            case PowerUpType.DASH:
                return HasDash;
            case PowerUpType.CHARGEATTACK:
                return HasChargeAttack;
            default:
                return false;
        }
    }
}
