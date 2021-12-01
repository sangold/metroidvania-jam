using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSO _currentCollectable;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   private void OnTriggerEnter2D(Collider2D other)
   {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null){
            if (_currentCollectable._collectableIs == CollectableEnum.HealthGain){
                Debug.Log("You pick up HEALTH GAIN :D!");
                HealthComponent healthComponent = player.GetComponent<HealthComponent>();
                healthComponent.HealthPiecesCollected += 1;
            }
            Destroy(this.gameObject);
        }
    }
}
