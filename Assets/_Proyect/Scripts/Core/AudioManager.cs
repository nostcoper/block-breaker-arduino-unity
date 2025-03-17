using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
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

    public void PlayOneShotSound(AudioClip sound){
        audioSource.PlayOneShot(sound);
    }
}
