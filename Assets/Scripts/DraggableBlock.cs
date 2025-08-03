using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class DraggableBlock : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider bc;
    private Transform touchingPlayer;
    private bool isBeingCarried  = false;
    private Quaternion initialRotation;

    [SerializeField] GameObject eBtn;

    void OnCollisionStay(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            if (!isBeingCarried && collision.collider.CompareTag("Player"))
            {
                touchingPlayer = collision.transform;
            }
        }
        else
        {
            rb.isKinematic = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            touchingPlayer = null;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eBtn.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            eBtn.SetActive(false);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        bc = GetComponent<BoxCollider>();
        bc.isTrigger = false;
    }

    public bool CanBeGrabbed()
    {
        return touchingPlayer  != null && !isBeingCarried;
    }

    public void Grab()
    {
        rb.isKinematic = true;
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
        rb.isKinematic = false;
    }
}