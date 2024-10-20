using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Pausemenu : MonoBehaviour
{
    private bool inSetting = false;
    private AudioManager audioManager;
    public static bool GamePaused = false;

    [Header("------------UI---------------")]
    public GameObject pauseMenuUI;
    public GameObject SettingMenuUI;
    public GameObject ConfirmerMenuUI;
    [SerializeField] private Slider volumeMusicSlider;
    [SerializeField] private Slider volumeSFXSlider;
    public AudioMixer audioMixer;

    void Start()
    {
        // Synchronise les sliders avec l'AudioManager
        volumeMusicSlider.value = AudioManager.instance.GetMusicVolume();
        volumeSFXSlider.value = AudioManager.instance.GetSFXVolume();

        // Ajoute des listeners pour gérer les changements de volume
        volumeMusicSlider.onValueChanged.AddListener(SetMusicVolume);
        volumeSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void goSetting()
    {
        pauseMenuUI.SetActive(false);
        SettingMenuUI.SetActive(true);
        inSetting = true;
    }

    public void Back()
    {
        pauseMenuUI.SetActive(true);
        SettingMenuUI.SetActive(false);
        inSetting = false;
    }

    public void QuitGame()
    {
        ConfirmerMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !inSetting)
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && inSetting)
        {
            pauseMenuUI.SetActive(true);
            SettingMenuUI.SetActive(false);
            inSetting = false;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }
}
