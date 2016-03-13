using Assets.Scripts.Game;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    public float Speed = 5f;
    public float jumpForce = 700f;
    public int maxJumpCount = 2;

    [Header("Ground Detection")]
    public Transform GroundCheck;
    public float GroundCheckRadius = 0.01f;
    public LayerMask GroundMask;

    private Rigidbody2D _rigidbody;
    private bool isGrounded = false;
    private int jumpCount = 0;

    // MonoBehavior methods
    //***********************************
    void Start()
    {
        GameLogic.Instance.RegisterPlayer(this);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnDestroy()
    {
        GameLogic.Instance.RemovePlayer(this);
    }

    void FixedUpdate()
    {
        if (GameLogic.Instance.GetCurrentPlayer() != this)
        {
            return;
        }

		if (GameLogic.Instance.MovementBlocked) {
			return;
		}

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundMask);
        if (isGrounded)
        {
            jumpCount = 0;
        }

        float move = Input.GetAxis("Horizontal");

        _rigidbody.velocity = new Vector2(move * Speed, _rigidbody.velocity.y);
    }

    void Update()
    {
        if (GameLogic.Instance.GetCurrentPlayer() != this)
        {
            return;
        }

		if (GameLogic.Instance.MovementBlocked) {
			return;
		}

        if ((isGrounded || jumpCount < maxJumpCount-1) && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // Public methods
    //***********************************
    public void SetPlayerName(string name)
    {
        this.name = name;
    }

    // Private methods
    //***********************************
    public void Jump()
    {
        isGrounded = false;
        ++jumpCount;
        _rigidbody.AddForce(new Vector2(0f, jumpForce));
    }
}
