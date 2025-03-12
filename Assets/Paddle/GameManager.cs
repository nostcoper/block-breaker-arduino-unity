using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// La clase GameManager hereda del Singleton genérico para asegurar que solo exista una instancia en la escena.
public class GameManager : Singleton<GameManager>
{
    // Variables públicas para puntaje y vidas, accesibles desde el Inspector.
    public int score = 0;
    public int lives = 3;

    // Prefabs asignados desde el Inspector para instanciar la bola y la paleta.
    public GameObject ballPrefab;
    public GameObject paddlePrefab;

    // Variables privadas para almacenar las instancias actuales de la bola y la paleta.
    private GameObject currentBall;
    private GameObject currentPaddle;

    // Método Start: se ejecuta al iniciar la escena.
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

    // Corrutina que espera 0.5 segundos y luego envía el estado de vidas al Arduino.
    IEnumerator EnviarVidaArduino()
    {
        yield return new WaitForSeconds(0.5f);
        SendLivesToArduino();
    }

    // Método para instanciar la paleta si aún no existe en la escena.
    void SpawnPaddle()
    {
        if (currentPaddle == null)
        {
            // Se crea la paleta en la posición (0, -4, 0) sin rotación (Quaternion.identity).
            currentPaddle = Instantiate(paddlePrefab, new Vector3(0, -4, 0), Quaternion.identity);
        }
    }

    // Método para instanciar la bola si aún no existe en la escena.
    void SpawnBall()
    {
        if (currentBall == null)
        {
            // Se crea la bola en la posición (0, -3.5, 0) sin rotación.
            currentBall = Instantiate(ballPrefab, new Vector3(0, -3.5f, 0), Quaternion.identity);
        }
    }

    // Método público para sumar puntos y actualizar la UI del puntaje.
    public void AddScore(int points)
    {
        score += points;
        if (GameManagerUI.instance != null)
            GameManagerUI.instance.UpdateScoreUI(score);
    }

    // Método público para manejar la pérdida de una vida.
    public void LoseLife()
    {
        // Se decrementa la cantidad de vidas.
        lives--;
        // Se envía el estado actualizado al Arduino.
        SendLivesToArduino();

        if (lives > 0)
        {
            // Si aún quedan vidas, se reinicia la bola (se la "resetea" para que vuelva a estar pegada al paddle).
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

    // Método para enviar el estado actual de las vidas al Arduino mediante el mensaje "L:<vidas>"
    void SendLivesToArduino()
    {
        // Utilizamos la instancia del ArduinoControllerPot (implementado como singleton) para enviar el mensaje.
        if (ArduinoControllerPot.Intance != null)
            ArduinoControllerPot.Intance.SendToArduino("L:" + lives);
    }
}
