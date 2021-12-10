using System;
using UnityEngine;

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

        CurrentHealth = 8;
        MaxHealth = 4;

        HealthCollectible = 0;
        ManaCollectible = 0;
    }

    public void UnlockAbility(string abilityName)
    {
        switch(abilityName)
        {
            case "Mirror":
                HasMirror = true;
                Debug.Log("CAN TAKE MIRROR NOW");
                break;
            case "Scythe":
                HasScythe = true;
                break;
            case "DoubleJump":
                HasDoubleJump = true;
                break;
            case "WallJump":
                HasWallJump = true;
                break;
            case "GhostDash":
                HasGhostDash = true;
                break;
            case "Dash":
                HasDash = true;
                break;
            case "ChargeAttack":
                HasChargeAttack = true;
                break;
        }
    }
}
