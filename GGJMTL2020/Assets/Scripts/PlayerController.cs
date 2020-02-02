using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Rewired;

public class PlayerController : MonoBehaviour
{
	public GameRessource currentStuff = GameRessource.None;

	// The Rewired player id of this character
	public int playerId = 0;

	public Player player; // The Rewired Player

	Vector2 i_movement;

    public Transform groundCheck;

    public float moveSpeed = 5f;
    public float jumpSpeed = 1f;

    private float scale;
    private float gravity;
    private float bottom;

    private bool climbingLadder = false;
    private bool grounded = false;
	private bool interacting = false;
	private bool jumpKey = false;
	private bool disableInteracting = false;

    private Rigidbody2D rb2d;
    private Animator animator;
    private Collider2D collider2d;

	// Start is called before the first frame update
	PlayerInput control;

	//System.Action myAction;
	//delegate void delgIntAsArg(int a);

	//private void myfunc(int b) { }
	void Awake()
	{
		/*delgIntAsArg myDelg = myfunc;
		myDelg += (int i) => { Debug.Log("i: "); };
		myDelg.Invoke(6);

		myAction += Awake;
		myAction.Invoke();
		myAction += () => { Awake(); };*/

		player = ReInput.players.GetPlayer(playerId);
		Debug.Log("Nb Of player = " + ReInput.players.playerCount.ToString());

		/*control = new PlayerInput();
		control.Player.Interact.performed += ctx => InteractPerformed(ctx.ReadValue<float>());
		control.Player.Interact.canceled += ctx => InteractPerformed(ctx.ReadValue<float>());
		control.Player.MoveUp.performed += ctx => OnMoveUp();

		//control.Player.Move.performed += ctx => { i_movement = ctx.ReadValue<Vector2>(); OnMove(ctx.); };
		control.Player.Move.canceled += ctx => i_movement = Vector2.zero;*/
	}

	public void InteractPerformed(float activeState)
	{
		Debug.Log("Test dans PlayerController Interact performed, State = " + activeState.ToString());
	}

	public void StopInteract()
	{
		Debug.Log("Test dans PlayerController Interact Canceled");
	}

	void Start()
    {
		animator = GetComponent<Animator> ();
        rb2d = GetComponent<Rigidbody2D> ();
        collider2d = GetComponent<Collider2D> ();
        scale = transform.localScale.x;
        gravity = rb2d.gravityScale;

        bottom = collider2d.bounds.extents.y;
    }

	void Update()
	{
		GetInput();
		ProcessInput();
	}

	//void OnEnable()
	//{
	//	control.Player.Enable();
	//}

	//void OnDisable()
	//{
	//	control.Player.Disable();
	//}

	void FixedUpdate()
    {
        // grounded?
        grounded = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector3.down, 0.5f, 1 << LayerMask.NameToLayer("Colliders"));
		Debug.Log("NB GROUND = " + hits.Length.ToString());
        foreach(RaycastHit2D hit in hits)
		{
            if(hit.collider.gameObject != gameObject)
                grounded = true;
        }

        //Move();
    }

	private void GetInput()
	{
		// Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
		// whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

		i_movement.x = player.GetAxis("MoveX"); // get input by name or action id
		i_movement.y = player.GetAxis("MoveY");
		interacting = player.GetButton("Interact");
		jumpKey = player.GetButton("Jump");
		Debug.Log("Dans Get Input");
	}

	private void ProcessInput()
	{
		// Process interact
		if (interacting == true)
		{
			Debug.Log("Dans interact");
			Collider2D[] interactInRange = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0, LayerMask.GetMask("Interact"));
			Debug.Log("On a " + interactInRange.Length.ToString() + " interactible a porter");
			if (interactInRange.Length > 0)
			{
				for (int i = 0; i < interactInRange.Length; i++)
				{
					Interactible myInteract = interactInRange[i].GetComponent<Interactible>();
					if (myInteract != null)
					{
						myInteract.StartExecute(this);
						disableInteracting = true;
						Debug.Log("Get GameObject = " + interactInRange[i].gameObject.name);
						break;
					}
				}
				return;
			}
		}
		else if (disableInteracting == true)
		{
			disableInteracting = false;
			Collider2D[] interactInRange = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0, LayerMask.GetMask("Interact"));
			if (interactInRange.Length > 0)
			{
				for (int i = 0; i < interactInRange.Length; i++)
				{
					Interactible myInteract = interactInRange[i].GetComponent<Interactible>();
					if (myInteract != null)
					{
						myInteract.EndExecute();
					}
				}
			}
		}

		Debug.Log("Test dans PlayerController On move up");

		if (grounded == true && climbingLadder == false && jumpKey == true)
		{
			Vector2 j_movement = new Vector3(0, 1.0f) * jumpSpeed;
			rb2d.AddForce(j_movement);
			grounded = false;
			FindObjectOfType<AudioManager>().Play("JumpSFX");
			return;
		}


		// Process movement
		// flip
		if (i_movement.x != 0)
			transform.localScale = new Vector3(scale * Mathf.Sign(i_movement.x), transform.localScale.y, transform.localScale.z);

		if (climbingLadder && i_movement.y > -0.7 && i_movement.y < 0.7) i_movement.y = 0f;

		Vector2 movement = climbingLadder ? i_movement * moveSpeed : new Vector2(i_movement.x * moveSpeed, rb2d.velocity.y);
		rb2d.velocity = movement;
		animator.SetFloat("Velocity", Mathf.Abs(movement.x));
	}


	//private void OnMove(InputValue value)
	//{
	//	i_movement = value.Get<Vector2>();
	//	Debug.Log("Test dans PlayerController OnMove");
	//	// flip
	//	if (i_movement.x != 0)
	//		transform.localScale = new Vector3(scale * Mathf.Sign(i_movement.x), transform.localScale.y, transform.localScale.z);

	//	if (climbingLadder && i_movement.y > -0.7 && i_movement.y < 0.7) i_movement.y = 0f;

	//	Vector2 movement = climbingLadder ? i_movement * moveSpeed : new Vector2(i_movement.x * moveSpeed, rb2d.velocity.y);
	//	rb2d.velocity = movement;
	//	animator.SetFloat("Velocity", Mathf.Abs(movement.x));
	//}

	//private void OnMoveUp()
 //   {
	//	Debug.Log("Test dans PlayerController On move up");

	//	if (!grounded || climbingLadder) return;
 //       Vector2 movement = new Vector3(0, 1.0f) * jumpSpeed;
 //       rb2d.AddForce(movement);
 //   }
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

	/*
	InputPkg GetInputPkg(int pid)
	{

	}

	private class InputPkg
	{
		public enum KeyPressMode { None, Press, Held, Release }

		public Vector2 leftAxis;
		public Vector2 rightAxis;
		public KeyPressMode xButton;



	}
	*/
}
