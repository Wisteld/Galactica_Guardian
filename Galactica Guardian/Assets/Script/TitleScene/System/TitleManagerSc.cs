using UnityEngine;
using UnityEngine.UI;
using Common;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections;
using System.Drawing;

public class TitleManagerSc : MonoBehaviour
{
    [Header("テキストボックス")]
    [SerializeField] Text hiscore_text;
    [SerializeField] Text start_message_text;
    [SerializeField] Text credit_text;
    [SerializeField] float text_bring_under;
    [Header("BGM/SE")]
    [SerializeField] AudioClip clip_titlebgm;
    [SerializeField] AudioClip clip_credit;
    [SerializeField] AudioClip clip_jingle;
    int hiScore;
    float fadeSpeed;
    int creditCount;
    bool creditFlag;
    bool fadingOut;

    void Start()
    {
        SoundManagerSc.Instance.StopBGM();
        fadeSpeed = Com.FADE_SPEED;
        fadingOut = true;
        creditCount = 0;
        creditFlag = false;
        if(clip_titlebgm  != null)
        {
            SoundManagerSc.Instance.PlayBGM(clip_titlebgm, true);
        }        
        ScoreManagerSc.Instance.ResetScore();
        start_message_text.text = "INSERT COIN";
        credit_text.text = $"CREDIT：{creditCount}";
        hiScore = ScoreManagerSc.Instance.GetHighScore();
        hiscore_text.text = $"HISCORE:{hiScore}";
    }

    void Update()
    {
        if (
        #region 簡易版InputSystem
            (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
            (Mouse.current != null &&Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame) ||
            (Gamepad.all.Any(pad => pad.allControls.Any(c => c is ButtonControl b && b.wasPressedThisFrame))) ||
            (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        #endregion
            )
        {
            if(!creditFlag )
            {
                creditFlag = true;
                creditCount++;
                SoundManagerSc.Instance.PlaySE(clip_credit);
                start_message_text.text = "PUSH START BUTTON";
                credit_text.text = $"CREDIT：{creditCount}";
            }
            else if(creditFlag && creditCount > 0)
            {
                creditCount--;
                credit_text.text = $"CREDIT：{creditCount}";
                StartCoroutine("StartJingle");
            }
        }

        BlinkText(start_message_text);
    }

    /// <summary>
    /// Update内で呼び出して文字を点滅させる.
    /// </summary>
    /// <param name="blinkText">点滅させる文字</param>
    void BlinkText(Text blinkText)
    {
        UnityEngine.Color color = blinkText.color;
        if (fadingOut)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            if (color.a <= text_bring_under) // 薄くなる下限
                fadingOut = false;
        }
        else
        {
            color.a += Time.deltaTime * fadeSpeed;
            if (color.a >= 1f)
                fadingOut = true;
        }
        blinkText.color = color;
    }

    void StartGame()
    {
        SceneLoader.ChangeScene(Scenes.GAME);
    }

    IEnumerator StartJingle()
    {
        if (clip_jingle == null)
        {
            Debug.LogWarning("Jingle None");
            StartGame();
        }
        SoundManagerSc.Instance.PlayBGM(clip_jingle, false);
        fadeSpeed = Com.FADE_SPEED_FAST;
        yield return new WaitForSeconds(clip_jingle.length);

        StartGame();
    }
}
