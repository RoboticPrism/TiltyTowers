using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private OVRScreenFade ovrScreenFade;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        ovrScreenFade = this.GetComponent<OVRScreenFade>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene()
    {
        StartCoroutine("SceneTransition");
    }

    IEnumerator SceneTransition()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        // Fade out
        ovrScreenFade.FadeOut();
        while (ovrScreenFade.IsTransitioning())
        {
            yield return null;
        }

        // Swap scenes
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
        while (unloadOp != null && !unloadOp.isDone )
        {
            yield return null;
        }

        while (loadOp != null && !loadOp.isDone)
        {
            yield return null;
        }

        // Set connections for new stackables
        gameManager.ResetConnections();

        // Fade in
        ovrScreenFade.FadeIn();
        while (ovrScreenFade.IsTransitioning())
        {
            yield return null;
        }
    }
}
