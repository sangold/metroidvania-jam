using UnityEngine;
using UnityEngine.UI;

public class UILevelSquare : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Outline _outline;
    [SerializeField] private RectTransform _rt;
    private float _squareSize = 32f;
    private Color _darkColor;
    private Color _mainColor;
    private Color _lightColor;
    private Color _lighterColor;
    private bool _isExplored = false;
    private bool _isActive = false;

    public void Init(Level level, Transform parent, Vector2 offset)
    {
        SetColor(
            Level.GetDarkestColor(level.World),
            Level.GetMainColor(level.World),
            Level.GetLightColor(level.World),
            Level.GetLightestColor(level.World)
        );
        transform.SetParent(parent);
        _rt.anchoredPosition = new Vector3(
            level.PosX * _squareSize + offset.x,
            -(level.PosY * _squareSize) - offset.y,
            0
        );
    }

    public void SetColor(Color d, Color m, Color l, Color lr)
    {
        _darkColor = d;
        _mainColor = m;
        _lightColor = l;
        _lighterColor = lr;
    }
    public void SetActive(bool isActive)
    {
        _isActive = isActive;
        _outline.effectColor = isActive ? _lighterColor : _darkColor;
    }

    public void SetExplored(bool isExplored)
    {
        _isExplored = isExplored;
        _image.color = isExplored ? _mainColor : _lightColor;
    }
}
