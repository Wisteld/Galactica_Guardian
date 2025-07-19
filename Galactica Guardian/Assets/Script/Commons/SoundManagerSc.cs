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
            Destroy(gameObject); // 不正なインスタンスを削除.
        }
    }

    /// <summary>
    /// オーディオ用オブジェクトプール作成.
    /// </summary>
    void InitSound()
    {
        seSourcePoolSize = Com.SE_LIST_MAX;
        bgmSource = gameObject.AddComponent<AudioSource>(); // BGM用オーディオソースを生成.
        bgmSource.volume = 1f; // BGMソースのボリュームを最大にしておく.
        bgmSource.loop = true; // 初期状態ではループをオンに.
        bgmSource.playOnAwake = false; // 開始時に音を出さなくしておく.
        for (int i = 0; i < seSourcePoolSize; i++)
        {
            AudioSource se = gameObject.AddComponent<AudioSource>(); // SE用オーディオソースを生成.
            se.playOnAwake = false;  // 開始時に音を出さなくしておく.
            seSources.Add(se); // SE用オーディオソースをリストに格納.
        }
    }

    private void Awake()
    {
        InitInstance();
    }

    /// <summary>
    /// BGM再生.
    /// </summary>
    /// <param name="bgm">再生するAudioclip.</param>
    /// <param name="isLoop">ループの有無.</param>
    public void PlayBGM(AudioClip bgm, bool isLoop)
    {
        if (Com.DEBUG_MODE_SYSTEM) Debug.Log($"BGM_Clip{bgm}");
        if (bgm == null) return; // clipが無ければ処理を止める.

        if (!bgmSource.enabled)
        {
            Debug.LogWarning("BGM AudioSource is disabled!");
            bgmSource.enabled = true; // 強制的にオンにしておく
        }

        if (bgmSource.isPlaying) // BGMが現在流れているなら.
        {
            bgmSource.Stop(); // BGMを停止.
        }

        bgmSource.clip = bgm; // clipを再生するBGMに指定.
        bgmSource.loop = isLoop; // isLoopをループ設定に代入.
        bgmSource.Play(); // BGM再生.
        if (Com.DEBUG_MODE_SYSTEM)Debug.Log($"BGM playing: {bgmSource.isPlaying}, Volume: {bgmSource.volume}, Mute: {bgmSource.mute}");
    }

    /// <summary>
    /// BGM停止.
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    /// <summary>
    /// BGMを別の曲に即時切り替える(機能的にはPlayBGMと同じ).
    /// </summary>
    /// <param name="newBgm">再生するBGM.</param>
    /// <param name="isLoop">ループの有無.</param>
    public void ChangeBGM(AudioClip newBgm, bool isLoop = true)
    {
        PlayBGM(newBgm, isLoop);
    }

    /// <summary>
    /// SE再生.
    /// </summary>
    /// <param name="seClip">再生するSE.</param>
    public void PlaySE(AudioClip seClip)
    {
        foreach (AudioSource se in seSources)
        {
            if (!se.isPlaying)
            {
                se.PlayOneShot(seClip);
                return;
            }
        }

        // すべて使用中なら1つ強制使用（デバッグ用）
        if (Com.DEBUG_MODE_SYSTEM)
        {
            Debug.LogWarning("SE AudioSource 全て使用中。強制再生。");
        }

        seSources[0].PlayOneShot(seClip);
    }
}
