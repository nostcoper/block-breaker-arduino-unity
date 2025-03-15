using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UIElements;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] public int NumberOfPlayers = 1;
    public int score = 0;
    public int lives = 3;
    public GameObject ballPrefab;
    public GameObject paddlePrefab;
    private GameObject currentBall;
    public List<GameObject> paddles = new List<GameObject>();
    public KeyboardController keyboardController;

    void Start()
    {
        Debug.Log("Num Jugadores: " + NumberOfPlayers);
        SpawnPaddle();
        SpawnBall();
        StartCoroutine(EnviarVidaArduino());

        if (GameManagerUI.instance != null){
            GameManagerUI.instance.UpdateScoreUI(score);
            GameManagerUI.instance.RegisterBall(currentBall);
            GameManagerUI.instance.RegisterPaddles(paddles);
        }

    }

      IEnumerator EnviarVidaArduino()
    {
        yield return new WaitForSeconds(0.5f);
        SendLivesToArduino();
    } 

void SpawnPaddle()
{
    float screenWidth = Camera.main.orthographicSize * 2.0f * Camera.main.aspect;
    
    float paddleWidth = paddlePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
    float totalPaddlesWidth = paddleWidth * NumberOfPlayers;
    
    float usableWidth = screenWidth * 0.8f;
    float spacing = (usableWidth - totalPaddlesWidth) / (NumberOfPlayers + 1);
    
    float startX = -screenWidth / 2 + (screenWidth - usableWidth) / 2 + spacing;
    
    for (int i = 0; i < NumberOfPlayers; i++) {
        Debug.Log("Instanciando paddle " + i);
        float xPos = startX + (paddleWidth / 2) + i * (paddleWidth + spacing);
        GameObject newPaddle = Instantiate(paddlePrefab, new Vector3(xPos, -3.5f, 0), Quaternion.identity);
        newPaddle.GetComponent<PaddleController>().SetController(keyboardController);
        paddles.Add(newPaddle);
    }
}

    void SpawnBall()
    {
        Debug.Log(ballPrefab);
        if (currentBall == null)
        {
            currentBall = Instantiate(ballPrefab, new Vector3(0, -3.5f, 0), Quaternion.identity);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        if (GameManagerUI.instance != null)
            GameManagerUI.instance.UpdateScoreUI(score);
    }

    public void LoseLife()
    {
        lives--;
        //SendLivesToArduino();

        if (lives > 0)
        {
                if (currentBall != null)
                currentBall.GetComponent<BallController>().ResetBall();
        }
        else
        {
            if (GameManagerUI.instance != null)
                GameManagerUI.instance.ShowGameOver(score);
        }
    }

     void SendLivesToArduino()
    {
        if (ArduinoControllerPot.Instance != null)
            ArduinoControllerPot.Instance.SendToArduino("L:" + lives);
    } 

    public void RedirectScene(String name){
        SceneManager.LoadScene(name);
    }
}


