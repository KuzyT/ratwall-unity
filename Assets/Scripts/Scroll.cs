using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
	// Скорость "лазания" по стене (на деле - двигается стена)
    public float speed = 0.5f;

    // Каждый фрейм сдвигаем стену
    void Update()
    {
    	if (!GameController.gameover) {
	        Vector2 offset = new Vector2(0, Time.time * speed);

	        GetComponent<Renderer>().material.mainTextureOffset = offset;    		
    	}
    }
}
