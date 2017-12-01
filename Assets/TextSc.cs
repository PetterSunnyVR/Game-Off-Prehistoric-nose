using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSc : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int score = GameManager.score;
        GetComponent<Text>().text = "Your score is: " + score;
	}
	

}
