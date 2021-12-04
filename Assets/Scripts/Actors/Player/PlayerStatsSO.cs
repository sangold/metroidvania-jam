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

    public int CurrentHealth;
    public int MaxHealth;

    public int HealthCollectible;
    public int ManaCollectible;

    private void OnDisable()
    {
        // Reset for later

        //HasScythe = false;
        //HasDoubleJump = false;
        //HasWallJump = false;
        //HasGhostDash = false;
        //HasDash = false;
        //HasChargeAttack = false;

        CurrentHealth = 8;
        MaxHealth = 4;

        HealthCollectible = 0;
        ManaCollectible = 0;
    }
}
