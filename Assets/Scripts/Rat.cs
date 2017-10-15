using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Ограничение перемещения
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class Rat : MonoBehaviour {

    // Скорость перемещения крысюка
    public float speed;
    // Ограничение перемещения
    public Boundary boundary;
    // Максимальное число жизней
    public int maxLives = 3;
    // Объект с интерфейсом количества оставшихся жизней
    public GameObject livesText;
    // Текущее число жизней
    private int lives;

    void Start()
    {
        // Инициализация значения (для рестарта)
        lives = maxLives;
        // Обновить интерфейс для начального значения
        setLivesGUI();
    }

    void FixedUpdate()
    {
        // Расчет вектора перемещения крысюка с помощью устройства ввода с учетом заданной скорости
        var rigid = GetComponent<Rigidbody2D>();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rigid.velocity = movement * speed;

        // Обновить позицию с учетом ограничений перемещения
        rigid.position = new Vector2
            (
                Mathf.Clamp(rigid.position.x, boundary.xMin, boundary.xMax), 
                Mathf.Clamp(rigid.position.y, boundary.yMin, boundary.yMax)
            );

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // В случае, если сковородка врезалась в крысюка (или наоборот)
        if (other.tag == "Pan")
        {
            // Уничтожить сковородку
            Destroy(other.gameObject);
            // Понизить текущее число жизней
            lives--;
            // Обновить интерфейс текущего числа жизней
            setLivesGUI();
            // Проиграть файл ранения
            GetComponent<AudioSource>().Play();

            // Если не осталось жизней - конец игры
            if (lives == 0)
            {
                GameController.gameover = true;
                Destroy(gameObject);
                //gameObject.SetActive(false);
            }
        }
    }

    // Обновление интерфейса количества оставшихся жизней
    void setLivesGUI() {
        livesText.GetComponent<Text>().text = lives.ToString();
    }
}
