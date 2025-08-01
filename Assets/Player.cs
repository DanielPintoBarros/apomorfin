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
    private bool airJump;
    private float grabMoveLoss = 0.3f;
    private Vector3 lastCheckpointPosition;


    private DraggableBlock currentBlock;
    private Vector3 blockOffset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = 2f;
        jumpForce = 4f;
        grabMoveLoss = 0.7f; // não pode ser maior que 1
        gameObject.tag = "Player";
        airJump = false;
        lastCheckpointPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 move;
        if (currentBlock != null)
        {
            move = moveDirection * moveSpeed * (1.0f - grabMoveLoss) * Time.deltaTime;
        }
        else
        {
            move = moveDirection * moveSpeed * Time.deltaTime;
        }

        if (moveDirection != Vector3.zero && currentBlock == null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
        }
        rb.MovePosition(rb.position + move);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || !airJump))
        {
            // Tratamento para o pulo duplo
            if (!isGrounded && !airJump)
            {
                airJump = true;
                Vector3 velocity = rb.velocity;
                velocity.y = 0f;
                rb.velocity = velocity;
            }
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            airJump = false;
            Camera.main.GetComponent<CameraFollowGroundY>().UpdateGroundY(collision.contacts[0].point.y);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Saiu do chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
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
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            // Atualiza o último ponto de respawn
            lastCheckpointPosition = other.transform.position;
            Debug.Log("Checkpoint atualizado!");
        }

        if (other.CompareTag("Damage"))
        {
            // Morreu - teleporta para o último respawn
            DieAndRespawn();
        }
    }

    void DieAndRespawn()
    {
        // Opcional: adicionar delay, efeitos, resetar blocos, etc.
        rb.velocity = Vector3.zero; // Zera velocidade ao morrer
        if (currentBlock != null)
        {
            currentBlock.Release();
            currentBlock = null;
        }
        transform.position = lastCheckpointPosition;
        Debug.Log("Morreu! Voltando ao último checkpoint.");
    }
}
