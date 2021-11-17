using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomLoadTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isLoadedScene = false;

    [SerializeField]
    private string sceneNameToLoad = "";

    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;//make the yellow square invisible ingame.

        CheckIfSceneAlreadyLoaded();
    }
    private void CheckIfSceneAlreadyLoaded(){
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == gameObject.name)
            {
                isLoadedScene = true;
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.name == "RoomTriggerCollider"){
        Player player = other.gameObject.GetComponentInParent<Player>();
        Debug.Log(player);
        if (player != null)
        {
            if (isLoadedScene == false)
            {
                SceneManager.LoadScene(sceneNameToLoad, LoadSceneMode.Additive);
                isLoadedScene = true;
            }
            player._touchingARoom = sceneNameToLoad;
            /*Camera.main.GetComponent<CameraScript>().SetBounds(
                gameObject.transform.position.x,
                gameObject.transform.position.x + _spriteRenderer.sprite.rect.width / 2,
                gameObject.transform.position.y,
                gameObject.transform.position.y + _spriteRenderer.sprite.rect.height / 2);
            */
        }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "RoomTriggerCollider"){
            Player player = other.gameObject.GetComponentInParent<Player>();
            if (player != null) {
                if (isLoadedScene == true)
                {
                 SceneManager.UnloadSceneAsync(sceneNameToLoad);
                  isLoadedScene = false;
                }
                if (player._touchingARoom == sceneNameToLoad){
                     player._touchingARoom = null;
                }
            }
        }
    }
}
