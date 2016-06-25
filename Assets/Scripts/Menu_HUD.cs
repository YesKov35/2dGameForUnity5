using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu_HUD : MonoBehaviour {

    public Player player; 
    public Text text_hp, text_mp, text_sm;
    int hp, mp, sm;

    public Text chat;
    public Text name;
    public InputField msg;
    float timer = 0;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!msg.gameObject.active && chat.gameObject.active)
        {
            timer += Time.deltaTime;
            if(timer > 6)
            {
                chat.gameObject.active = false;
                timer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(msg.gameObject.active)
            {
                if (msg.text != "")
                    player.CmdSendMessage(player.transform.name, msg.text, player.clanName);
                msg.gameObject.active = false;
            }
            else
            {
                msg.gameObject.active = true;
                msg.text = "";
            }
            chat.gameObject.active = true;
        }
    }

    public void LevelUpHP()
    {
        hp = int.Parse(text_hp.text) + 1;
        text_hp.text = hp.ToString();
    }

    public void LevelUpMP()
    {
        mp = int.Parse(text_mp.text) + 1;
        text_mp.text = mp.ToString();
    }

    public void LevelUpSM()
    {
        sm = int.Parse(text_sm.text) + 1;
        text_sm.text = sm.ToString();
        player.stamina += 50;
    }

    public void newMsg()
    {
        chat.gameObject.active = true;
    }

    public void Name(string s)
    {
        name.text = s;
    }
}
