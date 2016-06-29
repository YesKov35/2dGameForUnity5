using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopTrigger : MonoBehaviour {

    public string clan;
    public bool open = false;
    public Button button_chop;
    public GameObject Shop_panel;
    List<Items> items;
    Button Item;

    public Player player;

    public Text text, cost;
    

    void Start()
    {
        items = new List<Items>();
        items.Add(new Items("Item1", 400));
        items.Add(new Items("Item2", 400));
        items.Add(new Items("Item3", 400));
        items.Add(new Items("Item4", 400));
        items.Add(new Items("Item5", 400));
        items.Add(new Items("Item6", 400));
        items.Add(new Items("Item7", 400));
        items.Add(new Items("Item8", 400));
        items.Add(new Items("Item9", 400));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.transform.name == player.transform.name)
        {
            if (clan.Equals(col.GetComponent<Player>().clanName) || !col.GetComponent<Player>().GetDeath())
            {
                button_chop.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.transform.name == player.transform.name)
        {
            if (clan.Equals(col.GetComponent<Player>().clanName) || !col.GetComponent<Player>().GetDeath())
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
        if (player == null)
            return;
        Item.interactable = false;
        player.jumpForce = player.jumpForce * 2;
    }
}
