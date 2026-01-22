using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Common;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;

public class ResultSceneManagerSc : MonoBehaviour
{
    [SerializeField] Text score_text;
    [SerializeField] Text hi_score_text;
    [SerializeField] Text kill_score_text;
    [SerializeField] Text kill_bonus_text;
    [SerializeField] Text hp_bonus_text;
    [SerializeField] Text no_damage_bonus_text;
    [SerializeField] GameObject thank_you_text;
    [SerializeField] AudioClip result_clip;
    [SerializeField] Image[] hp_segments;   // 並べたHPアイコン
    [SerializeField] Sprite[] full_sprites;    // [0]=左, [1]=中, [2]=右
    [SerializeField] Sprite[] empty_sprites;   // [0]=左, [1]=中, [2]=右
    [Header("ボーナスの表示待ち時間")]
    [SerializeField] float wait_bonus_time;
    [Header("効果音")]
    [SerializeField] AudioClip clip_bonus;
    [SerializeField] AudioClip clip_bonus_last;

    int score = 0;
    int hiscore = 0;
    int killScore = 0;
    int playerHp = 0;
    
    bool noDamageFlag = false;

    bool resultFlag = false;

    void Start()
    {
        SoundManagerSc.Instance.ChangeBGM(result_clip, true);
        ScoreManagerSc.Instance.HiScoreCheck();

        resultFlag = false;
        thank_you_text.SetActive(false);
        score = ScoreManagerSc.Instance.GetNowScore();
        hiscore = ScoreManagerSc.Instance.GetHighScore();
        killScore = ScoreManagerSc.Instance.GetNowKillScore();
        playerHp = ScoreManagerSc.Instance.GetPlayerHp();
        noDamageFlag = ScoreManagerSc.Instance.GetNoDamageFlag();
        UpdateHPUI(playerHp);
        SetScoreText();
        StartCoroutine(CheckBonus());
    }

    void Update()
    {
        if(resultFlag == false) { return; }

        if (
        #region 簡易版InputSystem
            (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame) ||
            (Gamepad.all.Any(pad => pad.allControls.Any(c => c is ButtonControl b && b.wasPressedThisFrame)) ||
            (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)))
        #endregion)

        {
            SceneLoader.ChangeScene(Scenes.TITLE);
        }
    }

    void SetScoreText()
    {
        if (score > hiscore) { hiscore = score; }
        if (score_text != null)
        {
            score_text.text = $"SCORE:{score}";
        }
        else
        {
            Debug.LogError("ScoreText None");
        }
        if (hi_score_text != null)
        {
            hi_score_text.text = $"HISCORE:{hiscore}";
        }
        else
        {
            Debug.LogError("HiScoreText None");
        }
    }

    void UpdateHPUI(int currentHP)
    {
        for (int i = 0; i < hp_segments.Length; i++)
        {
            hp_segments[i].sprite = (i < currentHP) ? full_sprites[i] : empty_sprites[i];
        }
    }

    IEnumerator CheckBonus()
    {
        if (kill_score_text != null && kill_bonus_text != null)
        {
            kill_score_text.text = $"KILL BONUS {killScore}";
            yield return new WaitForSeconds(wait_bonus_time);
            kill_bonus_text.text = $"× {Score.SCORE_BONUS_KILL}";
            score += killScore * Score.SCORE_BONUS_KILL;
            SetScoreText();
            SoundManagerSc.Instance.PlaySE(clip_bonus);
        }
        else
        {
            Debug.LogError("KillScoreText None");
        }

        yield return new WaitForSeconds(wait_bonus_time);

        if (hp_bonus_text != null)
        {
            hp_bonus_text.text = $"× {Score.SCORE_BONUS_HP}";
            score += playerHp * Score.SCORE_BONUS_HP;
            SetScoreText();
            SoundManagerSc.Instance.PlaySE(clip_bonus);
        }
        else
        {
            Debug.LogError("HPBonusText None");
        }

        yield return new WaitForSeconds(wait_bonus_time);

        if (no_damage_bonus_text != null)
        {
            if (noDamageFlag)
            {
                no_damage_bonus_text.text = $"× {Score.SCORE_BONUS_SECRET}";
                score += Score.SCORE_BONUS_SECRET;
                SetScoreText();
                SoundManagerSc.Instance.PlaySE(clip_bonus_last);
            }
            else
            {
                no_damage_bonus_text.text = "× 0";
                SoundManagerSc.Instance.PlaySE(clip_bonus_last);
            }
        }
        else
        {
            Debug.LogError("NoDamageBonusText None");
        }
        ScoreManagerSc.Instance.UpdateScore(score);
        ScoreManagerSc.Instance.HiScoreCheck();
        thank_you_text.SetActive(true);

        resultFlag = true;
    }
}
