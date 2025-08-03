using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private bool canLearnFly = false, canLearnPush = false, canFly = false, canPush = false; 

    public float moveSpeed;
    public float jumpForce;
    private Rigidbody rb;
    public bool isGrounded;
    public bool airJump;
    public bool isDead = false;
    private float grabMoveLoss = 0.3f;
    public Vector3 lastCheckpointPosition;
    public RespawnPoint respawnPoint;
    private Animator animator;
    public Animator uiAnim;
    public string currentAnimation;

    public GameObject feet, wings, spaceBtn;

    private float respawnDelay = 1.2f;
    private float currentDelay = 0f;

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
        animator = GetComponent<Animator>();
        currentAnimation = "Idle";
    }

    // Update is called once per frame
    void Update()
    {
        if (canPush)
        {
            feet.SetActive(true);
        }

        if (canPush)
        {
            wings.SetActive(true);
            spaceBtn.SetActive(true);
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        if (canPush)
        {
            if ((moveX != 0.0f || moveZ != 0.0f))
            {
                animator.SetBool("Walk", true);
                currentAnimation = "Walk";
            }
            else if (moveX == 0.0f && moveZ == 0.0f)
            {
                animator.SetBool("Walk", false);
                currentAnimation = "Idle";
            }
        }
        else
        {
            if ((moveX != 0.0f || moveZ != 0.0f) && currentAnimation == "Idle")
            {
                animator.SetBool("Swim", true);
                currentAnimation = "Swim";
            }
            else if (moveX == 0.0f && moveZ == 0.0f && currentAnimation == "Swim")
            {
                animator.SetBool("Swim", false);
                currentAnimation = "Idle";
            }
        }
        

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

        if (canPush && canFly)
        {
            if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || !airJump))
            {
                if (currentAnimation == "Walk" || currentAnimation == "Fly" || currentAnimation == "Swim")
                {
                    animator.SetBool(currentAnimation, false);
                }
                animator.SetBool("Fly", true);

                if (!isGrounded && !airJump)
                {
                    airJump = true;
                }



                Vector3 velocity = rb.velocity;
                velocity.y = 0f;
                rb.velocity = velocity;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && canPush)
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

        if (isDead)
        {
            currentDelay += Time.deltaTime;

            if (currentDelay >= respawnDelay)
            {
                DieAndRespawn();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CheckIfGrounded();
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

    private void SetRespawnBlock()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1.2f);
        foreach (var respawn in FindObjectsOfType<RespawnPoint>())
        {
            respawnPoint = respawn;
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            RespawnPoint newRespawn = other.GetComponent<RespawnPoint>();
            if (newRespawn != null && newRespawn != respawnPoint)
            {
                if (respawnPoint != null)
                {
                    respawnPoint.SpawnDesactivate();
                }
                respawnPoint = newRespawn;
                respawnPoint.SpawnActivate();
            }
            
        }

        if (other.CompareTag("Damage"))
        {
            // Morreu - teleporta para o último respawn
            uiAnim.Play("WaterBillboardTransitionVerse");
            isDead = true;
        }
    }

    void DieAndRespawn()
    {
        animator.SetTrigger("Hurt");
        uiAnim.Play("WaterBillboardTransitionReverse");

        rb.velocity = Vector3.zero; // Zera velocidade ao morrer
        if (currentBlock != null)
        {
            currentBlock.Release();
            currentBlock = null;
        }
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.GetSpawnPosition();
        }

        animator.SetTrigger("Reset");
        isDead = false;
        currentDelay = 0f;
    }
    
    void CheckIfGrounded()
    {
    Vector3 origin = transform.position + Vector3.down * 0.1f;
    float radius = 0.1f;

    // Detecta se há colisores abaixo do jogador (exclui o próprio player)
    isGrounded = Physics.CheckSphere(origin, radius, ~LayerMask.GetMask("Player"));

        if (isGrounded)
        {
            animator.SetBool("Fly", false);
            airJump = false;
        }
    }

    public void CanLearnAbility(string ability)
    {
        if (ability == "push") 
        {
            canLearnPush = true;
        }

        if (ability == "fly")
        {
            canLearnFly = true;
        }
    }

    public void LearnAbility(string ability)
    {
        if (ability == "push")
        {
            canPush = true;
        }

        if (ability == "fly")
        {
            canFly = true;
        }
    }

    public bool CheckCanLearnAbility(string ability)
    {
        if (ability == "push" && canLearnPush == true)
        {
            return true;
        }

        if (ability == "fly" && canLearnFly == true)
        {
            return false;
        }

        return false;
    }
}
