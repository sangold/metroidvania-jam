using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{
    Player _self;

    private int _fireTileLayer = 8;
    // Start is called before the first frame update
    void Start()
    {
        _self = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == _fireTileLayer && _self.state != Player._finiteState.ghostDash)
        {
            _self.TakeDamage(10,1,new Vector2(other.gameObject.transform.position.x - _self.transform.position.x,0).normalized * -10, 1);
        }
    }
}
