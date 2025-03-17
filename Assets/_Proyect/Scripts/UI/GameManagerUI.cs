using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//PRUEBA
using System.Collections.Generic;
//

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI instance;

    [Header("Elementos de UI")]
    public TextMeshProUGUI scoreText;           // Asigna desde el Inspector el Text que muestra el puntaje
    public GameObject gameOverPanel; // Panel que se activa al Game Over
    public TextMeshProUGUI gameOverScoreText;   // Texto para mostrar el puntaje final en Game Over

    public Button gameOverExit; //Boton para salir en game over

    //PRUEBA

    [Header("Referencias de Juego")]
    private GameObject ball;
    public List<GameObject> paddles = new List<GameObject>();
    private List<GameObject> bricks;
    //

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start(){
        gameOverExit.onClick.AddListener(OnExitButton);
    }

    // Actualiza el texto del puntaje
    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
            scoreText.text = "SCORE \n" + score;
    }

    //Destruir los ladrillos restantes
    private void DestroyAllBricks()
    {
        foreach (GameObject brick in bricks)
        {
            if (brick != null)
            {
                Destroy(brick);
            }
        }
        bricks.Clear();
    }

    //REGISTRAR PADDLES , BOLA y LADRILLOS
    public void RegisterBall(GameObject ballInstance)
    {
        ball = ballInstance;
    }

    public void RegisterPaddles(List<GameObject> paddleInstances)
    {
        paddles = paddleInstances;
    }
    public void RegisterBricks(List<GameObject> brickInstances)
    {
        bricks = brickInstances;
    }


    // Muestra el panel de Game Over y actualiza el puntaje final
    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverScoreText != null)
                gameOverScoreText.text = "FINAL SCORE \n" + finalScore;


            //PRUEBA

            // Ocultar la pelota
            if (ball != null)
            {
                ball.SetActive(false);
            }

            // Ocultar los paddles
            if (paddles != null)
            {
                foreach (GameObject paddle in paddles)
                {
                    if (paddle != null)
                    {
                        paddle.SetActive(false);
                    }
                }
            }
            // Ocultar el score durante el juego
            if (scoreText != null)
            {
                scoreText.gameObject.SetActive(false);
            }
            DestroyAllBricks();
        }
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.SpawnElements();
    }

    public void OnExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
