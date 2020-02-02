using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    private bool active = true;

    private LevelLoader levelLoader;
    private OVRGrabbable grabbable;

    [SerializeField]
    private GameObject spinningChild;

    [SerializeField]
    private float spinSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        levelLoader = GameObject.FindObjectOfType<LevelLoader>();
        grabbable = this.GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        spinningChild.transform.rotation = Quaternion.Euler(spinningChild.transform.rotation.eulerAngles + new Vector3(0f, spinSpeed + Time.deltaTime, 0f));
        if (active && grabbable.isGrabbed)
        {
            levelLoader.NextScene();
            active = false;
        }
    }
}
