using MJ.GameState;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _content;
    [SerializeField]
    private TextMeshProUGUI _buttonText;
    [SerializeField]
    private Image _image;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Init(ModalContentSO content)
    {
        _title.text = content.Title;
        _content.text = content.Content;
        _image.sprite = content.Image;
        gameObject.SetActive(true);
        GameManager.Instance.SetState(new PauseGameState());
    }

    public void Close()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetState(new GameLoopState());
    }
}
