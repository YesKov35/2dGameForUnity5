using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int hp = 150;

    [SyncVar]
    bool hit = false;

    public float maxSpeed = 7f;
    bool facingRight = true;
    bool spell = false;
    public Rigidbody2D Body;
    Camera menuCamera;

    bool grounded = false;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public float jumpForce;


	bool death = false;

    Animator anim;
    Camera cam;

	float timer;
	public float spawnTime;

    // Use this for initialization
    void Start ()
    {
		menuCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		transform.name = "Player" + GetComponent<NetworkIdentity>().netId.ToString();
		cam = transform.GetComponentInChildren<Camera>().GetComponent<Camera>();
		anim = GetComponent<Animator>();
		Body = GetComponent<Rigidbody2D>();
		//Text txt = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
		//txt.text = hp.ToString();
		menuCamera.enabled = false;
    }

	public bool GetDeath()
	{
		return death;
	}

    public void Respawn()
    {
        if (isLocalPlayer)
        {
            death = false;
            menuCamera.enabled = false;
            anim.SetBool("Death", false);
            Cmdqwe();
        }
    }

    [Command]
    void Cmdqwe()
    {
        hp = 100;
    }
    public void DestroyPlayer()
    {
        if (isLocalPlayer)
        {
            death = true;
            anim.SetFloat("Speed", 0);
            Body.velocity = new Vector2(0, Body.velocity.y);
            menuCamera.enabled = true;
            anim.SetBool("Death", true);
            hp = 100;
        }
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
	public void CmdHit(string id, int dmg)
    {
        GameObject go = GameObject.Find(id);
        go.GetComponent<Player>().CmdGetDamage(dmg);
    }

    void OnGUI()
    {
        if(isLocalPlayer)
        {
            GUI.Label(new Rect(Screen.width - 100, 25, 200, 50), "HP: " + hp);
        }
    }

    //Получение урона
    [Command]
    public void CmdGetDamage(int dmg)
    {
        hp -= dmg;
        hit = true;
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (hp < 0)
        {
            DestroyPlayer();
        }
        if (hit) 
		{
			gameObject.GetComponent<Animation> ().Play ("damage");
			hit = false;
		}
        if (isLocalPlayer)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
            anim.SetBool("Ground", grounded);

            anim.SetFloat("vSpeed", Body.velocity.y);

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

    void SpellEnd()
    {
		anim.SetBool("Spell", false);
    }

    void SpellStart()
    {
		anim.SetBool("Spell", true);
    }

    void Update()
	{
        if (isLocalPlayer)
        {
            if (death)
            {
                timer += Time.deltaTime;
                if (timer > spawnTime)
                {
                    Respawn();
                    timer = 0;
                }
            }
            if (!anim.GetBool("Protect") && Input.GetKeyDown(KeyCode.LeftControl) && !anim.GetBool("Spell"))
            {
				anim.SetBool("Spell", true);
                if (Body.transform.localScale.x > 0)
                {
                    Body.AddForce(new Vector2(5000, 200));
                }
                else
                {
                    Body.AddForce(new Vector2(-5000, 200));
                }
            }
            if (/*grounded &&*/ Input.GetKeyDown(KeyCode.Space))
            {
                Body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
            }

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

			if (Input.GetKeyDown(KeyCode.O))
			{
				DestroyPlayer ();
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
