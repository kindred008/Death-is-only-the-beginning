using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static bool playerSpawned;

    [SerializeField] float playerSpeed = 5.0f;
    [SerializeField] float playerJumpSpeed = 10.0f;

    private float characterMove;
    private bool charFlipped;
    private bool grounded;

    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private SpriteRenderer playerRenderer;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerRb.velocity.y < -2)
        {
            playerAnim.SetBool("playerFalling", true);
        } else
        {
            playerAnim.SetBool("playerFalling", false);
        }

        if (playerSpawned)
        {
            characterMove = Input.GetAxisRaw("Horizontal");
            /*if (playerAnim.GetBool("playerFalling"))
                characterMove *= 0.5f;*/

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

        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            playerRb.AddForce(Vector2.up * playerJumpSpeed, ForceMode2D.Impulse);
            grounded = false;
            playerAnim.SetBool("playerJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerSpawned = false;
            SceneManager.LoadScene(0);
        }

        Debug.Log(grounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            playerAnim.SetBool("playerJumping", false);
        }
    }

    private void FixedUpdate()
    {
        playerRb.velocity = new Vector2(characterMove * playerSpeed, playerRb.velocity.y);
    }
}
