using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {
    GameObject boss;
    public bool triggered;
	// Use this for initialization
	void Start () {
        
        triggered = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            if (!triggered)
            {
                GameObject.FindObjectOfType<Camera>().TriggerEndLevel();
                GameObject.FindObjectOfType<FinishLevelTrigger>().EnableTrigger();
                boss = GameObject.FindGameObjectWithTag("Boss");
                if (boss != null)
                {
                    GameObject.FindObjectOfType<Boss1>().SetMayhame(true);
                    GetComponent<AudioSource>().Play();
                }
                
                GameObject.FindObjectOfType<Player>().SetMayhame();
                triggered = true;
            }
            
        }
    }

    public void SetTriggered(bool val)
    {
        triggered = val;
    }
}
