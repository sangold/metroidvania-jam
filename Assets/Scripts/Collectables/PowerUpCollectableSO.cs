using UnityEngine;

[CreateAssetMenu(fileName = "PUCollectableSO", menuName = "Assets/Collectables/PowerUp")]
public class PowerUpCollectableSO : CollectableSO
{
    public PowerUpType PowerUpType;
    public ModalContentSO Description;

    public PowerUpCollectableSO()
    {
        CollectableType = CollectableEnum.ABILITY;
    }
}
