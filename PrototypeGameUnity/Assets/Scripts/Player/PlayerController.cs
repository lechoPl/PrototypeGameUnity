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
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundMask);
        if (isGrounded)
        {
            jumpCount = 0;
        }


        float move = Input.GetAxis("Horizontal");
        //float jump = Input.GetKey("Jump");

        _rigidbody.velocity = new Vector2(move * Speed, _rigidbody.velocity.y);
    }

    void Update()
    {
        if ((isGrounded || jumpCount < maxJumpCount-1) && Input.GetButtonDown("Jump"))
        {
            isGrounded = false;
            ++jumpCount;
            _rigidbody.AddForce(new Vector2(0f, jumpForce));
        }
    }
}
