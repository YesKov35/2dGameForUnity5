using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CharacterControllerScript : NetworkBehaviour
{
    public float maxSpeed = 10f;
    bool facingRight = true;
    public Rigidbody2D Body;

    bool grounded = false;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public float jumpForce = 300f;

    Animator anim;
    Camera cam;

    // Use this for initialization
    void Start ()
    {
        cam = transform.GetComponentInChildren<Camera>().GetComponent<Camera>();
        anim = GetComponent<Animator>();
        Body = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (isLocalPlayer)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

            float move = Input.GetAxis("Horizontal");

            anim.SetFloat("Speed", Mathf.Abs(move));

            Body.velocity = new Vector2(move * maxSpeed, Body.velocity.y);

            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();
        }
        else
        {
            if (cam.enabled)
            {
                cam.enabled = false;
                cam.gameObject.GetComponent<AudioListener>().enabled = false;
            }
        }

    }

    void Update()
    {
        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Body.AddForce(new Vector2(0, jumpForce));
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale; 
    }
}
