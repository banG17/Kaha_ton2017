using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
public class hpbar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float hp;
			if(gameObject.name=="hpbar1")gameObject.transform.localScale=new Vector3(hp = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlatformerCharacter2D>().hp * 0.001f,gameObject.transform.localScale.y,1);
			else gameObject.transform.localScale=new Vector3(GameObject.FindGameObjectWithTag("Player1").GetComponent<PlatformerCharacter2D>().hp * 0.001f,gameObject.transform.localScale.y,1);
	}
}
}