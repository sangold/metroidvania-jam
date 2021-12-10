using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UIModalEvent", fileName = "New modal event")]
public class UIModalEvent: ScriptableObject
{
    public UnityAction<ModalContentSO> OnModalOpen;

    public void RaiseEvent(ModalContentSO modalContent)
    {
        OnModalOpen?.Invoke(modalContent);
    }
}