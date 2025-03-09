using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;
    private const string HighScoreKey = "HighScores";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveScore(float newScore)
    {
        List<float> scores = GetHighScores();
        scores.Add(newScore);
        scores.Sort((a, b) => b.CompareTo(a)); // Sort descending
        if (scores.Count > 10) scores.RemoveAt(10); // Keep top 10

        PlayerPrefs.SetString(HighScoreKey, string.Join(",", scores));
        PlayerPrefs.Save();
    }

    public void ClearScores()
    {
        PlayerPrefs.DeleteKey(HighScoreKey);
        PlayerPrefs.Save();
        UIController.instance.ReloadHighScore();
    }

    public List<float> GetHighScores()
    {
        string savedScores = PlayerPrefs.GetString(HighScoreKey, "");
        List<float> scores = new List<float>();

        if (!string.IsNullOrEmpty(savedScores))
        {
            string[] splitScores = savedScores.Split(',');
            foreach (string s in splitScores)
            {
                if (float.TryParse(s, out float score))
                    scores.Add(score);
            }
        }

        return scores;
    }

    public List<string> GetFormatedHighScores()
    {
        List<float> scores = GetHighScores();
        // formated scores into time string like 0:00
        List<string> formatedScores = new List<string>();
        foreach (float score in scores)
        {
            int minutes = Mathf.FloorToInt(score / 60);
            int seconds = Mathf.FloorToInt(score % 60);
            formatedScores.Add(minutes + ":" + seconds.ToString("00"));
        }
        return formatedScores;
    }
}
