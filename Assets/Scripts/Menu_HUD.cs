using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu_HUD : MonoBehaviour
{

    public PlayerUpdate update;
    public Text text_hp, text_mp, text_sm;
    int hp, mp, sm;

    public Text Chat;
    public Image ChatBag;
    public Text NameHoDamage;
    public InputField Msg;
    public Text CountKillsRed, CountKillsGreen;
    public Text Clock;
    public Scrollbar sb_hp, sb_mp, sb_sm, sb_exp;
    public Text tx_hp, tx_mp, tx_sm, tx_lvl, tx_gold;
    public Text kills, death, countUp;
    float timer = 0;

    public GameObject stat_panel;
    public Text playerStat;

    // Update is called once per frame
    void Update()
    {
        if (!update.GetDeath())
        {
            sb_sm.value = sb_mp.value = sb_hp.value = 0;
            sb_hp.size = (float)update.hp / update.player.max_hp;
            sb_mp.size = (float)update.player.mana / update.player.max_mana;
            sb_sm.size = (float)update.player.stamina / update.player.max_stamina;
            sb_exp.size = (float)update.player.exp / update.player.max_exp;
            tx_hp.text = update.hp + "/" + update.player.max_hp;
            tx_mp.text = update.player.mana + "/" + update.player.max_mana;
            tx_sm.text = update.player.stamina + "/" + update.player.max_stamina;
            tx_lvl.text = "level." + update.player.level;
            countUp.text = update.player.countUp.ToString();

            kills.text = update.kill.ToString();
            death.text = update.GetCountDeath();

            if (sb_exp.size == 1)
            {
                update.player.level++;
                update.player.exp = 0;
                update.player.countUp += 2;
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
                    update.CmdSendMessage(update.transform.name, Msg.text, update.clanName);
                Msg.gameObject.active = false;
                update.chat = false;
            }
            else
            {
                Msg.gameObject.active = true;
                update.chat = true;
                Msg.text = "";
            }
            Chat.gameObject.active = true;
            ChatBag.gameObject.active = true;
        }
    }

    public void LevelUpHP()
    {
        if (update.player.countUp > 0)
        {
            hp = int.Parse(text_hp.text) + 1;
            text_hp.text = hp.ToString();
            update.player.max_hp += 50;
            update.player.countUp--;
        }
    }

    public void LevelUpMP()
    {
        if (update.player.countUp > 0)
        {
            mp = int.Parse(text_mp.text) + 1;
            text_mp.text = mp.ToString();
            update.player.max_mana += 50;
            update.player.countUp--;
        }
    }

    public void LevelUpSM()
    {
        if (update.player.countUp > 0)
        {
            sm = int.Parse(text_sm.text) + 1;
            text_sm.text = sm.ToString();
            update.player.max_stamina += 50;
            update.player.countUp--;
        }
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
            update.chat = false;
        }
        else
        {
            Msg.gameObject.active = true;
            update.chat = true;
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
            playerStat.text = update.player.max_hp + "\n" + update.player.max_mana + "\n" + update.player.max_stamina +
                 "\n" + update.player.jumpForce;
            tx_gold.text = update.player.gold.ToString();
            stat_panel.SetActive(true);
        }
    }

    public void GetExp()
    {
        update.player.exp += 50;
    }
}
