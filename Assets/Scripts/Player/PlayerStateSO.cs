
using UnityEngine;

public enum PlayerState { STANDARD, INAIR, ATTACK, GHOSTDASH, HURT, SLIDE, WALLING, WALLJUMPING, DEAD }
[CreateAssetMenu(fileName = "PlayerState", menuName = "Assets/Player/State")]
public class PlayerStateSO : ScriptableObject
{

    public PlayerState StateType;
    public float GravityScale;
    public float HorizontalSpeed;
    public float JumpForce;
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
