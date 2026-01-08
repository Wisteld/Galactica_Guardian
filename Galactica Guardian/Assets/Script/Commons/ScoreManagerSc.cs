using UnityEngine;
using Common;
using Snum = Common.Score.Snum;

public class ScoreManagerSc : MonoBehaviour
{
    public static ScoreManagerSc Instance { get; private set; }

    public event System.Action<int> OnScoreChanged;

    int nowScore;
    int nowKillScore;
    int[] highScore = new int[(int)Snum.RANK_MAX];

    int playerHp = 0;
    bool noDamageFlag = false;

    public int GetNowScore()
    {
        return nowScore;
    }

    public int GetHighScore(int num = 0)
    {
        Debug.Log($"ハイスコアが返されます。{num}番目を参照します。ハイスコア{highScore[num]}");
        return highScore[num];
    }

    public int GetNowKillScore()
    {
        return nowKillScore;
    }

    public int GetPlayerHp()
    {
        return playerHp;
    }

    public bool GetNoDamageFlag()
    {
        return noDamageFlag;
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
            Debug.Log("Instance生成完了、HIScoreを初期化");
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
        LoadScore();
    }

    void LoadScore()
    {
        // 上位3件のデモスコアを初期セット
        int[] defaultScores = new int[]
        {
        Score.DEFAULT_HIGHSCORE,   // RANK_1
        Score.DEFAULT_MIDDLESCORE, // RANK_2
        Score.DEFAULT_LOWSCORE     // RANK_3
        };

        bool wroteDefaults = false;

        // ハイスコアを読みこむ.
        for (int i = 0; i < (int)Snum.RANK_MAX; i++)
        {
            if (!PlayerPrefs.HasKey("HighScore_" + i) || 0 == PlayerPrefs.GetInt("HighScore_" + i))
            {
                Debug.Log($"HighScore_{i} Not Found. Set Default {defaultScores[i]}");
                highScore[i] = defaultScores[i];
                PlayerPrefs.SetInt("HighScore_" + i, defaultScores[i]);
                wroteDefaults = true;
            }
            else
            {
                highScore[i] = PlayerPrefs.GetInt("HighScore_" + i);
                Debug.Log($"HighScore_{i} Check HIScore {highScore[i]}");
            }
        }

        if (wroteDefaults)
        {
            SaveScore();
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
        OnScoreChanged?.Invoke(nowScore);
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
    /// ボーナススコア判定用のプレイヤー情報を取得
    /// </summary>
    public void GetPlayerBonus()
    {
        playerHp = PlayerSc.Instance.GetPlayerHp();
        noDamageFlag = PlayerSc.Instance.GetNoDamageFlag();
    }

    /// <summary>
    /// スコアをリセットする.
    /// </summary>
    public void ResetScore()
    {
        nowScore = 0;
        nowKillScore = 0;
        OnScoreChanged?.Invoke(nowScore);
        playerHp = 0;
        noDamageFlag = false;
    }

    /// <summary>
    /// ハイスコアが更新されたか調べる.
    /// </summary>
    public void HiScoreCheck()
    {
        int ToScore = nowScore + (nowKillScore * Score.SCORE_BONUS_KILL);

        // ハイスコア更新.
        // スコア配列に現在スコアを追加して、降順にソート
        var scores = new System.Collections.Generic.List<int>(highScore)
        {ToScore};
        scores.Sort((a, b) => b.CompareTo(a)); // 降順ソート

        // 上位3件だけ残す
        for (int i = 0; i < (int)Snum.RANK_MAX; i++)
        {
            highScore[i] = scores[i];
        }

        SaveScore();
    }

    /// <summary>
    /// スコアセーブ.
    /// </summary>
    void SaveScore()
    {
        for(int i = 0; i < (int)(Snum.RANK_MAX); i++)
        {
            PlayerPrefs.SetInt("HighScore_" + i, highScore[i]);
        }
        PlayerPrefs.Save();
    }
}
