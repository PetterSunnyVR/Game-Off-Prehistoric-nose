using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector3 parent;
    public float distance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(transform.position.x-parent.x)> distance)
        {
            Destroy(gameObject);
        }
	}

    public void SetShooter(Vector3 pos)
    {
        parent = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetHit();
            Destroy(gameObject);
        }
    }
}
