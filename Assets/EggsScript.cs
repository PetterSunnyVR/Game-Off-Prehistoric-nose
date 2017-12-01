using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsScript : MonoBehaviour {
    bool wasTriggered = false;
    public Sprite brokenEggs;
    public GameObject healthPack;
    protected GameObject disposableParent;

    private void Start()
    {
        disposableParent = GameObject.Find("Disposable");
        if (disposableParent == null)
        {
            disposableParent = new GameObject("Disposable");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Throwable")|| collision.CompareTag("Weapon"))
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = brokenEggs;
            GameObject hpack = Instantiate(healthPack, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            hpack.transform.parent = disposableParent.transform;
            GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
