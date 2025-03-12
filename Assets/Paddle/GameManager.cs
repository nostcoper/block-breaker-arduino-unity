using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// La clase GameManager hereda del Singleton gen�rico para asegurar que solo exista una instancia en la escena.
public class GameManager : Singleton<GameManager>
{
    // Variables p�blicas para puntaje y vidas, accesibles desde el Inspector.
    public int score = 0;
    public int lives = 3;

    // Prefabs asignados desde el Inspector para instanciar la bola y la paleta.
    public GameObject ballPrefab;
    public GameObject paddlePrefab;

    // Variables privadas para almacenar las instancias actuales de la bola y la paleta.
    private GameObject currentBall;
    private GameObject currentPaddle;

    // M�todo Start: se ejecuta al iniciar la escena.
    void Start()
    {
        // Se reinician vidas y puntaje al iniciar el juego.
        lives = 3;
        score = 0;

        // Se crean la paleta y la bola.
        SpawnPaddle();
        SpawnBall();
        // Se inicia una corrutina que espera un tiempo para enviar el estado de vidas al Arduino.
        StartCoroutine(EnviarVidaArduino());

        // Actualiza la UI del puntaje si la instancia de GameManagerUI existe.
        if (GameManagerUI.instance != null)
            GameManagerUI.instance.UpdateScoreUI(score);
    }

    // Corrutina que espera 0.5 segundos y luego env�a el estado de vidas al Arduino.
    IEnumerator EnviarVidaArduino()
    {
        yield return new WaitForSeconds(0.5f);
        SendLivesToArduino();
    }

    // M�todo para instanciar la paleta si a�n no existe en la escena.
    void SpawnPaddle()
    {
        if (currentPaddle == null)
        {
            // Se crea la paleta en la posici�n (0, -4, 0) sin rotaci�n (Quaternion.identity).
            currentPaddle = Instantiate(paddlePrefab, new Vector3(0, -4, 0), Quaternion.identity);
        }
    }

    // M�todo para instanciar la bola si a�n no existe en la escena.
    void SpawnBall()
    {
        if (currentBall == null)
        {
            // Se crea la bola en la posici�n (0, -3.5, 0) sin rotaci�n.
            currentBall = Instantiate(ballPrefab, new Vector3(0, -3.5f, 0), Quaternion.identity);
        }
    }

    // M�todo p�blico para sumar puntos y actualizar la UI del puntaje.
    public void AddScore(int points)
    {
        score += points;
        if (GameManagerUI.instance != null)
            GameManagerUI.instance.UpdateScoreUI(score);
    }

    // M�todo p�blico para manejar la p�rdida de una vida.
    public void LoseLife()
    {
        // Se decrementa la cantidad de vidas.
        lives--;
        // Se env�a el estado actualizado al Arduino.
        SendLivesToArduino();

        if (lives > 0)
        {
            // Si a�n quedan vidas, se reinicia la bola (se la "resetea" para que vuelva a estar pegada al paddle).
            if (currentBall != null)
                currentBall.GetComponent<BallController>().ResetBall();
        }
        else
        {
            // Si no quedan vidas, se muestra la UI de Game Over con el puntaje final.
            if (GameManagerUI.instance != null)
                GameManagerUI.instance.ShowGameOver(score);
        }
    }

    // M�todo para enviar el estado actual de las vidas al Arduino mediante el mensaje "L:<vidas>"
    void SendLivesToArduino()
    {
        // Utilizamos la instancia del ArduinoControllerPot (implementado como singleton) para enviar el mensaje.
        if (ArduinoControllerPot.Intance != null)
            ArduinoControllerPot.Intance.SendToArduino("L:" + lives);
    }
}
