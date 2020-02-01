using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 i_movement;

    public Transform groundCheck;

    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;

    private float scale;
    private float gravity;
    private float bottom;

    private bool climbingLadder = false;
    private bool grounded = false;

    private Rigidbody2D rb2d;
    private Animator animator;
    private Collider2D collider2d;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator> ();
        rb2d = GetComponent<Rigidbody2D> ();
        collider2d = GetComponent<Collider2D> ();
        scale = transform.localScale.x;
        gravity = rb2d.gravityScale;

        bottom = collider2d.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        // grounded?
        grounded = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector3.down, 0.1f);
        foreach(RaycastHit2D hit in hits) {
            if(hit.collider.gameObject != gameObject)
                grounded = true;
        }

        Move();
    }

    private void Move()
    {
        if(climbingLadder && i_movement.y > -0.7 && i_movement.y < 0.7) i_movement.y = 0f;

        Vector2 movement = climbingLadder ? i_movement * moveSpeed : new Vector2(i_movement.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = movement;
        animator.SetFloat("Velocity", Mathf.Abs(movement.x));
    }

    private void OnMove(InputValue value)
    {
        i_movement = value.Get<Vector2>();

        // flip
        if(i_movement.x != 0)
            transform.localScale = new Vector3(scale * Mathf.Sign(i_movement.x), transform.localScale.y, transform.localScale.z);
    }

    private void OnMoveUp()
    {
        if(!grounded || climbingLadder) return;
        Vector2 movement = new Vector3(0, 1.0f) * jumpSpeed;
        rb2d.AddForce(movement);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Ladder") {
            climbingLadder = true;
            rb2d.gravityScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "Ladder") {
            climbingLadder = false;
            rb2d.gravityScale = gravity;
        }
    }
}
