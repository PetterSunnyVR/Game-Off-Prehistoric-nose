using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBarScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && (collision.gameObject.GetComponent<Boss1>() == null))
        {
            collision.gameObject.SetActive(false);
        }else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Throwable") && collision.gameObject.activeSelf)
        {
            Destroy(collision.gameObject);
        }
    }
}
