using Reapling.SaveLoad;
using UnityEngine;

public class Collectable : MonoBehaviour, ISaveable
{
    [SerializeField] private CollectableSO _collectableInfo;

    [System.Serializable]
    public class CollectibleData
    {
        public bool IsTaken;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null){
            player.GetItem(_collectableInfo);
            gameObject.SetActive(false);
        }
    }

    public object CaptureState()
    {
        return new CollectibleData { IsTaken = !gameObject.activeInHierarchy };
    }

    public void RestoreState(object state)
    {
        CollectibleData data = JsonSerializer.Deserialize<CollectibleData>(state);
        if (data.IsTaken)
            gameObject.SetActive(false);
    }
}
