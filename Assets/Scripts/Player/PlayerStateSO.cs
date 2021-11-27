
using UnityEngine;

public enum PlayerState { STANDARD, INAIR, ATTACK, GHOSTDASH, HURT, DASH, WALL_SLIDE, WALLJUMPING, DEAD , SPIN_ATTACK }
[CreateAssetMenu(fileName = "PlayerState", menuName = "Assets/Player/State")]
public class PlayerStateSO : ScriptableObject
{

    public PlayerState StateType;
    public float GravityScale;
    public float HorizontalSpeed;
    public float JumpForce;
    public float VerticalSpeed;
    public bool CanMove;
    public bool CanDash;
    public bool CanGhost;
    public bool CanAttack;
    public bool CanJump;
    public bool CanChargeAttack;
    public bool HasStandardTransition;
}
