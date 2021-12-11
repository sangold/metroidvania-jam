using UnityEngine;

[CreateAssetMenu(menuName = "Content/ModalContent")]
public class ModalContentSO: ScriptableObject
{
    public string Title;
    [TextArea(10, 30)]
    public string Content;
    public Sprite Image;
}