using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


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
    public ArduinoController arduinoController;
    public GameManagerUI GameManagerUI;

    void OnEnable(){
        ArduinoConection.Instance.IsConnected();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        score = 0;
        lives = 3;
        SendLivesToArduino();
        SpawnElements();
        GetComponent<BrickSpawner>().currentHitpoint = 1;
        GetComponent<BrickSpawner>().currentProgression = 0;
        GetComponent<BrickSpawner>().SpawnBricks();

        if (GameManagerUI.instance != null){
            GameManagerUI.instance.UpdateScoreUI(score);
            GameManagerUI.instance.RegisterBall(currentBall);
            GameManagerUI.instance.RegisterPaddles(paddles);
        }
    }

    public void CheckAllBlocksDestroyed()
    {
        List<GameObject> remainingBlocks =  GetComponent<BrickSpawner>().bricks;
        Debug.Log(remainingBlocks.Count);
        if (remainingBlocks.Count == 0)
        {
            GetComponent<BrickSpawner>().IncreaseProgression();
            GetComponent<BrickSpawner>().SpawnBricks();
        }
    }

    public void removeBrick(GameObject gameObject){
        GetComponent<BrickSpawner>().bricks.Remove(gameObject);
    }

    
    void Start()
    {
        Debug.Log("Num Jugadores: " + NumberOfPlayers);
        SpawnElements();
        StartCoroutine(EnviarVidaArduino());

        if (GameManagerUI.instance != null){
            GameManagerUI.instance.UpdateScoreUI(score);
            GameManagerUI.instance.RegisterBall(currentBall);
            GameManagerUI.instance.RegisterPaddles(paddles);
        }

    }

        void Update(){
            ManageControllers();
        }

        public void SpawnElements(){
            SpawnPaddle();
            SpawnBall();
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
        float usableWidth = screenWidth * 0.8f;
        
        int paddlesToCreate = ArduinoConection.Instance.IsConnected() ? NumberOfPlayers : 1;
        
        DestroyExistingPaddles();
        
        float totalPaddlesWidth = paddleWidth * paddlesToCreate;
        float spacing = (usableWidth - totalPaddlesWidth) / (paddlesToCreate + 1);
        float startX = -screenWidth / 2 + (screenWidth - usableWidth) / 2 + spacing;
        
        for (int i = 0; i < paddlesToCreate; i++) {
            Debug.Log($"Instanciando paddle {i}");
            float xPos = startX + (paddleWidth / 2) + i * (paddleWidth + spacing);
            GameObject newPaddle = Instantiate(paddlePrefab, new Vector3(xPos, -3.5f, 0), Quaternion.identity);
            AssignController(newPaddle, i);
            if (GameManagerUI.instance != null && GameManagerUI.instance.gameOverPanel.activeSelf)
            {
                newPaddle.SetActive(false);
            }

            paddles.Add(newPaddle);
        }
        
        UpdateGameUI();
    }


    private void DestroyExistingPaddles()
    {
        foreach (var paddle in paddles) {
            if (paddle != null) {
                Destroy(paddle);
            }
        }
        paddles.Clear();
    }

    private void AssignController(GameObject paddle, int playerIndex)
    {
        PaddleController controller = paddle.GetComponent<PaddleController>();
        if (playerIndex == 0 && ArduinoConection.Instance.IsConnected()) {
            controller.SetController(arduinoController);
        } else {
            controller.SetController(keyboardController);
        }
    }

    void ManageControllers()
    {
        bool arduinoConnected = ArduinoConection.Instance.IsConnected();
        int currentPaddleCount = paddles.Count;
        int targetPaddleCount = arduinoConnected ? NumberOfPlayers : 1;
        
        if (currentPaddleCount != targetPaddleCount) {
            SpawnPaddle();
            return;
        }

        // Add this check
        if (currentPaddleCount > 0 && paddles[0] != null) {
            PaddleController paddleController = paddles[0].GetComponent<PaddleController>();
            bool usingArduino = paddleController.Controller is ArduinoController;
            
            if (arduinoConnected && !usingArduino) {
                Debug.Log("Arduino conectado. Jugador 1 usa Arduino.");
                paddleController.SetController(arduinoController);
            } 
            else if (!arduinoConnected && usingArduino) {
                Debug.Log("Arduino desconectado. Jugador 1 usa teclado.");
                paddleController.SetController(keyboardController);
            }
        }
    }

    private void UpdateGameUI()
    {
        if (GameManagerUI.instance != null) {
            GameManagerUI.instance.RegisterPaddles(paddles);
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
        SendLivesToArduino();
        currentBall.GetComponent<BallController>().SpawnDeathEffect();
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
        if (ArduinoConection.Instance != null)
            ArduinoConection.Instance.SendCommand("L:" + lives);
    } 
}