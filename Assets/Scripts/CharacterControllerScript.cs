using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CharacterControllerScript : NetworkBehaviour
{
    public float maxSpeed = 7f;
    bool facingRight = true;
    public Rigidbody2D Body;

    bool grounded = false;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public float jumpForce = 500f;

    public float hp = 100;

    Animator anim;
    Camera cam;

    // Use this for initialization
    void Start ()
    {
        cam = transform.GetComponentInChildren<Camera>().GetComponent<Camera>();
        anim = GetComponent<Animator>();
        Body = GetComponent<Rigidbody2D>();

        Text txt = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        txt.text = hp.ToString();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (isLocalPlayer)
        {
            //grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

            float move = Input.GetAxis("Horizontal");

            anim.SetFloat("Speed", Mathf.Abs(Body.velocity.x));

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
        if(/*grounded &&*/ Input.GetKeyDown(KeyCode.Space))
        {
            Body.AddForce(new Vector2(0, jumpForce));
        }
        if (isLocalPlayer)
        {
            if (!anim.GetBool("Keydown") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetBool("Keydown", true);
                
            }
            if (anim.GetBool("Keydown") && Input.GetKeyUp(KeyCode.Mouse0))
            {
                anim.SetBool("Keydown", false);
            }

            if(Input.GetKeyUp(KeyCode.F))
            {
                Damage();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale; 
    }

    public void Damage()
    {
        gameObject.GetComponent<Animation>().Play("damage");
    }
}
