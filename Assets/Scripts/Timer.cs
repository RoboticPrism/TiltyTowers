using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private Coroutine runningCoroutine;

    private GameManager gameManager;
    private float maxTimerSeconds;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        maxTimerSeconds = gameManager.GetMaxTimeSeconds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer()
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        image.gameObject.SetActive(true);
        runningCoroutine = StartCoroutine("timerCoroutine");
    }

    public void StopTimer()
    {
        image.gameObject.SetActive(false);
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
    }

    IEnumerator timerCoroutine()
    {
        while (gameManager.GetCurrentTimeSeconds() > 0f) {
            float currentTimeSeconds = 
            image.fillAmount = (maxTimerSeconds - gameManager.GetCurrentTimeSeconds()) / maxTimerSeconds;
            yield return null;
        }
    }
}
