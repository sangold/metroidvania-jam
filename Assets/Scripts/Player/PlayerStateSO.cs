
using UnityEngine;

public enum PlayerState { STANDARD, INAIR, ATTACK, GHOSTDASH, HURT, SLIDE, WALLING, WALLJUMPING, DEAD }
[CreateAssetMenu(fileName = "PlayerState", menuName = "Assets/Player/State")]
public class PlayerStateSO : ScriptableObject
{

    public PlayerState StateType;
    public float gravityScale;
    public float HSpeed;
    public float VSpeed;
    public float MaxHSpeed;
    public float MaxVSpeed;
    public float Friction;
    public bool CanMove;
    public bool CanDash;
    public bool CanGhost;
    public bool CanAttack;
    public bool CanJump;
    public bool HasStandardTransition;
}
