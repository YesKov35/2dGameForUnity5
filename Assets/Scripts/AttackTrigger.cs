using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
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
            gameObject.SendMessageUpwards("Hit", col.transform.name);
            //col.GetComponent<Player>().CmdHit(col.transform.name, 10);
        }
    }
}