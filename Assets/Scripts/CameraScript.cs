using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private BoxCollider2D _boundBox;
    private float _halfHeight;
    private float _halfWidth;

    private void GetCameraSize()
    {
        _halfHeight = Camera.main.orthographicSize;
        _halfWidth = _halfHeight * Camera.main.aspect;
    }
    // Start is called before the first frame update
    void Start()
    {
        GetCameraSize();
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }
    private void FollowTarget(){
        if (_target != null)
        {
            transform.position = new Vector3(
            Mathf.Clamp(_target.transform.position.x, _boundBox.bounds.min.x + _halfWidth, _boundBox.bounds.max.x - _halfWidth),
            Mathf.Clamp(_target.transform.position.y, _boundBox.bounds.min.y + _halfHeight, _boundBox.bounds.max.y - _halfHeight),
            -10);
        } else
        {
            _target = FindObjectOfType<Player>().gameObject;
        }
    }
}
