using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Camera MirrorCamera;
    public RenderTexture RenderTexture;
    public SpriteRenderer Sr;
    [SerializeField]
    private SpriteRenderer MirrorSpriteRenderer;
    [SerializeField]
    private GameObject _buttonIndicator;
    public enum MirrorType { FRONT, SIDE, WORLD }
    [SerializeField]
    private MirrorType _mirrorType;

    private BoxCollider2D _boxCollider;

    private Animator _animator;

    public bool IsDisabled;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        MirrorCamera.targetTexture = Instantiate(RenderTexture);
        Sr.material.SetTexture("_BaseMap", MirrorCamera.targetTexture);
        _buttonIndicator.SetActive(false);
        if (IsDisabled)
            _boxCollider.enabled = false;
    }

    public void SetActiveMirror(bool isActive)
    {
        IsDisabled = !isActive;
        _boxCollider.enabled = !IsDisabled;
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
        Open();
        MirrorSpriteRenderer.material.SetFloat("_isOutline", 1f);
        _buttonIndicator.SetActive(true);
    }

    private void HideInteraction()
    {
        Close();
        MirrorSpriteRenderer.material.SetFloat("_isOutline", 0f);
        _buttonIndicator.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null || !player.CheckAbility(PowerUpType.MIRROR))
            return;

        player.IsInInteractivePortalRange = true;
        ShowInteraction();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null || !player.CheckAbility(PowerUpType.MIRROR))
            return;

        player.IsInInteractivePortalRange = false;
        HideInteraction();
    }
}
