using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour
{
    [SerializeField]
    private bool inBounds = false;

    private Rigidbody rigidbody;
    private Collider collider;
    private OVRGrabbable grabbable;
    private OculusSampleFramework.DistanceGrabbable distanceGrabbable;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<Collider>();
        grabbable = this.GetComponent<OVRGrabbable>();
        distanceGrabbable = this.GetComponent<OculusSampleFramework.DistanceGrabbable>();

        startingPosition = this.transform.position;
        startingRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<OutOfBoundsCatcher>())
        {
            Debug.Log("Enter collider");
            inBounds = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<OutOfBoundsCatcher>())
        {
            Debug.Log("Exit collider");
            inBounds = true;
        }
    }

    // Surface if the item is currently held
    public bool CheckIfGrabbed() => grabbable.isGrabbed;

    // Surface if the item is in bounds
    public bool CheckInBounds() => inBounds;

    // Make this grabbable unmovible and not grabbable
    public void MakeStatic()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        distanceGrabbable.enabled = false;
    }

    // Make this object moveable and grabbable
    public void MakeInteractible()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        distanceGrabbable.enabled = true;
    }
}
