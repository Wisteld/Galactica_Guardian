using UnityEngine;
using UnityEngine.UI;

public class UIScoreManagerSc : MonoBehaviour
{
    [SerializeField] Text score_text;

    [SerializeField] Text hiscore_text;

    int hiScore;

    void InitUI()
    {
        hiScore = ScoreManagerSc.Instance.GetHighScore();
        hiscore_text.text = $"HISCORE:{hiScore}";
    }

    private void Awake()
    {
        ScoreManagerSc.Instance.OnScoreChanged += UpdateUI;
    }

    void Start()
    {
        InitUI();
    }

    private void OnDestroy()
    {
        if (ScoreManagerSc.Instance != null)
            ScoreManagerSc.Instance.OnScoreChanged -= UpdateUI;
    }

    void UpdateUI(int score)
    {
        score_text.text = $"SCORE: {score}";
    }
}
