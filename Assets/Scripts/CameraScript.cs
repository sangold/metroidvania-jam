using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 0, -10);


    /*
    Boundaries of the camera.
    */
    private float _maxLeft = -9999999;
    private float _maxRight = 9999999;
    private float _maxUp = -9999999;
    private float _maxDown = 9999999;
    // Start is called before the first frame update
    void Start()
    {
        FollowTarget();
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }
    private void FollowTarget(){
        if (_target != null)
        {
            transform.position = _target.transform.position + _offset;
        }
    }
    private void LateUpdate()
    {
        BoundLimit();
    }
    private void BoundLimit(){
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, _maxLeft, _maxRight),
            Mathf.Clamp(transform.position.y, _maxUp, _maxDown),
            transform.position.z
        );
    }
    public void SetBounds(float left, float right, float up, float down){
        _maxLeft = left;
        _maxRight = right;
        _maxUp = up;
        _maxDown = down;
        
    }
}
