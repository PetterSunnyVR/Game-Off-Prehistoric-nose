using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour {

    public AudioClip[] clips;
    public GameObject groundCheck;
    bool grounded;
    bool dispensable = true;
    bool oneShot = true;
    float checkWallRadious = 0.001f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (collision is CircleCollider2D))
        {
            if (!collision.gameObject.GetComponent<Player>().CheckHasStone())
            {
                Debug.Log("Picking up rock trigger");
                collision.GetComponent<Player>().SetHasStone();
                GetComponent<AudioSource>().clip = clips[0];
                GetComponent<AudioSource>().Play();
                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("DisableComponenet", GetComponent<AudioSource>().clip.length);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<Player>().CheckHasStone())
            {
                collision.gameObject.GetComponent<Player>().SetHasStone();
                GetComponent<AudioSource>().clip = clips[0];
                GetComponent<AudioSource>().Play();
                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("DisableComponenet", GetComponent<AudioSource>().clip.length);
            }
            
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<AudioSource>().clip = clips[1];
            GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponent<Enemy>().LoseLife(2);
        }else if(collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("BOSS");
            if (oneShot)
            {
                GetComponent<AudioSource>().clip = clips[1];
                GetComponent<AudioSource>().Play();
                collision.gameObject.GetComponent<Boss1>().LoseLife(2);
                oneShot = false;
            }
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            grounded = Physics2D.OverlapCircle(groundCheck.transform.position, checkWallRadious);
            if (grounded)
            {
                gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                
                Debug.Log("Kinematic");
            }
            
        }


    }

    private void DisableComponenet()
    {
        gameObject.SetActive(false);
    }

    public void SetIfDispensable(bool value)
    {
        dispensable = value;
    }

    public bool GetIfDispensable()
    {
        return dispensable;
    }
}
