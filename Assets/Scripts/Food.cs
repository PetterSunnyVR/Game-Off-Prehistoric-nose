using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AudioSource>().Play();
            
            GameManager.instance.AddLife();
            Invoke("DisableComponenet", GetComponent<AudioSource>().clip.length);

        }

    }

    private void DisableComponenet()
    {
        Destroy(gameObject);
    }
}
