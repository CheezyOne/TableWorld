using UnityEngine;
using TMPro;

public class LoseWindow : BaseWindow
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private string _score;
    [SerializeField] private int _levelScoreMultiplier;

    public override void Init()
    {
        base.Init();
        _scoreText.text = _score + GetFinalScore();
    }

    private string GetFinalScore()
    {
        string score = (GameInfoHolder.Level * _levelScoreMultiplier + (int)TimerController.Instance.TimeLeft).ToString();
        return score;
    }
}