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
    private AudioSource audioSource;
    [SerializeField] private AudioClip theme ;
    [SerializeField] private float musicVolume = 0.5f;

    
    void OnEnable() {
        audioSource = GetComponent<AudioSource>();
        AudioSource musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = false;
        musicAudioSource.volume = musicVolume;
        musicAudioSource.clip = theme;
        musicAudioSource.Play();
    }


    void Start()
    {
        playBtn.onClick.AddListener(OnPlayButtonClicked);
        exitBtn.onClick.AddListener(Application.Quit);
    }

    private void OnPlayButtonClicked()
    {
        //Destroy(GameManager.Instance);
        Debug.Log("PRESIONANDO BOTON SALIR");
        SceneManager.LoadScene("MainLevel");
    }

}
