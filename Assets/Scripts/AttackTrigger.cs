using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (gameObject.GetComponentInParent<Player>().clanName.Equals(col.GetComponent<Player>().clanName) || col.GetComponent<Player>().GetDeath())
            {
                return;
            }

            bool block = col.GetComponent<Animator>().GetBool("Protect");
            float s = col.transform.localScale.x;
			int n = 30;
			if (!block)
				n = 300;
            
			if ((gameObject.transform.position.x > col.transform.position.x && s > 0 && block) || (gameObject.transform.position.x < col.transform.position.x && s < 0 && block) || !block) 
			{
				if (gameObject.transform.position.x > col.transform.position.x) 
				{
					col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-n, 0));
					if (block)
						return;
				} 
				else 
				{
					col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (n, 0));
					if (block)
						return;
				}

            }
            //GameObject go = GameObject.Find(col.transform.name);
            //if ((go.GetComponent<Player>().hp - 10) <= 0 && col.GetComponent<Player>().hp > 0)
            //{
                //gameObject.GetComponentInParent<Player>().kill += 1;
                //gameObject.SendMessageUpwards("Kill", gameObject.GetComponentInParent<Player>().transform.name);
            //}
            object[] tempStorage = new object[2];
            tempStorage[0] = col.transform.name;
            tempStorage[1] = gameObject.GetComponentInParent<Player>().transform.name;
            gameObject.SendMessageUpwards("Hit", tempStorage);
            //col.GetComponent<Player>().CmdHit(col.transform.name, 10);
        }
    }
}