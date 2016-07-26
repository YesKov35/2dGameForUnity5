using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerUpdate : NetworkBehaviour
{

    public Player player;

    [SyncVar]
    public int hp;

    [SyncVar]
    string namehodamage;

    [SyncVar]
    public int kill = 0;

    int cur_kill = 0;
    int count_death;

    int count = 0;

    [SyncVar]
    bool hit = false;

    public Rigidbody2D Body;
    Camera menuCamera;

    bool grounded = false;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    float groundRadius = 0.2f;

    [SyncVar]
    public string clanName;

    [SyncVar]
	bool death = false;

    Animator anim;
    Camera cam;

	float timer;
	public float spawnTime;

    Menu_HUD menu;

    public bool chat = false;

    void Start ()
    {
		menuCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		transform.name = "Player" + GetComponent<NetworkIdentity>().netId.ToString();
		cam = transform.GetComponentInChildren<Camera>().GetComponent<Camera>();
		anim = GetComponent<Animator>();
		Body = GetComponent<Rigidbody2D>();
        player.speed = player.normalSpeed;

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
        {
            menu.update = this;
            GameObject.FindWithTag("Shop").GetComponent<ShopTrigger>().update = this;
            Parallaxing p = GameObject.FindWithTag("Parallax").GetComponent<Parallaxing>();
            
            p.player = cam;
            p.clanName = clanName;
            p.enabled = true;
        }


    }

	public bool GetDeath()
	{
		return death;
	}

    public string GetCountDeath()
    {
        return count_death.ToString();
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
    void CmdHP(int helth)
    {
        hp = helth;
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
            count_death++;
            CmdSendKill();
        }
    }

    [Client]
    public void Hit(object[] id)
    {
        if (isLocalPlayer)
        {
            CmdHit(id[0].ToString(), id[1].ToString(), int.Parse(id[2].ToString()));
        }
    }

    [Command]
	public void CmdHit(string id, string id2, int dmg)
    {
        GameObject go = GameObject.Find(id);
        go.GetComponent<PlayerUpdate>().CmdGetDamage(id2, dmg);
    }

    [Command]
    public void CmdKill(string id)
    {
        GameObject go = GameObject.Find(id);
        go.GetComponent<PlayerUpdate>().CmdGetKill();
    }

    [Command]
    public void CmdGetKill()
    {
        kill += 1;
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

    void FixedUpdate()
    {
        if (hit)
        {
            gameObject.GetComponent<Animation>().Play("damage");
            hit = false;
        }
        if (isLocalPlayer)
        {
            if (death)
                return;
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
            anim.SetBool("Ground", grounded);

            anim.SetFloat("vSpeed", Body.velocity.y);

            float move = Input.GetAxis("Horizontal");

            if (!anim.GetBool("Protect") || death)
            {
                anim.SetFloat("Speed", Mathf.Abs(Body.velocity.x));

                Body.velocity = new Vector2(move * player.speed, Body.velocity.y);
            }
            if (move > 0 && !player.facingRight)
                Flip();
            else if (move < 0 && player.facingRight)
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
            menu.Clock.text = ((int)Time.timeSinceLevelLoad).ToString();
            if (hp <= 0 && !death)
            {
                timer = 0;
                DestroyPlayer();
                CmdHP(player.max_hp);
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
            if (player.speed == player.runSpeed && player.stamina == 0)
            {
                player.speed = player.normalSpeed;
                anim.speed = 1;
            }
            if (count < (int)Time.timeSinceLevelLoad)
            {
                int n = (int)Time.timeSinceLevelLoad - count;

                if ((player.stamina + player.respStamina) <= player.max_stamina && player.speed < player.runSpeed && !anim.GetBool("Protect") && !anim.GetBool("Keydown"))
                    player.stamina += player.respStamina;
                else if ((player.speed == player.runSpeed  || anim.GetBool("Keydown")) && player.stamina > 0)
                    player.stamina -= 25;
                count = (int)Time.timeSinceLevelLoad;

                if ((player.mana + player.respMana) <= player.max_mana)
                    player.mana += player.respMana;
            }
            if (cur_kill < kill)
            {
                player.exp += (kill - cur_kill) * 50;
                player.gold += (kill - cur_kill) * 1000;
                cur_kill = kill;
            }
            if (!anim.GetBool("Protect") && Input.GetKeyDown(KeyCode.LeftControl) && !anim.GetBool("Spell") && player.mana >= 100)
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
                player.mana -= 100;
            }
            if (grounded && Input.GetKeyDown(KeyCode.Space) && !chat)
            {
                Body.AddForce(new Vector2(0, player.jumpForce), ForceMode2D.Force);
            }

            if (!anim.GetBool("Keydown") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetBool("Keydown", true);
                anim.speed = 1;
                player.speed = player.normalSpeed / 2;
                
            }
            else if (anim.GetBool("Keydown") && Input.GetKeyUp(KeyCode.Mouse0))
            {
                anim.SetBool("Keydown", false);
                player.speed = player.normalSpeed;
            }

            if (!anim.GetBool("Keydown") && Input.GetKeyDown(KeyCode.LeftShift))
            {
                player.speed = player.runSpeed;
                anim.speed = 1.5f;

            }
            else if (!anim.GetBool("Keydown") && Input.GetKeyUp(KeyCode.LeftShift))
            {
                player.speed = player.normalSpeed;
                anim.speed = 1;
            }

            if (/*!anim.GetBool("Protect") &&*/Input.GetKeyDown(KeyCode.Mouse1))
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
        player.facingRight = !player.facingRight;
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
        menu.Chat.text += "<color=" + color + ">" + name + "</color> : " + msg + "\n";
        menu.newMsg();
    }

    [Command]
    public void CmdSendKill()
    {
        RpcSendKill(clanName);
    }

    [ClientRpc]
    public void RpcSendKill(string clan)
    {
        if(clan == "red")
        {
            menu.CountKillsGreen.text = (int.Parse(menu.CountKillsGreen.text) + 1).ToString();
        }
        else
        {
            menu.CountKillsRed.text = (int.Parse(menu.CountKillsRed.text) + 1).ToString();
        }
    }
}
