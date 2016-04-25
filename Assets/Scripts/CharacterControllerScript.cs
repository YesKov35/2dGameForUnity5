using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CharacterControllerScript : NetworkBehaviour
{
    public float maxSpeed = 10f;
    bool facingRight = true;
    public Rigidbody2D Body;

    Animator anim;
    public Camera cam;

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

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale; 
    }
}
