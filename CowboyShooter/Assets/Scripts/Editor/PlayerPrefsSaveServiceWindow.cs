#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsSaveServiceWindow : EditorWindow
{
    private PlayerPrefsSaveService _service;
    private string _playerName = "Player";
    private int _score;
    private List<ScoreEntry> _scores = new List<ScoreEntry>();
    private Vector2 _scrollPosition;

    [MenuItem("Tools/PlayerPrefs Save Service Tester")]
    private static void ShowWindow()
    {
        var window = GetWindow<PlayerPrefsSaveServiceWindow>();
        window.titleContent = new GUIContent("Score Save Tester");
        window.minSize = new Vector2(320, 300);
    }

    private void OnEnable()
    {
        _service = new PlayerPrefsSaveService();
        RefreshScores();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("PlayerPrefsSaveService Tester", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        _playerName = EditorGUILayout.TextField("Player Name", _playerName);
        _score = EditorGUILayout.IntField("Score", _score);

        EditorGUILayout.Space();
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Save Score"))
            {
                _service.Save(_playerName, _score);
                RefreshScores();
            }

            if (GUILayout.Button("Refresh"))
            {
                RefreshScores();
            }

            using (new EditorGUI.DisabledScope(_scores.Count == 0))
            {
                if (GUILayout.Button("Clear Scores"))
                {
                    if (EditorUtility.DisplayDialog(
                        "Clear Scores",
                        "Delete all scores saved by PlayerPrefsSaveService?",
                        "Clear",
                        "Cancel"))
                    {
                        _service.ClearScores();
                        RefreshScores();
                    }
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"Saved Scores ({_scores.Count})", EditorStyles.boldLabel);

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, "box");
        if (_scores.Count == 0)
        {
            EditorGUILayout.LabelField("No scores saved.");
        }
        else
        {
            for (var i = 0; i < _scores.Count; i++)
            {
                var entry = _scores[i];
                EditorGUILayout.LabelField($"{i + 1}. {entry.UserName} — {entry.Score}");
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void RefreshScores()
    {
        _scores = _service.GetScores();
    }
}
#endif