using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class confirmer : MonoBehaviour
{
    public GameObject confirmerMenu;

    
    public GameObject pauseMenu;

    public void Quitter(){
        GameManager.Instance.score = 0;
        SceneManager.LoadSceneAsync(0);
        Pausemenu.GamePaused = false;
        Time.timeScale = 1f;
    }
    public void Annuler(){
        pauseMenu.SetActive(true);
        confirmerMenu.SetActive(false);
    }
}
