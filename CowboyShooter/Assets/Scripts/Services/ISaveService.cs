using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveService
{
    void Save(string userName, int score);
    List<ScoreEntry> GetScores();
    void ClearScores();
}

[Serializable]
public struct ScoreEntry
{
    public string UserName;
    public int Score;
    public ScoreEntry(string userName, int score)
    {
        UserName = userName;
        Score = score;
    }
}

[Serializable]
public class ScoreEntryList
{
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}