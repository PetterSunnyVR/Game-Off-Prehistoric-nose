using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public Sprite[] numbers;
    Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeScore(int i)
    {
        image = GetComponent<Image>();
        if (i < numbers.Length)
        {
            image.sprite = numbers[i];
        }
    }
}
