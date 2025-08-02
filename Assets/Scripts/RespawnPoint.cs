using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class RespawnPoint : MonoBehaviour
{
    private Rigidbody rb;
    public Transform spawnPosition;
    private BoxCollider bc;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
    }
    public void SpawnActivate()
    {
        animator.SetBool("Active", true);
    }

    public void SpawnDesactivate()
    {
        animator.SetBool("Active", false);
    }
    

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition != null ? spawnPosition.position : transform.position;
    }
}