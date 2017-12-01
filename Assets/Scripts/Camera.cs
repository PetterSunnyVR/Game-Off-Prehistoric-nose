using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public Transform target, aspectRatioGuard;
    public float smoothing = 5f;
    bool following = false;
    public bool isEndOfLevel = false;
    Vector3 offsetAspectRatio;
    // Use this for initialization
    void Start () {
        target = GameObject.FindObjectOfType<Player>().transform;
        offsetAspectRatio = transform.position - aspectRatioGuard.position;

    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
		if((target.position.x >= transform.position.x)&& !isEndOfLevel)
        {
            following = true;
        }
        if((target.GetComponent<Player>().move <= 0)|| isEndOfLevel)
        {
            following = false;
        }
        if (following )
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
            aspectRatioGuard.position = transform.position - offsetAspectRatio;    
        }
        

    }

    public void ResetFollowing()
    {
        following = false;
        
    }

    public void TriggerEndLevel()
    {
        isEndOfLevel = true;
    }

    public void ResetEndLevel()
    {
        isEndOfLevel = false;
    }

    public bool GetFollowing()
    {
        return following;
    }
}
