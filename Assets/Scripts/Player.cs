using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int hp;

    public int stamina;
    public int max_stamina;
    public int exp;

    //Test1
    [SyncVar]
    string namehodamage;

    [SyncVar]
    public int kill = 0;

    int cur_kill = 0;

    int count = 0;

    [SyncVar]
    bool hit = false;

    float Speed;
    public float normalSpeed;
    public float runSpeed;
    bool facingRight = true;
    bool spell = false;
    public Rigidbody2D Body;
    Camera menuCamera;

    bool grounded = false;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public float jumpForce;

    [SyncVar]
    public string clanName;

    [SyncVar]
	bool death = false;

    Animator anim;
    Camera cam;

	float timer;
	public float spawnTime;

    Text up_hp;
    Text up_sm;
    Text up_mp;

    Menu_HUD menu;

    // Use this for initialization
    void Start ()
    {
		menuCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		transform.name = "Player" + GetComponent<NetworkIdentity>().netId.ToString();
		cam = transform.GetComponentInChildren<Camera>().GetComponent<Camera>();
		anim = GetComponent<Animator>();
		Body = GetComponent<Rigidbody2D>();
        Speed = normalSpeed;

        menu = GameObject.FindWithTag("menu").GetComponent<Menu_HUD>();

        menuCamera.enabled = false;
        if (clanName == "red")
        {
            Body.transform.position = new Vector3(60, 0, 0);
        }
        else
        {
            Body.transform.position = new Vector3(0, 0, 0);
        }

        if (isLocalPlayer)
            menu.player = this;
    }

	public bool GetDeath()
	{
		return death;
	}

    public void Respawn()
    {
        if (isLocalPlayer && death)
        {
            CmdSetDeath(false);
            menuCamera.enabled = false;
            anim.SetBool("Death", false);
            if (clanName == "red")
            {
                Body.transform.position = new Vector3(60, 0, 0);
            }
            else
            {
                Body.transform.position = new Vector3(0, 0, 0);
            }

        }
    }

    [Command]
    void CmdSetDeath(bool flag)
    {
        death = flag;
    }

    [Command]
    void CmdHP()
    {
        hp += 100;
    }

    public void DestroyPlayer()
    {
        if (isLocalPlayer && !death)
        {
            menuCamera.enabled = true;
            anim.SetBool("Death", true);
            death = true;
            CmdSetDeath(death);
            CmdKill(namehodamage);
        }
    }

    [Client]
    public void Hit(object[] id)
    {
        if (isLocalPlayer)
        {
            CmdHit(id[0].ToString(), id[1].ToString(), 10);
        }
    }

    [Command]
	public void CmdHit(string id, string id2, int dmg)
    {
        GameObject go = GameObject.Find(id);
        go.GetComponent<Player>().CmdGetDamage(id2, dmg);
    }

    [Client]
    public void Kill(string id)
    {
        if (isLocalPlayer)
        {
            //CmdKill(id);
        }
    }

    [Command]
    public void CmdKill(string id)
    {
        GameObject go = GameObject.Find(id);
        go.GetComponent<Player>().CmdGetKill();
    }

    [Command]
    public void CmdGetKill()
    {
        kill += 1;
    }

    void OnGUI()
    {
        if (isLocalPlayer)
        {
            GUI.Label(new Rect(Screen.width - 100, 25, 200, 50), "HP: " + hp);
            GUI.Label(new Rect(Screen.width - 100, 75, 200, 50), "SM: " + stamina);
            GUI.Label(new Rect(Screen.width - 100, 125, 200, 50), "EXP: " + exp);
            GUI.Label(new Rect(100, 25, 200, 50), "KILL: " + kill);
            GUI.Label(new Rect(Screen.width/2, 25, 200, 50), ((int)Time.timeSinceLevelLoad).ToString());
            if (death)
            {
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 500, 150), ((int)timer).ToString());
            }
        }
    }

    //Получение урона
    [Command]
    public void CmdGetDamage(string id, int dmg)
    {
        if (!death)
        {
            namehodamage = id;
            hp -= dmg;
            hit = true;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //if (death)
           // return;
        if (hit)
        {
            gameObject.GetComponent<Animation>().Play("damage");
            hit = false;
        }
        if (isLocalPlayer)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
            anim.SetBool("Ground", grounded);

            anim.SetFloat("vSpeed", Body.velocity.y);

            float move = Input.GetAxis("Horizontal");

            if (!anim.GetBool("Protect") || death)
            {
                anim.SetFloat("Speed", Mathf.Abs(Body.velocity.x));

                Body.velocity = new Vector2(move * Speed, Body.velocity.y);
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

    void Update()
	{
        if (isLocalPlayer)
        {
            menu.Name(namehodamage);
            if (hp <= 0 && !death)
            {
                timer = 0;
                DestroyPlayer();
                CmdHP();
            }
            else if (death)
            {
                timer += Time.deltaTime;
                if (timer > spawnTime)
                {
                    Respawn();
                }
                return;
            }
            if (Speed == runSpeed && stamina == 0)
            {
                Speed = normalSpeed;
                anim.speed = 1;
            }
            if (count < (int)Time.timeSinceLevelLoad)
            {
                int n = (int)Time.timeSinceLevelLoad - count;

                if ((stamina + 10) <= max_stamina && Speed < runSpeed && !anim.GetBool("Keydown"))
                    stamina += 10;
                else if (Speed == runSpeed && stamina > 0)
                    stamina -= 10;
                count = (int)Time.timeSinceLevelLoad;
            }
            if (cur_kill < kill)
            {
                exp += (kill - cur_kill) * 20;
                cur_kill = kill;
            }
            if (!anim.GetBool("Protect") && Input.GetKeyDown(KeyCode.LeftControl) && !anim.GetBool("Spell") && stamina >= 50)
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
                stamina -= 50;
            }
            if (/*grounded &&*/ Input.GetKeyDown(KeyCode.Space))
            {
                Body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
            }

            if (!anim.GetBool("Keydown") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetBool("Keydown", true);
                anim.speed = 1;
                Speed = normalSpeed / 2;
                
            }
            else if (anim.GetBool("Keydown") && Input.GetKeyUp(KeyCode.Mouse0))
            {
                anim.SetBool("Keydown", false);
                Speed = normalSpeed;
            }

            if (!anim.GetBool("Keydown") && Input.GetKeyDown(KeyCode.LeftShift))
            {
                Speed = runSpeed;
                anim.speed = 1.5f;

            }
            else if (!anim.GetBool("Keydown") && Input.GetKeyUp(KeyCode.LeftShift))
            {
                Speed = normalSpeed;
                anim.speed = 1;
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

    [Command]
    public void CmdSendMessage(string name, string msg, string color)
    {
        RpcSendMessage(name, msg, color);
        menu.newMsg();
    }

    [ClientRpc]
    public void RpcSendMessage(string name, string msg, string color)
    {
        menu.chat.text += "<color=" + color + ">" + name + "</color> : " + msg + "\n";
        menu.newMsg();
    }
}
