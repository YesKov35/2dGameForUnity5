using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            bool block = col.GetComponent<Animator>().GetBool("Protect");
            float s = col.transform.localScale.x;
            if(block)
            {
                if((gameObject.transform.position.x > col.transform.position.x && s > 0) || (gameObject.transform.position.x < col.transform.position.x && s < 0))
                {
                    if(gameObject.transform.position.x > col.transform.position.x)
                    {
                        col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3, 0), ForceMode2D.Impulse);
                    }
                    else
                    {
                        col.GetComponent<Rigidbody2D>().AddForce(new Vector2(3, 0), ForceMode2D.Impulse);
                    }
                    return;
                }
            }
            gameObject.SendMessageUpwards("Hit", col.transform.name);
        }
    }
}