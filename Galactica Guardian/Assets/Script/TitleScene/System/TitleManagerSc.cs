using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Common;

public class TitleManagerSc : MonoBehaviour
{
    [SerializeField] AudioClip titlebgm_clip;

    void Start()
    {
        SoundManagerSc.Instance.PlayBGM(titlebgm_clip, true);
        ScoreManagerSc.Instance.ResetScore();
    }

    public void StartGame()
    {
        SceneLoader.ChangeScene(Scenes.GAME);
    }
}
