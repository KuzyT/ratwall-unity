using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour {

    // Максимальное и минимальное значение угловой скорости, скорость падения сковородки
    public float maxTumble, minTumble, speed;

    void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        // Скорость вращения и падения
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(minTumble, maxTumble);
        GetComponent<Rigidbody2D>().velocity = -1 * GetComponent<Transform>().up * speed;
    }
}
