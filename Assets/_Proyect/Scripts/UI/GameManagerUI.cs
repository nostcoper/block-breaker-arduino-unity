using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI instance;

    [Header("Elementos de UI")]
    public TextMeshProUGUI scoreText;           // Asigna desde el Inspector el Text que muestra el puntaje
    public GameObject gameOverPanel; // Panel que se activa al Game Over
    public TextMeshProUGUI gameOverScoreText;   // Texto para mostrar el puntaje final en Game Over

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

    // Actualiza el texto del puntaje
    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
            scoreText.text = "SCORE \n" + score;
    }

    // Muestra el panel de Game Over y actualiza el puntaje final
    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverScoreText != null)
                gameOverScoreText.text = "Puntaje Final: " + finalScore;
        }
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Se asigna al bot�n "Salir"
    public void OnExitButton()
    {
        Application.Quit();
    }
}
