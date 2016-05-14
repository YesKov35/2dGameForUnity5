using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int hp = 100;

    [SyncVar]
    bool hit = false;

    public float maxSpeed = 7f;
    bool facingRight = true;
    public Rigidbody2D Body;

    bool grounded = false;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public float jumpForce = 500f;

    Animator anim;
    Camera cam;

    // Use this for initialization
    void Start ()
    {
        transform.name = "Player" + GetComponent<NetworkIdentity>().netId.ToString();
        cam = transform.GetComponentInChildren<Camera>().GetComponent<Camera>();
        anim = GetComponent<Animator>();
        Body = GetComponent<Rigidbody2D>();
        //Text txt = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        //txt.text = hp.ToString();
    }
    [Client]
    public void Hit(string id)
    {
        if (isLocalPlayer)
        {
            CmdHit(id, 10);
        }
    }

    [Command]
	void CmdHit(string id, int dmg)
    {
        GameObject go = GameObject.Find(id);
        go.GetComponent<Player>().GetDamage(dmg);
    }

    void OnGUI()
    {
        if(isLocalPlayer)
        {
            GUI.Label(new Rect(Screen.width - 100, 25, 200, 50), "HP: " + hp);
        }
    }

    //Получение урона
    public void GetDamage(int dmg)
    {
        hp -= dmg;
        hit = true;
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (hit)
            {
                gameObject.GetComponent<Animation>().Play("damage");
                hit = false;
            }
        if (isLocalPlayer)
        {
            //grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
            float move = Input.GetAxis("Horizontal");

            if (!anim.GetBool("Protect"))
            {
                anim.SetFloat("Speed", Mathf.Abs(Body.velocity.x));

                Body.velocity = new Vector2(move * maxSpeed, Body.velocity.y);
            }
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
                maxSpeed = 3f;
                
            }
            else if (anim.GetBool("Keydown") && Input.GetKeyUp(KeyCode.Mouse0))
            {
                anim.SetBool("Keydown", false);
                maxSpeed = 7f;
            }

            if (/*!anim.GetBool("Protect") &&*/ Input.GetKeyDown(KeyCode.Mouse1))
            {
                anim.SetBool("Protect", !anim.GetBool("Protect"));
                anim.SetFloat("Speed", 0);
                Body.velocity = new Vector2(0, Body.velocity.y);

            }
            /*if (anim.GetBool("Protect") && Input.GetKeyUp(KeyCode.Mouse1))
            {
                anim.SetBool("Protect", false);
            }*/
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
