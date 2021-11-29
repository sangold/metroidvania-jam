using UnityEngine;

public class ActorDetector : MonoBehaviour
{
    [SerializeField] LayerMask _collisionLayers;
    [HideInInspector]

    public bool ActorInRange => Target != null;
    [HideInInspector]
    public bool CanSee;
    [HideInInspector]

    public Transform Target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            Target = other.transform;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Target == null || other.gameObject.tag != "Player")
            return;

        Vector3 direction = (Target.position - transform.position).normalized;
        RaycastHit2D sightTest = Physics2D.Raycast(transform.position, direction, 5f, _collisionLayers);

        if (sightTest.collider != null && sightTest.collider.gameObject.tag == "Player")
        {
            CanSee = true;
        }
        else
        {
            CanSee = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Target = null;
            CanSee = false;
        }
    }
}
