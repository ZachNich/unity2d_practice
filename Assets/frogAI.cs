using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogAI : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    [SerializeField] float frogSpeed = 3f;
    [SerializeField] float frogJump = 5f;  // if set higher than 5, doesn't alternate jumpDir for some reason
    [SerializeField] Transform groundCheck;
    string jumpDir = "left";
    private bool isGrounded;
    private bool readyToJump;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(jumpCheck());
    }

    private void FixedUpdate()
    {        
        // Checks if frog is grounded or not
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
            animator.Play("frog_idle");
        }
        else
        {
            isGrounded = false;
            if (rb2d.velocity.y > 0)
            {
                animator.Play("frog_jump_up");
            }
            else if (rb2d.velocity.y < 0)
            {
                animator.Play("frog_jump_down");
            }
        }

        // Jumps left or right
        if (jumpDir == "right" && isGrounded && readyToJump)
        {
            // jump right
            spriteRenderer.flipX = true;
            rb2d.velocity = new Vector2(frogSpeed, frogJump);
            jumpDir = "left";
            readyToJump = false;
        }
        else if (jumpDir == "left" && isGrounded && readyToJump)
        {
            // jump left
            spriteRenderer.flipX = false;
            rb2d.velocity = new Vector2(-frogSpeed, frogJump);
            jumpDir = "right";
            readyToJump = false;
        }
    }

    // Jump at random intervals
    IEnumerator jumpCheck()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        if (isGrounded)
        {
            readyToJump = true;
        }
        StartCoroutine(jumpCheck());
    }
}
