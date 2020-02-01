using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 i_movement;

    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;

    private float scale;

    private Rigidbody2D rb2d;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator> ();
        rb2d = GetComponent<Rigidbody2D> ();
        scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movement = new Vector3(i_movement.x * moveSpeed, rb2d.velocity.y);
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
        Vector2 movement = new Vector3(0, 1.0f) * jumpSpeed;
        rb2d.AddForce(movement);
    }
}
