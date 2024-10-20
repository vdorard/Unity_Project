using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class canvapause : MonoBehaviour
{
  public GameObject pauseMenuUI;
  
    public void Pause(){
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;
    Pausemenu.GamePaused = true;
  }
}
