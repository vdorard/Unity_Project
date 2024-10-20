using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioLevel1 : MonoBehaviour
{
    [Header("------------Audiosources---------------")]
   [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource SFXSource;

    [Header("------------AudioClips---------------")]
   public AudioClip background;
   public AudioClip start;
   public AudioClip collision;
   public AudioClip bounce;
   public AudioClip shoot;
   public AudioClip click;

   public AudioClip victory;

   

private static AudioManager instance;
//    public void Start(){
//     musicSource.clip = background;
//     musicSource.Play();

//    }
   

    public void RestartMusic(){
        musicSource.Stop();  // Arrête la musique actuelle
        musicSource.Play();  // Redémarre la musique
    }
    public void PlayAndChangeScene(){
        RestartMusic();  // Redémarre la musique avant de changer de scène
        SceneManager.LoadSceneAsync(1);  // Change la scène
    }
   public void OnVolumeSliderChanged(float value){
        musicSource.volume = value;  
    }
   public void PlaySFX(AudioClip clip){
    SFXSource.PlayOneShot(clip);
   }

}
