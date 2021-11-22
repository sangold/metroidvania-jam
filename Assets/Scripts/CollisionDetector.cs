using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [HideInInspector]
    public bool OnGround, OnWall, OnRightWall, OnLeftWall;
    [HideInInspector]
    public int WallSide;
    
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _collisionRadius = .25f;
    [SerializeField]
    private Vector2 _bottomOffset, _rightOffset, _leftOffset;

    public bool IsPushingAgainstAWall(float dir)
    {
        return (dir < 0 && OnLeftWall) || (dir > 0 && OnRightWall);
    }

    private void FixedUpdate()
    {
        OnGround = Physics2D.OverlapCircle((Vector2)transform.position + _bottomOffset, _collisionRadius, _groundLayer);
        OnRightWall = Physics2D.OverlapCircle((Vector2)transform.position + _rightOffset, _collisionRadius, _groundLayer);
        OnLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + _leftOffset, _collisionRadius, _groundLayer);

        OnWall = OnRightWall || OnLeftWall;

        WallSide = OnRightWall ? -1 : 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + _bottomOffset, _collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + _rightOffset, _collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + _leftOffset, _collisionRadius);
    }
}
