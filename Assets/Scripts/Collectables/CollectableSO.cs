using UnityEngine;
public enum CollectableEnum{HealthGain}
[CreateAssetMenu(fileName = "CollectableSO", menuName = "Assets/Collectables/CollectableState")]
public class CollectableSO : ScriptableObject
{
    public CollectableEnum _collectableIs;
}
