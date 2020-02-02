using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [SerializeField]
    private AudioClip gameMusic;

    [SerializeField]
    private AudioClip winMusic;

    [SerializeField]
    private AudioClip failMusic;

    [SerializeField]
    private AudioClip evalMusic;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        PlayGameMusic();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameMusic()
    {
        audioSource.clip = gameMusic;
        audioSource.Play();
        audioSource.loop = true;
    } 

    public void PlayWinMusic()
    {
        audioSource.clip = winMusic;
        audioSource.Play();
        audioSource.loop = false;
    }

    public void PlayFailMusic()
    {
        audioSource.clip = failMusic;
        audioSource.Play();
        audioSource.loop = false;
    }

    public void PlayEvalMusic()
    {
        audioSource.clip = evalMusic;
        audioSource.Play();
        audioSource.loop = true;
    }
}
