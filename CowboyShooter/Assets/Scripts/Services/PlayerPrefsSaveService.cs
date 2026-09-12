using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPrefsSaveService : ISaveService
{
    public List<ScoreEntry> GetScores()
    {
        List<ScoreEntry> scores = LoadScores().entries.OrderBy(x => x.Score).Reverse().ToList();
        return scores;
    }

    public void Save(string userName, int score)
    {
        ScoreEntryList scores = LoadScores();
        ScoreEntry entry = new ScoreEntry(userName, score);
        scores.entries.Add(entry);
        string json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString("ScoreEntry", json);
    }

    private ScoreEntryList LoadScores()
    {
        string json = PlayerPrefs.GetString("ScoreEntry", "");
        if (string.IsNullOrEmpty(json))
        {
            return new ScoreEntryList();
        }
        return JsonUtility.FromJson<ScoreEntryList>(json);
    }

    public void ClearScores()
    {
        PlayerPrefs.DeleteKey("ScoreEntry");
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat("Volume", 1f);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
    }

}
