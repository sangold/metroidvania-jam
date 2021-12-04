using UnityEngine;

public class Bullet: MonoBehaviour
{

    private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;
    private int _damage;

    public void Init(float ms, int damage, Vector3 initPos, Vector3 targetPos)
    {
        transform.position = initPos;
        Vector3 direction = targetPos - initPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _moveSpeed = ms;
        _damage = damage;
    }

    private void Update()
    {
        _rb.velocity = transform.right * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies") || other.gameObject.layer == 0)
            return;
        
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthComponent>()?.TakeDamage(_damage);
        }
        Destroy(gameObject);

    }
}