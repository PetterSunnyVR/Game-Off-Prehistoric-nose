using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour {

    Player player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //trigger falling animation
            player.IsFallingTrue();
            if(collision.GetType() == typeof(CircleCollider2D))
            {
                GameObject.FindObjectOfType<GameManager>().AddToDeathCount();
            }
            
            GameObject.FindObjectOfType<GameManager>().StopFollowingCamera();
        }else if(collision.gameObject.CompareTag("Throwable"))
        {
            Destroy(collision.gameObject);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Reset the player - GameManager
            GameObject.FindObjectOfType<GameManager>().ResetGame();
        }
        
    }

}
