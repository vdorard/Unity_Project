using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score; // Variable pour stocker le score total

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Rendre le GameManager persistant
        }
        else
        {
            Destroy(gameObject); // Détruire les doublons
        }
    }
}
