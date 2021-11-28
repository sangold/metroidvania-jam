using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    public static Action OnLoadingScene;
    public static Action OnSceneLoaded;
    public enum Destination { A, B, C, D, E, F }
    [SerializeField][Tooltip("Scene Name (don't forget to add it to the build Menu)")]
    private string _sceneToLoad;
    public Destination DestinationIdentification;
    public Transform ExitPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        DontDestroyOnLoad(gameObject);
        OnLoadingScene?.Invoke();
        yield return new WaitForSeconds(.35f);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        yield return SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneToLoad));
        Portal destPortal = GetDestinationPortal();
        UpdatePlayer(destPortal);
        yield return new WaitForSeconds(.25f);
        Destroy(gameObject);
        OnSceneLoaded?.Invoke();
    }

    private void UpdatePlayer(Portal destPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 destination = destPortal.ExitPoint.position;
        destination.z = 0;
        player.transform.position = destination;
    }

    private Portal GetDestinationPortal()
    {
        foreach(Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.DestinationIdentification != DestinationIdentification) continue;

            return portal;
        }

        return null;
    }
}
