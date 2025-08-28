using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Common;

public class TitleManagerSc : MonoBehaviour
{
    [SerializeField] AudioClip titlebgm_clip;
    // Start is called before the first frame update
    void Start()
    {
        SoundManagerSc.Instance.PlayBGM(titlebgm_clip, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneLoader.ChangeScene(Scenes.GAME);
    }
}
