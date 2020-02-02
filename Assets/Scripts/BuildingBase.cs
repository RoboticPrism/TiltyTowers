using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : MonoBehaviour
{
    [SerializeField]
    private Transform explosionPosition;
    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private Transform successPosition;
    [SerializeField]
    private GameObject successPrefab;

    [SerializeField]
    private AudioClip explosionClip;
    [SerializeField]
    private AudioClip successClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetExplosionPosition()
    {
        return explosionPosition;
    }

    public void CreateExplosion()
    {
        Instantiate(explosionPrefab, explosionPosition.transform.position, explosionPosition.transform.rotation);
        audioSource.PlayOneShot(explosionClip);
    }

    public void CreateSuccess()
    {
        Instantiate(successPrefab, successPosition.transform.position, successPosition.transform.rotation);
        audioSource.PlayOneShot(successClip);
    }
}
