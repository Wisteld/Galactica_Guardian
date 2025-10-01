using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class ResultSceneManagerSc : MonoBehaviour
{
    [SerializeField] Text ScoreText;
    [SerializeField] Text HiScoreText;
    [SerializeField] Text KillScoreText;
    [SerializeField] Text KillBonusText;
    [SerializeField] AudioClip result_clip;

    int score = 0;
    int hiscore = 0;
    int playerHp = 0;
    
    bool noDamageFlag = false;

    bool ResultFlag = false;

    void Start()
    {
        SoundManagerSc.Instance.ChangeBGM(result_clip, true);
        ScoreManagerSc.Instance.HiScoreCheck();

        score = ScoreManagerSc.Instance.GetNowScore();
        hiscore = ScoreManagerSc.Instance.GetHighScore();
        playerHp = ScoreManagerSc.Instance.GetPlayerHp();
        noDamageFlag = ScoreManagerSc.Instance.GetNoDamageFlag();
        SetScoreText();
    }

    void Update()
    {
        
    }

    void SetScoreText()
    {
        if (ScoreText != null)
        {
            ScoreText.text = $"SCORE:{score}";
        }
        else
        {
            Debug.LogError("ScoreText None");
        }
        if (HiScoreText != null)
        {
            HiScoreText.text = $"SCORE:{hiscore}";
        }
        else
        {
            Debug.LogError("HiScoreText None");
        }
    }
}
