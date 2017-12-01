using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("DestroySign", 1f);

    }
	
    public void DestroySign()
    {
        Destroy(gameObject);
    }
}
