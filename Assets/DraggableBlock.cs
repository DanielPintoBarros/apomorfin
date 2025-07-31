using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableBlock : MonoBehaviour
{
    private Rigidbody rb;
    private Transform touchingPlayer ;
    private bool isBeingCarried  = false;
    private Quaternion initialRotation;

    void OnCollisionStay(Collision collision)
    {
        if (!isBeingCarried && collision.collider.CompareTag("Player"))
        {
            touchingPlayer = collision.transform;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform == touchingPlayer)
        {
            touchingPlayer = null;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public bool CanBeGrabbed()
    {
        return touchingPlayer  != null && !isBeingCarried;
    }

    public void Grab()
    {
        isBeingCarried = true;
        initialRotation = transform.rotation;
    }

    public void Follow(Vector3 move)
    {
        transform.position = transform.position + move;
        transform.rotation = initialRotation;
    }

    public void Release()
    {
        isBeingCarried = false;
    }
}