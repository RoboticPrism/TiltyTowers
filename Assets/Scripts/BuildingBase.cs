using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : MonoBehaviour
{
    [SerializeField]
    private Transform explosionPosition;
    [SerializeField]
    private GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
