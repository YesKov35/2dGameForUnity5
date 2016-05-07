using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            bool block = col.GetComponent<Animator>().GetBool("Protect");
            //bool fr = col.GetComponent<Player>().GetFacingRight();
            float s = col.transform.localScale.x;
            if(block)
            {
                if((gameObject.transform.position.x > col.transform.position.x && s > 0) || (gameObject.transform.position.x < col.transform.position.x && s < 0))
                {
                    return;
                }
            }
            gameObject.SendMessageUpwards("Hit", col.transform.name);
        }
    }
}