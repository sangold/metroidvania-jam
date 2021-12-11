using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Camera MirrorCamera;
    public RenderTexture RenderTexture;
    public SpriteRenderer Sr;
    public enum MirrorType { FRONT, SIDE, WORLD }
    [SerializeField]
    private MirrorType _mirrorType;

    private Animator _animator;
    [HideInInspector]
    public bool IsDisabled;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        MirrorCamera.targetTexture = Instantiate(RenderTexture);
        Sr.material.SetTexture("_BaseMap", MirrorCamera.targetTexture);
    }

    public void Open()
    {
        _animator.SetTrigger("Open");
    }

    public void Close()
    {
        _animator.SetTrigger("Close");
    }

    public void ForceClose()
    {
        _animator.SetTrigger("ForceClose");
    }

    private void ShowInteraction()
    {

    }

    private void HideInteraction()
    { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null || !player.CheckAbility(PowerUpType.MIRROR))
            return;

        ShowInteraction();
    }
}
