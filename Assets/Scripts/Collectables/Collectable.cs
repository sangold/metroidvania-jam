using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSO _collectableInfo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null){
            player.GetItem(_collectableInfo);
            Destroy(this.gameObject);
        }
    }
}
