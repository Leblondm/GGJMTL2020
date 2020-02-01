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
    
    // Start is called before the first frame update
    void Start()
    {
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
        Vector2 movement = new Vector3(i_movement.x, 0) * moveSpeed;
        rb2d.velocity = movement * Time.fixedDeltaTime;
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
        rb2d.velocity = new Vector2(rb2d.velocity.x, movement.y);
    }
}
