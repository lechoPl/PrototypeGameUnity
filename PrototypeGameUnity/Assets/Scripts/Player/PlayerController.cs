using Assets.Scripts.Game;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Common;

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
    private Collider2D _collider;
    private bool isGrounded = false;
    private int jumpCount = 0;

    private float _lastMove;
    private Vector2 _lastPosition;

    // MonoBehavior methods
    //***********************************
    void Start()
    {
        GameLogic.Instance.RegisterPlayer(this);
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _lastMove = Input.GetAxis("Horizontal");
        _lastPosition = transform.position;
    }

    void OnDestroy()
    {
        GameLogic.Instance.RemovePlayer(this);
    }

    void FixedUpdate()
    {
        if (!CanPlayerMove())
        {
            return;
        }

        isGrounded = Physics2D.OverlapCircleAll(GroundCheck.position, GroundCheckRadius, GroundMask).Count(c => c != _collider) > 0;
        if (isGrounded)
        {
            _lastMove = _lastMove + 100f; // to fix bug after reset velocity;
            jumpCount = 0;
        }

        float move = Input.GetAxis("Horizontal");

        Debug.Log(string.Format("{0} - last: {1}", move, _lastMove));

        if (_lastMove != move || Mathf.Abs(_lastPosition.x - transform.position.x) > Utilities.eps)
        {
            _rigidbody.velocity = new Vector2(move * Speed, _rigidbody.velocity.y);
        }

        _lastPosition = transform.position;
        _lastMove = move;
    }

    void Update()
    {
        if (!CanPlayerMove())
        {
            return;
        }

        if ((isGrounded || jumpCount < maxJumpCount-1) && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    // Public methods
    //***********************************
    public void ResetVelocity()
    {
        if(_rigidbody == null)
        {
            return;
        }

        _rigidbody.velocity = Vector2.zero;
    }

    public void SetPlayerName(string name)
    {
        this.name = name;
    }

    // Private methods
    //***********************************
    private void Jump()
    {
        isGrounded = false;
        ++jumpCount;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
        _rigidbody.AddForce(new Vector2(0f, jumpForce));
    }

    private bool CanPlayerMove()
    {
        if (GameLogic.Instance.GetCurrentPlayer() != this)
        {
            return false;
        }

        //if (GameLogic.Instance.MovementBlocked) {
        //	return false;
        //}

        return true;
    }
}
