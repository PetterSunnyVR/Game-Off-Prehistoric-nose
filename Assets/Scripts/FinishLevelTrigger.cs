using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevelTrigger : MonoBehaviour {

    bool isTrigger = false;
    GameObject Boss;
	// Use this for initialization
	void Start () {
        Boss = GameObject.FindGameObjectWithTag("Boss");
	}
	
	// Update is called once per frame
	void Update () {
        if (Boss == null)
        {
            Boss = GameObject.FindGameObjectWithTag("Boss");
            if(Boss == null)
            {
                GameObject.FindObjectOfType<FinishLevelTrigger>().GetComponent<BoxCollider2D>().isTrigger = true;
            }
            
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                SceneManager.LoadScene(2);
            }else
            {
                SceneManager.LoadScene(3);
            }
            
        }
    }


    public void EnableTrigger()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        isTrigger = true;
    }

    public bool GetTriggered()
    {
        return isTrigger;
    }
}
