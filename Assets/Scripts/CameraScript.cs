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

    private float height;

    private float width;

    [SerializeField]
    private float _transitionSpeed = 10;
    private void GetCameraSize()
    {
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }
    // Start is called before the first frame update
    void Start()
    {
        FollowTarget();
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
            transform.position = _target.transform.position + _offset;
        }
    }
    private void LateUpdate()
    {
        GetCameraSize();
        BoundLimit();
    }
    private void BoundLimit(){
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, _maxLeft + width/2, _maxRight - width / 2),
            Mathf.Clamp(transform.position.y, _maxUp + height / 2, _maxDown - height / 2),
            transform.position.z
        );
    }
    public void SetBounds(float left, float right, float up, float down){
        _maxLeft = Mathf.Lerp(_maxLeft, left, Time.fixedDeltaTime * _transitionSpeed);
        _maxRight = Mathf.Lerp(_maxRight, right, Time.fixedDeltaTime * _transitionSpeed);
        _maxUp = Mathf.Lerp(_maxUp, up, Time.fixedDeltaTime * _transitionSpeed);
        _maxDown = Mathf.Lerp(_maxDown, down, Time.fixedDeltaTime * _transitionSpeed);

        if (Mathf.Abs(left - _maxLeft) < .5f && _maxLeft != left) _maxLeft = left;
        if (Mathf.Abs(right - _maxRight) < .5f && _maxRight != right) _maxRight = right;
        if (Mathf.Abs(up - _maxUp) < .5f && _maxUp != up) _maxUp = up;
        if (Mathf.Abs(down - _maxDown) < .5f && _maxDown != down) _maxDown = down;
    }
}
