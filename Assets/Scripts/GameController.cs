using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Класс сковородки
[System.Serializable]
public class PanClass
{
    // Префаб сковородки
    public GameObject panObj;   
    // Пауза до начала падения сковородок
    public float start;       
    // Пауза между сковородками  
    public float pause; 		
}

public class GameController : MonoBehaviour {

    // Объект сковородки
    public PanClass pan;
    // Точка спавна
    public Vector2 spawnValues;
    // Объект с интерфейсом результата
    public GameObject scoreText;
    // Объект с интерфейсом рестарта игры
    public GameObject restartText;
    // Объект с интерфейсом панели паузы
    public GameObject pausePanel;
    // Время между повышениями результата
    public float scoreRate = 1.0F;
    // Значение, на которое повышается результат
    public int scoreAdd = 10;   
    // Результат
    public static int score;
    // Признак завершения игры
    public static bool gameover;
    // Время до следующего результата
    private float nextScore = 0.0F;
    // Признак того, что единоразовые действия после конца игры были выполнены
    private bool gameoverStarted;

    void Start () {
        // Инициализация значений (для рестарта)
        gameover = false;
        score = 0;
        gameoverStarted = false;
        // Запустить падение сковородок
        StartCoroutine(panSpawn());
    }

    void FixedUpdate()
    {
        if (!gameover)
        {
            // Обновить результат
            scoreText.GetComponent<Text>().text = score.ToString();
        }
    }

    void Update () {
        if (gameover){
            // Действия, выполняемые только один раз после конца игры до рестарта
            if (!gameoverStarted) {
                gameoverStarted = true;
                // Отобразить интерфейс рестарта
                restartText.SetActive(true);
            }

            // Рестарт по R
            if (Input.GetKey(KeyCode.R))
            {
                // Перезапуск сцены
                SceneManager.LoadScene(0);
            }         
        } else {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale != 0) {
                    // Поставить на паузу
                    Time.timeScale = 0;
                    pausePanel.SetActive(true);
                } else {
                    // Снять с паузы
                    Time.timeScale = 1;
                    pausePanel.SetActive(false);
                }
            }
        }

        // Подсчет результата
        if (!gameover && (Time.time > nextScore))
        {
            nextScore = Time.time + scoreRate;
            score = score + scoreAdd;
        }
    }

    // Падение сковородки
    IEnumerator panSpawn()
    {
        // Пауза до начала падения сковородок
        yield return new WaitForSeconds(pan.start);

        // Бесконечный цикл, до конца игры
        while (!gameover)
        {
            // Генерировать крутящуюся сковородку в случайном месте на определенной высоте
            Vector2 spawnPosition = new Vector2(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(pan.panObj, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(pan.pause);
        }
    }
}
