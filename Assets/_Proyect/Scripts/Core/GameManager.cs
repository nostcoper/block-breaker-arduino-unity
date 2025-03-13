using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int NumberOfPlayers;
    public int score = 0;
    public int lives = 3;
    public GameObject ballPrefab;
    public GameObject paddlePrefab;
    private GameObject currentBall;
    public List<GameObject> paddles = new List<GameObject>();

    void Start()
    {
        Debug.Log(NumberOfPlayers);
        SpawnPaddle();
        SpawnBall();
        StartCoroutine(EnviarVidaArduino());

        if (GameManagerUI.instance != null)
            GameManagerUI.instance.UpdateScoreUI(score);
    }

    IEnumerator EnviarVidaArduino()
    {
        yield return new WaitForSeconds(0.5f);
        SendLivesToArduino();
    }

    void SpawnPaddle()
    {
        for (int i = 0; i < NumberOfPlayers; i++){
            Debug.Log("instanciado " + i);
            GameObject newPaddle = Instantiate(paddlePrefab, new Vector3(0, -4, 0), Quaternion.identity);
            paddles.Add(newPaddle);
        }
    }

    void SpawnBall()
    {
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
        SendLivesToArduino();

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
}
