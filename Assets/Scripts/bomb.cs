using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour {

    public Rigidbody2D Player;
	// Use this for initialization
	void Start () {
        transform.position = new Vector2(Player.position.x + 1, Player.position.y + 1);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(300, 100));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
