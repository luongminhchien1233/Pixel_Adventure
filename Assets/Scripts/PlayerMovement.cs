using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private Animator aniM;
    private BoxCollider2D coll;
    private float dirX = 0f;
    [SerializeField] private LayerMask jumbableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private AudioSource jumpSoundEffect;

    private enum MovementState {idle, runing, jumping, falling};
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        aniM = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if(dirX > 0f)
        {
            state = MovementState.runing;
            spr.flipX = false;
        } else if (dirX < 0f)
        {
            state = MovementState.runing;
            spr.flipX = true;
        } else 
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > .2f)
        {
            state = MovementState.jumping;
        } else if (rb.velocity.y < -.2f){
            state = MovementState.falling;
        }

        aniM.SetInteger("state", (int)state);
    }
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 1f, jumbableGround);
    }   
}

