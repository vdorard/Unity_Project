using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class endLevel : MonoBehaviour
{
    private AudioManager audioManager;
    private Playerbehave playerbehave; 
    public GameObject endScreen;
    private LeaderBoardManager leaderBoardManager; 
    public GameObject leaderboardScreen;
    public void Awake(){
    audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); 
    playerbehave = GameObject.FindGameObjectWithTag("Player").GetComponent<Playerbehave>();
    leaderBoardManager = FindObjectOfType<LeaderBoardManager>();

    if (leaderBoardManager == null)
    {
        Debug.LogError("LeaderBoardManager n'a pas été trouvé !");
    }
}

    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player")){
            audioManager.PlaySFX(audioManager.victory);
            Debug.Log("Collision !");
            playerbehave.UpdateEndText();
            Time.timeScale = 0f;
            endScreen.SetActive(true);

        }

    }
    
    public void endGame(){
        endScreen.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.score = 0;
        SceneManager.LoadSceneAsync(0);


    }
    public void sauvegarder(){
        leaderboardScreen.SetActive(true);
        endScreen.SetActive(false);

    }
    public void sauvegarderetquitter()
{
    leaderboardScreen.SetActive(false);
    
    // Ajout d'un log pour vérifier la valeur du score avant sauvegarde
    leaderBoardManager.SetCurrentScore(GameManager.Instance.score);
    Debug.Log("Score actuel avant sauvegarde: " + GameManager.Instance.score);
    
    
    // Charger le menu principal
    GameManager.Instance.score = 0;
    SceneManager.LoadScene(0);
    Time.timeScale = 1f;
    // Ne réinitialisez pas le score ici pour qu'il soit correctement sauvegardé
}


    public void nextlevel(){
        endScreen.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(2);
    }
    
    
}
