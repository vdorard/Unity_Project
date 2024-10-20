using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    [Header("------------UI---------------")]
    [SerializeField] private Slider volumeMusicSlider;
    [SerializeField] private Slider volumeSFXSlider;
    public AudioMixer audioMixer;

    void Start()
{
    // Synchroniser les sliders avec l'AudioManager    
    float initialMusicVolume = AudioManager.instance.GetMusicVolume();
    float initialSFXVolume = AudioManager.instance.GetSFXVolume();
    
    volumeMusicSlider.value = initialMusicVolume;
    volumeSFXSlider.value = initialSFXVolume;

    // Appliquer les volumes dès le départ
    SetMusicVolume(initialMusicVolume);
    SetSFXVolume(initialSFXVolume);

    // Ajouter des listeners pour gérer les changements de volume
    volumeMusicSlider.onValueChanged.AddListener(SetMusicVolume);
    volumeSFXSlider.onValueChanged.AddListener(SetSFXVolume);
}


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void PlayGame()
    {
        audioManager.PlayAndChangeScene();
        audioManager.PlaySFX(audioManager.victory);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SetSFXVolume(volume);
        audioMixer.SetFloat("volumesfx", Mathf.Log10(volume) * 20);
    }
}
