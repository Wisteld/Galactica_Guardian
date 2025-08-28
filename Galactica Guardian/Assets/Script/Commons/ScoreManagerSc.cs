using UnityEngine;
using UnityEngine.UI;
using Common;
using Snum = Common.Score.Snum;

public class ScoreManagerSc : MonoBehaviour
{
    public static ScoreManagerSc Instance { get; private set; }

    int nowScore;
    int nowKillScore;
    int[] highScore = new int[(int)Snum.STAGE_MAX];
    int[] highKillScore = new int[(int)Snum.STAGE_MAX];

    /// <summary>
    /// 指定したステージ番号のハイスコアを取得する.
    /// </summary>
    /// <param name="num">ステージ番号.</param>
    /// <returns></returns>
    public int GetHighScore(Snum num)
    {
        return highScore[(int)num];
    }

    /// <summary>
    /// 指定したステージ番号のハイキルスコアを取得する.
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public int GetHigeKillScore(Snum num)
    {
        return highKillScore[(int)num];
    }

    /// <summary>
    /// インスタンスを初期化.
    /// </summary>
    void InitInstance()
    {
        if(Instance == null)
        {
            Instance = this; // インスタンスをセット.
            DontDestroyOnLoad(gameObject); // シーンを跨いでも消えないようにする.
            InitScore();
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 不正なインスタンスを削除.
        }
    }

    /// <summary>
    /// Scoreを初期化.
    /// </summary>
    void InitScore()
    {
        for (int i = 0; i < (int)Snum.STAGE_MAX; i++)
        {
            if (!PlayerPrefs.HasKey("HighScore_" + i))
            {
                Debug.Log($"HigeScore_{i} Not Found Set Default HighScore");
                highScore[i] = Score.DEFAULT_HIGHSCORE;
            }
            else
            {
                Debug.Log($"HigeScore_{i} Found");
            }
            if (!PlayerPrefs.HasKey("HighKillScore_" + i))
            {
                Debug.Log($"HigeScore_{i} Not Found Set Default HighScore");
                highScore[i] = Score.DEFAULT_HIGHSCORE;
            }
            else
            {
                Debug.Log($"HigeKillScore_{i} Found");
            }
        }
        LoadScore();
    }

    void LoadScore()
    {
        // ハイスコアを読みこむ.
        for(int i = 0; i < (int)Snum.STAGE_MAX; i++)
        {
            highScore[i] = PlayerPrefs.GetInt("HighScore_" + i);
        }
    }

    void Awake()
    {
        InitInstance();
    }

    void OnDestroy()
    {
        SaveScore();
    }

    /// <summary>
    /// 現在のスコアを加算.
    /// </summary>
    /// <param name="score">増加するスコア.</param>
    public void UpdateScore(int score)
    {
        nowScore += score;
    }

    /// <summary>
    /// 現在のキルスコアを加算(引数無しなら1加算).
    /// </summary>
    /// <param name="killscore">増加するキルスコア.</param>
    public void UpdateKillScore(int killscore = 1)
    {
        nowKillScore += killscore;
    }

    /// <summary>
    /// スコアをリセットする.
    /// </summary>
    public void ResetScore()
    {
        nowScore = 0;
        nowKillScore = 0;
    }

    /// <summary>
    /// ハイスコアが更新されたか調べる.
    /// </summary>
    /// <param name="num"></param>
    public void HiScoreCheck(Snum num)
    {
        // ハイスコア更新.
        if(nowScore > highScore[(int)num])
        {
            highScore[(int)num] = nowScore;
            SaveScore();
        }
        if(nowKillScore > highKillScore[(int)num])
        {
            highKillScore[(int)num] = nowKillScore;
            SaveKillScore();
        }
    }

    /// <summary>
    /// スコアセーブ.
    /// </summary>
    void SaveScore()
    {
        for(int i = 0; i < (int)(Snum.STAGE_MAX); i++)
        {
            PlayerPrefs.SetInt("HighScore_" + i, highScore[i]);
        }
        PlayerPrefs.Save();
    }

    void SaveKillScore()
    {
        for (int i = 0; i < (int)(Snum.STAGE_MAX); i++)
        {
            PlayerPrefs.SetInt("HighKillScore_" + i, highKillScore[i]);
        }
        PlayerPrefs.Save();
    }
}
