using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton : MonoBehaviour
{
    [SerializeField]
    private float returnSpeed;
    [SerializeField]
    private float activationDistance;

    private float distance;

    private bool pressed = false;
    private bool released = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rigidbody;
    private OVRGrabbable grabbable;

    private GameManager gameManager;


    void Start()
    {
        // Remember start position of button
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        rigidbody = this.GetComponent<Rigidbody>();
        grabbable = this.GetComponent<OVRGrabbable>();

        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        released = false;

        this.transform.localRotation = startRotation;
        
        // Use local position instead of global, so button can be rotated in any direction
        Vector3 localPos = transform.localPosition;
        localPos.x = startPosition.x;
        localPos.z = startPosition.z;
        
        // Clamp range
        if (localPos.y > startPosition.y)
        {
            localPos.y = startPosition.y;
        }
        if (localPos.y < startPosition.y - activationDistance)
        {
            localPos.y = startPosition.y - activationDistance; 
        }

        transform.localPosition = localPos;

        // Return button to startPosition
        if (!grabbable.isGrabbed)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * returnSpeed);
        }

        //Get distance of button press. Make sure to only have one moving axis.
        Vector3 allDistances = transform.localPosition - startPosition;
    
        distance = Mathf.Abs(allDistances.y);
     
        float pressComplete = Mathf.Clamp(1 / activationDistance * distance, 0f, 1f);

        //Activate pressed button
        if (pressComplete >= 0.7f && !pressed)
        {
            pressed = true;
            gameManager.Reset();
        }
        //Dectivate unpressed button
        else if (pressComplete <= 0.2f && pressed)
        {
            pressed = false;
            released = true;
        }
    }

    // Bring the button back to its default location
    public void Reset()
    {
        this.transform.localPosition = startPosition;
        // Kill all velocity too
        rigidbody.velocity = Vector3.zero;
    }
}
