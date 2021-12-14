using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScoreView : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        if(LevelGenerator.Instance)
        {
            LevelGenerator.Instance.ScoreChanged += OnScoreChanged;
        }
    }

    private void OnScoreChanged(object sender, int updatedScore)
    {
        if (_scoreText)
        {
            _scoreText.text = updatedScore.ToString();
        }
    }
}
