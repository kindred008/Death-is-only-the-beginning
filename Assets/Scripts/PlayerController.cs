using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static bool playerSpawned;

    [SerializeField] float playerSpeed = 5.0f;
    [SerializeField] float playerJumpSpeed = 10.0f;

    private float characterMove;
    private bool charFlipped;
    private bool grounded;
    private bool playerDead;

    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private SpriteRenderer playerRenderer;

    private LevelManager levelManager;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        if (playerSpawned)
        {
            PlayerMove();
            PlayerJump();
            PlayerFalling();
        }
    }

    private void FixedUpdate()
    {
        playerRb.velocity = new Vector2(characterMove * playerSpeed, playerRb.velocity.y);
    }

    private void PlayerMove()
    {
        // Used in FixedUpdate()
        characterMove = Input.GetAxisRaw("Horizontal");

        playerAnim.SetFloat("playerSpeed", Mathf.Abs(characterMove));

        if (characterMove > 0 && charFlipped)
        {
            playerRenderer.flipX = false;
            charFlipped = false;
        }
        else if (characterMove < 0 && !charFlipped)
        {
            playerRenderer.flipX = true;
            charFlipped = true;
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            playerRb.AddForce(Vector2.up * playerJumpSpeed, ForceMode2D.Impulse);
            grounded = false;
            playerAnim.SetBool("playerJumping", true);
        }
    }

    private void PlayerFalling()
    {
        if (playerRb.velocity.y < -2)
        {
            playerAnim.SetBool("playerFalling", true);
        }
        else
        {
            playerAnim.SetBool("playerFalling", false);
        }
    }

    public void PlayerSpawn()
    {
        transform.position = levelManager.currentCheckpoint.position;
        playerDead = false;
        playerRb.simulated = true;
        playerRenderer.enabled = true;
        playerAnim.Rebind();
        playerAnim.Update(0f);
    }

    public void PlayerDeath()
    {
        if (!playerDead)
        {
            playerDead = true;
            playerRb.simulated = false;
            playerSpawned = false;
            characterMove = 0;
            StartCoroutine(PlayerDeathRoutine());
        }
    }

    private IEnumerator PlayerDeathRoutine()
    {
        playerAnim.SetTrigger("playerDeath");
        yield return null;

        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length);

        playerRenderer.enabled = false;

        yield return new WaitForSeconds(0.5f);

        PlayerSpawn();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            playerAnim.SetBool("playerJumping", false);
        }
    }

    public void bounce(float bounceStrength)
    {
        playerRb.AddForce(Vector2.up * bounceStrength, ForceMode2D.Impulse);
        playerAnim.SetBool("playerJumping", false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            playerAnim.SetBool("playerJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(LeaveGround());
        }
    }

    private IEnumerator LeaveGround()
    {
        yield return new WaitForSeconds(0.1f);
        grounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            if (levelManager.AttemptFinish())
            {
                characterMove = 0;
                playerSpawned = false;
                playerAnim.SetTrigger("levelFinished");
            }
        }

        if (collision.CompareTag("Checkpoint"))
        {
            levelManager.NewCheckPoint(collision.transform);
        }
    }
}
