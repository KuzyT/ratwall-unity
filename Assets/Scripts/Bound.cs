using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour {

    void OnTriggerExit2D(Collider2D other)
    {
    	// Уничтожить объект, вышедший за рамку
        Destroy(other.gameObject);
    }
}
