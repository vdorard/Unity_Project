using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("------------Audiosources---------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("------------AudioClips---------------")]
    public AudioClip background;
    public AudioClip start;
    public AudioClip collision;
    public AudioClip bounce;
    public AudioClip shoot;
    public AudioClip victory;
    public AudioClip click;
    public AudioClip stop;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Charger les volumes sauvegardés
        float savedMusicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);

        // Jouer la musique de fond
        musicSource.clip = background;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
{
    musicSource.volume = volume;  // Met à jour la source directement
    PlayerPrefs.SetFloat("musicVolume", volume);
      // Ajuste aussi le mixer
}

public void SetSFXVolume(float volume)
{
    SFXSource.volume = volume;  // Met à jour la source directement
    PlayerPrefs.SetFloat("SFXVolume", volume);
      // Ajuste aussi le mixer
}

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("musicVolume", 1f);  // Valeur par défaut
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 1f);  // Valeur par défaut
    }

    public void RestartMusic()
    {
        musicSource.Stop();  // Arrête la musique actuelle
        musicSource.Play();  // Redémarre la musique
    }

    public void PlayAndChangeScene()
    {
        RestartMusic();  // Redémarre la musique avant de changer de scène
        SceneManager.LoadSceneAsync(1);  // Change la scène
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
