using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
public class LeaderBoardDisplay : MonoBehaviour
{
    public TMP_Text leaderBoardText; // Référence au TextMeshPro où afficher le leaderboard

    private LeaderBoardData leaderBoardData = new LeaderBoardData();
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        LoadLeaderBoard(); // Charger les données sauvegardées
        UpdateLeaderBoardUI(); // Afficher le leaderboard
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
    public void leaderBoard(){
        
    }

    void UpdateLeaderBoardUI()
    {
        leaderBoardText.text = "";
        foreach (var entry in leaderBoardData.leaderBoardEntries)
        {
            leaderBoardText.text += $"{entry.pseudo}: {entry.score}\n";
        }
    }
}
