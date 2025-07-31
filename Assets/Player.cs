using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody rb;
    private bool isGrounded;


    private DraggableBlock currentBlock;
    private Vector3 blockOffset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = 2f;
        jumpForce = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
        Vector3 velocity = rb.velocity;

        if (moveDirection != Vector3.zero && currentBlock == null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        rb.MovePosition(rb.position + move);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentBlock != null)
            {
                currentBlock.Release();
                currentBlock = null;
            }
            else
            {
                TryGrabBlock();
            }
        }

        if (currentBlock != null)
        {
            currentBlock.Follow(move);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Detecta se está tocando o chão
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        // Saiu do chão
        isGrounded = false;
    }

    private void TryGrabBlock()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1.2f);
        foreach (var block in FindObjectsOfType<DraggableBlock>())
        {
            if (block.CanBeGrabbed())
            {
                currentBlock = block;
                block.Grab();
                break;
            }
        }
    }
}
