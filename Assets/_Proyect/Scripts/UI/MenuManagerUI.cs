using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManagerUI : MonoBehaviour
{
    //Crear unSingleton

    [Header("Elementos de UI")]
    
    public Button playBtn;
    public Button exitBtn;

    public GameObject ballPrefab;
    public GameObject paddlePrefab;


// Número de jugadores (puedes cambiarlo según tu lógica)
    public int numberOfPlayers;

    void Start()
    {
        playBtn.onClick.AddListener(OnPlayButtonClicked);
        exitBtn.onClick.AddListener(Application.Quit);
    }

    private void OnPlayButtonClicked()
    {
        // // Establecer el número de jugadores en el GameManager
        // GameManager.Instance.NumberOfPlayers = numberOfPlayers;
        GameManager.Instance.RedirectScene("MainLevel");
        // GameManager.Instance.paddlePrefab = paddlePrefab;
        // GameManager.Instance.ballPrefab = ballPrefab;
    }

}
