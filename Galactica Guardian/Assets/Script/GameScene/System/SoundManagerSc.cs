using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SoundManagerSc : MonoBehaviour
{
    public static SoundManagerSc Instance { get; private set; }

    AudioSource bgmSource;

    int seSourcePoolSize;
    List<AudioSource> seSources = new List<AudioSource>();
    
    /// <summary>
    /// Instance初期化処理.
    /// </summary>
    void InitInstance()
    {
        if (Instance == null) // Instanceが無いなら
        {
            Instance = this; // Instanceをセット.
            DontDestroyOnLoad(gameObject); // シーンを跨いでも維持.
            InitSound(); // オーディオソースを用意.
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void InitSound()
    {
        seSourcePoolSize = Com.SE_LIST_MAX;
    }

    private void Awake()
    {
        InitInstance();
    }

    public void PlayBGM(AudioClip bgm)
    {
        bgmSource.PlayOneShot(bgm);
    }
}
