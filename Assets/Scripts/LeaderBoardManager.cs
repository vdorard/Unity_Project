using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;    
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    public TMP_InputField pseudoInputField; // Champ pour entrer le pseudo
    public TMP_Text leaderBoardText; // Texte pour afficher le leaderboard
    public Button submitButton; // Bouton pour valider le score

    private LeaderBoardData leaderBoardData = new LeaderBoardData(); // Données du leaderboard
    private string filePath;

    private int currentScore; // Le score actuel (à récupérer depuis ton système de score)

    void Start()
    {
        
        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        LoadLeaderBoard(); // Charger les données sauvegardées

        submitButton.onClick.AddListener(SubmitScore); // Associer la fonction de soumission
        UpdateLeaderBoardUI();
    }

    public void SetCurrentScore(int score)
{
    currentScore = score; // Assigner le score actuel avant de revenir au menu
    Debug.Log($"Score actuel dans SetCurrentScore: {currentScore}"); // Vérifiez que le score est correctement assigné
}


   void SubmitScore()
{
    string pseudo = pseudoInputField.text;

    if (!string.IsNullOrEmpty(pseudo))
    {
        // Ajouter un nouvel entry dans le leaderboard
        Debug.Log($"Ajout de l'entrée: Pseudo = {pseudo}, Score = {currentScore}");
        leaderBoardData.leaderBoardEntries.Add(new LeaderBoardEntry(pseudo, currentScore));

        // Trier le leaderboard du plus grand au plus petit score
        leaderBoardData.leaderBoardEntries.Sort((a, b) => b.score.CompareTo(a.score)); // Notez que c'est b avant a pour trier du plus grand au plus petit

        // Garder uniquement les 10 meilleurs scores
        if (leaderBoardData.leaderBoardEntries.Count > 10)
        {
            leaderBoardData.leaderBoardEntries = leaderBoardData.leaderBoardEntries.GetRange(0, 10);
        }

        // Sauvegarder le leaderboard
        SaveLeaderBoard();

        // Mettre à jour l'affichage
        UpdateLeaderBoardUI();

        // Vider le champ pseudo après soumission
        pseudoInputField.text = "";
    }
}


    void UpdateLeaderBoardUI()
    {
        leaderBoardText.text = "";
        foreach (var entry in leaderBoardData.leaderBoardEntries)
        {
            leaderBoardText.text += $"{entry.pseudo}: {entry.score}\n";
        }
    }

    void SaveLeaderBoard()
    {
        // Convertir les données du leaderboard en JSON et les sauvegarder dans un fichier
        string json = JsonUtility.ToJson(leaderBoardData, true);
        File.WriteAllText(filePath, json);
    }

    void LoadLeaderBoard()
    {
        // Si le fichier existe, charger les données
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            leaderBoardData = JsonUtility.FromJson<LeaderBoardData>(json);
        }
    }
    public void ResetLeaderBoard()
{
    leaderBoardData.leaderBoardEntries.Clear(); // Vide la liste des entrées
    SaveLeaderBoard(); // Sauvegarde le changement dans le fichier
    Debug.Log("Leaderboard réinitialisé !");
}

}

// Classe pour un entry dans le leaderboard
[System.Serializable]
public class LeaderBoardEntry
{
    public string pseudo;
    public int score;

    public LeaderBoardEntry(string pseudo, int score)
    {
        this.pseudo = pseudo;
        this.score = score;
    }
}

// Classe pour stocker tous les entries du leaderboard
[System.Serializable]
public class LeaderBoardData
{
    public List<LeaderBoardEntry> leaderBoardEntries = new List<LeaderBoardEntry>();
}
