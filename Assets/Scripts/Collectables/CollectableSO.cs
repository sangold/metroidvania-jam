using UnityEngine;
public enum CollectableEnum{
    HEALTH_UP,
    MANA_UP,
    ABILITY }
[CreateAssetMenu(fileName = "CollectableSO", menuName = "Assets/Collectables/CollectableState")]
public class CollectableSO : ScriptableObject
{
    public CollectableEnum CollectableType;
    public Color Color;
    public string AbilityName;
}
