using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopTrigger : MonoBehaviour
{
    public bool open = false;
    public Button button_chop;
    public GameObject Shop_panel;
    List<Items> items;
    Button Item;

    public PlayerUpdate update;

    public Text text, cost;
    

    void Start()
    {
        items = new List<Items>();
        items.Add(new Items("Item1", 400));
        items.Add(new Items(ItemStat("JUMP BOOTS", "Jump boots — увеличивают прыжок своему владельцу.\nОсновные бонусы к прыжку от нескольких пар ботинок не складываются.", "+200 ", "к прыжку"), 400));
        items.Add(new Items("Item3", 400));
        items.Add(new Items("Item4", 400));
        items.Add(new Items("Item5", 400));
        items.Add(new Items("Item6", 400));
        items.Add(new Items("Item7", 400));
        items.Add(new Items("Item8", 400));
        items.Add(new Items("Item9", 400));
    }

    string ItemStat(string name, string msg, string statCount, string stat)
    {
        return "<color=black>" + name + "</color>\n" + msg + "\n<color=blue>" + statCount + "</color>" + stat;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //проверяем на локального игрока 
        if (col.CompareTag("Player") && col.transform.name == update.transform.name)
        {
            if (!col.GetComponent<PlayerUpdate>().GetDeath())
            {
                button_chop.gameObject.SetActive(true);//активирование кнопки магазин
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.transform.name == update.transform.name)
        {
            if (!col.GetComponent<PlayerUpdate>().GetDeath())
            {
                button_chop.gameObject.SetActive(false);
                Shop_panel.SetActive(false);
            }
        }
    }

    public void ClickShopPanel()
    {
        if (!open)
        {
            open = true;
        }
        else
        {
            open = false;
        }
        Shop_panel.SetActive(open);
    }

    public void OnItemClick(Button btn)
    {
        text.text = items[int.Parse(btn.transform.name.ToString())].text;
        cost.text = items[int.Parse(btn.transform.name.ToString())].cost.ToString();
        Item = btn;
    }

    public void ClickBuy()
    {
        if (update == null)
            return;
        Item.interactable = false;
        update.player.jumpForce = update.player.jumpForce * 2;
    }
}
