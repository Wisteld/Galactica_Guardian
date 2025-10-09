using UnityEngine;
using UnityEngine.UI;
using Common;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections;

public class TitleManagerSc : MonoBehaviour
{
    [Header("テキストボックス")]
    [SerializeField] Text start_message_text;
    [SerializeField] Text credit_text;
    [Header("BGM/SE")]
    [SerializeField] AudioClip clip_titlebgm;
    [SerializeField] AudioClip clip_credit;
    [SerializeField] AudioClip clip_jingle;
    int creditCount;
    bool creditFlag;

    void Start()
    {
        creditCount = 0;
        creditFlag = false;
        SoundManagerSc.Instance.PlayBGM(clip_titlebgm, true);
        ScoreManagerSc.Instance.ResetScore();
        start_message_text.text = "INSERT COIN";
        credit_text.text = $"CREDIT：{creditCount}";
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
            }
            else if(creditFlag && creditCount > 0)
            {
                creditCount--;
                StartCoroutine("StartJingle");
            }
        }
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
        yield return new WaitForSeconds(clip_jingle.length);

        StartGame();
    }
}
