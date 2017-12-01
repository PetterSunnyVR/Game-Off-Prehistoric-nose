using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (collision is BoxCollider2D))
        {
            GameObject.FindObjectOfType<GameManager>().UpdateScore(1);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = false;
            Invoke("DisableComponenet", GetComponent<AudioSource>().clip.length);

        }
    }

    private void DisableComponenet()
    {
        Destroy(gameObject);
    }
}
