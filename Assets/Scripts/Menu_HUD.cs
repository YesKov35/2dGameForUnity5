using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu_HUD : MonoBehaviour
{

    public Player player;
    public Text text_hp, text_mp, text_sm;
    int hp, mp, sm;

    public Text Chat;
    public Image ChatBag;
    public Text NameHoDamage;
    public InputField Msg;
    public Text CountKillsRed, CountKillsGreen;
    public Text Clock;
    public Scrollbar sb_hp, sb_mp, sb_sm, sb_exp;
    public Text tx_hp, tx_mp, tx_sm, tx_lvl;
    public Text kills, death;
    float timer = 0;

    public GameObject stat_panel;
    public Text playerStat;

    // Update is called once per frame
    void Update()
    {
        if (!player.GetDeath())
        {
            sb_sm.value = sb_mp.value = sb_hp.value = 0;
            sb_hp.size = (float)player.hp / player.max_hp;
            sb_mp.size = (float)player.mana / player.max_mana;
            sb_sm.size = (float)player.stamina / player.max_stamina;
            sb_exp.size = (float)player.exp / player.max_exp;
            tx_hp.text = player.hp + "/" + player.max_hp;
            tx_mp.text = player.mana + "/" + player.max_mana;
            tx_sm.text = player.stamina + "/" + player.max_stamina;
            tx_lvl.text = "level." + player.level;

            kills.text = player.kill.ToString();
            death.text = player.GetCountDeath();

            if (sb_exp.size == 1)
            {
                player.level++;
                player.exp = 0;
            }
        }
        if (!Msg.gameObject.active && Chat.gameObject.active)
        {
            timer += Time.deltaTime;
            if (timer > 6)
            {
                Chat.gameObject.active = false;
                ChatBag.gameObject.active = false;
                timer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Msg.gameObject.active)
            {
                if (Msg.text != "")
                    player.CmdSendMessage(player.transform.name, Msg.text, player.clanName);
                Msg.gameObject.active = false;
                player.chat = false;
            }
            else
            {
                Msg.gameObject.active = true;
                player.chat = true;
                Msg.text = "";
            }
            Chat.gameObject.active = true;
            ChatBag.gameObject.active = true;
        }
    }

    public void LevelUpHP()
    {
        hp = int.Parse(text_hp.text) + 1;
        text_hp.text = hp.ToString();
        player.max_hp += 50;
    }

    public void LevelUpMP()
    {
        mp = int.Parse(text_mp.text) + 1;
        text_mp.text = mp.ToString();
        player.max_mana += 50;
    }

    public void LevelUpSM()
    {
        sm = int.Parse(text_sm.text) + 1;
        text_sm.text = sm.ToString();
        player.max_stamina += 50;
    }

    public void newMsg()
    {
        Chat.gameObject.active = true;
        ChatBag.gameObject.active = true;
        timer = 0;
    }

    public void Name(string s)
    {
        NameHoDamage.text = s;
    }

    public void OpenMsg()
    {
        if (Msg.gameObject.active)
        {
            Msg.gameObject.active = false;
            player.chat = false;
        }
        else
        {
            Msg.gameObject.active = true;
            player.chat = true;
        }
    }

    public void PlayerStat()
    {
        if (stat_panel.active)
        {
            stat_panel.SetActive(false);
        }
        else
        {
            playerStat.text = player.max_hp + "\n" + player.max_mana + "\n" + player.max_stamina +
                 "\n" + player.jumpForce;
            stat_panel.SetActive(true);
        }
    }

    public void GetExp()
    {
        player.exp += 50;
    }
}
