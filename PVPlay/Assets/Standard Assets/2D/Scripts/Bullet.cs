using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float speed=20;
	private float startX=0;

	// Use this for initialization
	void Start () {
		startX = gameObject.transform.position.x;
		if(gameObject.transform.localScale.y>0)gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed , gameObject.GetComponent<Rigidbody2D>().velocity.y);
		else gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0-speed , gameObject.GetComponent<Rigidbody2D>().velocity.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (startX + 30 < gameObject.transform.position.x)
			Destroy (gameObject);
	}
	void OnCollisionEnter2D (Collision2D col){
		Destroy(gameObject);
	}
}
