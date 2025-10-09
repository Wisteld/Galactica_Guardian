using UnityEngine;
using ObjectPool;
using Common;
using E_Type = Common.ENum.E_Type;
using static Common.ENum;
using System.Collections.Generic;
using System;
using System.Collections;

public class GameManagerSc : MonoBehaviour
{
    #region SerializeField.
    [Header("生成するエネミー")]
    [SerializeField] GameObject enemy_prefab;
    [SerializeField] GameObject enemyα_prefab;
    [SerializeField] GameObject enemyβ_prefab;
    [SerializeField] GameObject enemy_hme_prefab;
    [SerializeField] GameObject enemy_boss_prefab;
    [SerializeField] GameObject item_carrier_prefab;
    [Header("エネミー生成データ")]
    [SerializeField] List<WaveData> wave_data;
    [Tooltip("裏ボスBGM")]
    [SerializeField] AudioClip secretBossBGM;
    // [SerializeField] private string waveSetPath = "WaveData_S1";
    [Header("生成するエフェクト")]
    [SerializeField] GameObject explosion_prefab;
    [SerializeField] GameObject explosion_min_prefab;
    [Header("生成する弾")]
    [SerializeField] GameObject player_bullet_prefab;
    [SerializeField] GameObject player_laser_prefab;
    [SerializeField] GameObject player_missile_prefab;
    [SerializeField] GameObject enemy_bullet_prefab;
    [SerializeField] GameObject enemy_bullet_lock_prefab;
    [SerializeField] GameObject enemy_missile_prefab;
    #endregion
    #region Instance・変数.
    public static GameManagerSc Instance { get; private set; }
    public Vector2 screenMin { get; private set; }
    public Vector2 screenMax { get; private set; }

    Dictionary<AnchorType, Vector3> anchorPositions = new Dictionary<AnchorType, Vector3>();

    int currentWaveIndex;
    int activeEnemyCount;

    int popEnemyCount;

    bool isWaveRunning = false;
    bool isKillBonus = false;
    bool debugFlag = Com.DEBUG_MODE_SYSTEM;
    #endregion

    #region 初期化処理.
    /// <summary>
    /// カメラ初期化.
    /// </summary>
    void InitCamera()
    {
        // 画面の高さに合わせてカメラサイズ調整(縦スクロールなので高さ優先).
        float targetWidth = 720f / 100f; // 1ユニット = 100px 換算.
        float targetAspect = targetWidth / (1080f / 100f); // 9:16のアスペクト比を設定.

        float windowAspect = (float)Screen.width / (float)Screen.height; // 現在の実行環境のアスペクト比を取得(小数点以下の切り捨て無し).
        float scaleHeight = windowAspect / targetAspect; // 現在のアスペクト比が意図するアスペクト比とどの程度ズレているか把握.

        Camera cam = Camera.main; // メインカメラ取得.

        if (scaleHeight < 1.0f) // 画面が横長の場合.
        {
            Rect rect = cam.rect; // カメラの現在のビューポートを取得.

            rect.width = 1.0f;    // カメラの表示幅を最大に.
            rect.height = scaleHeight; // 縦方向の表示範囲縮小(グラフィックが横に引き延ばされないように).
            rect.x = 0; // カメラ表示領域の左端を画面の左端に合わせる.
            rect.y = (1.0f - scaleHeight) / 2.0f; // 表示領域を画面の中央に持ってくる.

            cam.rect = rect; // 調整内容をカメラに適用.
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight; // 幅のスケールを計算.

            Rect rect = cam.rect; // カメラの現在のビューポートを取得.

            rect.width = scaleWidth; // 横方向の表示範囲縮小(グラフィックの引き伸ばし対策).
            rect.height = 1.0f;      // カメラの表示高さを最大に.
            rect.x = (1.0f - scaleWidth) / 2.0f; // 表示領域を画面中央に持ってくる.
            rect.y = 0; // カメラの表示範囲の下端を画面の下端に合わせる.

            cam.rect = rect;
        }

        if (Instance == null) Instance = this; // Instanceが無ければInstanceを設定.

        // 一度だけ取得
        screenMin = Camera.main.ViewportToWorldPoint(Vector2.zero); // 画面の左下隅の座標.
        screenMax = Camera.main.ViewportToWorldPoint(Vector2.one);  // 画面の右上隅の座標.
    }

    /// <summary>
    /// 生成座標初期化.
    /// </summary>
    void InitPop()
    {
        GameObject[] anchorObjects = GameObject.FindGameObjectsWithTag(Tags.POP_ANCHOR); // tag指定でAnchorを配列取得.
        foreach (var obj in anchorObjects)
        {
            string name = obj.name.Replace("Anchor_", ""); // Anchor_部分を削除してstring型変数nameに入れる.
            if (Enum.TryParse(name, out AnchorType type)) // nameと同じ文字列の列挙体があるか探す.
            {
                anchorPositions[type] = obj.transform.position; // 座標を対応する列挙体と紐づけて格納.
                if (debugFlag)
                {
                    Debug.Log($"Anchor取得:{obj.name}");
                }
            }
            else // 無ければ警告.
            {
                Debug.LogWarning($"不正なアンカー名: {obj.name}");
            }
        }
    }

    void InitManager()
    {
        currentWaveIndex = 0;
    }

    void InitSetWaves()
    {
        if (wave_data == null)
        {
            // wave_data = new List<WaveSetData> { Resources.Load<WaveSetData>(waveSetPath) };
            if (wave_data == null)
            {
                Debug.LogError("WaveSetData が Resources から読み込めませんでした！");
            }
        }
    }

    /// <summary>
    /// ObjectPoolのInstanceをClearする.
    /// </summary>
    void InstanceClear()
    {
        EnemyPool.Instance.ClearInstance();
        EffectPool.Instance.ClearInstance();
        BulletPool.Instance.ClearInstance();
    }
    #endregion

    #region UnityEvent.
    private void Awake()
    {
        EnemyPool.Instance.GenerateEnemy(enemy_prefab, enemyα_prefab, enemyβ_prefab,
            enemy_hme_prefab, enemy_boss_prefab, item_carrier_prefab); // エネミーオブジェクトプール準備.
        EffectPool.Instance.GenerateEffect(explosion_prefab, explosion_min_prefab); // エフェクトオブジェクトプール準備.

        BulletPool.Instance.GenerateBullet(player_bullet_prefab, player_laser_prefab, player_missile_prefab,
            enemy_bullet_prefab, enemy_bullet_lock_prefab, enemy_missile_prefab); // 各種弾オブジェクトプール準備.

        InitCamera();
        InitSetWaves();
    }
    void Start()
    {
        InitPop();
        InitManager();
        if (!isWaveRunning)
        {
            StartCoroutine(WaveRoutine());
        }
    }

    private void OnDestroy()
    {
        // Sceneを再度読み込んだ際にObjectPoolが再生成出来るようInstanceをリセットしておく.
        InstanceClear();
    }
    #endregion

    bool CheckKillRate(float threshold = Score.SCORE_BONUS_KILL_PERCENT)
    {
        if (popEnemyCount == 0) return false;
        float rate = (float)ScoreManagerSc.Instance.GetNowKillScore()/popEnemyCount;
        Debug.Log($"KillRate={rate}({(float)ScoreManagerSc.Instance.GetNowKillScore() / popEnemyCount}");
        return rate >= threshold;
    }

    public bool TryGetAnchor(AnchorType type, out Vector3 pos)
    {
        return anchorPositions.TryGetValue(type, out pos);
    }

    IEnumerator WaveRoutine()
    {
        //for (int setIndex = 0; setIndex < wave_data.Count; setIndex++)
        //{
        //    WaveSetData currentWaveSet = wave_data[setIndex];

            for (int waveIndex = 0; waveIndex < wave_data.Count; waveIndex++)
            {
                WaveData currentWaveData = wave_data[waveIndex];
                if (debugFlag)
                {
                    Debug.Log($"開始: Wave {currentWaveIndex + 1} / {wave_data.Count}");
                }

                if (currentWaveData.isBossWave)
                { if (CheckKillRate()) { isKillBonus = true; } }

                if (currentWaveData.waveBGM != null)
                {
                    SoundManagerSc.Instance.PlayBGM(currentWaveData.waveBGM, currentWaveData.isLoopBGM);
                }

                isWaveRunning = true;
                activeEnemyCount = currentWaveData.spawns.Count;

                foreach (var spawn in currentWaveData.spawns)
                {
                    StartCoroutine(SpawnEnemyWithDelay(spawn));
                }

                yield return new WaitUntil(() => activeEnemyCount <= 0); // 全てのエネミーが居なくなったら.

                if (debugFlag)
                {
                    Debug.Log($"Wave{currentWaveIndex + 1}End");
                }

            yield return new WaitForSeconds(4f); // 次のWaveまでの待機
                currentWaveIndex++;

            if (currentWaveData.isBossWave)
            {
                if (isKillBonus)
                {
                    ScoreManagerSc.Instance.UpdateScore(Score.SCORE_BONUS_B);
                }
                if (ScoreManagerSc.Instance.GetNowScore() >= Com.ENEMY_HME_POPBORDER) // ←条件スコア
                {
                    Debug.Log("条件達成！裏ボス戦へ突入！");
                    anchorPositions.TryGetValue(AnchorType.CENTER, out Vector3 spawnPos);
                    EnemyPool.Instance.Generate(E_Type.CARRIER, spawnPos);
                    yield return StartCoroutine(SpawnSecretBoss());
                    yield break; // 裏ボス後はここでWaveRoutine終了
                }
            }
        }
        //}

        // 全Wave終了後.
        Debug.Log("全Wave終了!");
        ScoreManagerSc.Instance.GetPlayerBonus();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_TIME);
        SoundManagerSc.Instance.StopBGM();
        SceneLoader.ChangeScene(Scenes.RESULT);
    }

    IEnumerator SpawnEnemyWithDelay(EnemySpawnData spawnData)
    {
        yield return new WaitForSeconds(spawnData.appearTime);

        if (anchorPositions.TryGetValue(spawnData.spawnPosition, out Vector3 spawnPos))
        {
            GameObject enemy = EnemyPool.Instance.Generate(spawnData.enemyType, spawnPos);

            if (spawnData.enemyType != E_Type.CARRIER) // Carrier以外のエネミーをカウントしておく.
            {
                popEnemyCount++;
            }

            if (enemy != null)
            {
                Enemy_BaseSc enemyScript = enemy.GetComponent<Enemy_BaseSc>();
                if (enemyScript != null)
                {
                    enemyScript.OnDeath = () =>
                    {
                        activeEnemyCount--;
                        if (debugFlag) Debug.Log($"残り: {activeEnemyCount}");
                    };
                }
            }
        }
        else
        {
            Debug.LogWarning($"アンカー位置が不明: {spawnData.spawnPosition}");
        }
    }

    IEnumerator SpawnSecretBoss()
    {
        // BGM切替（裏ボス専用曲があれば）
        SoundManagerSc.Instance.PlayBGM(secretBossBGM, true);

        // ちょっと演出
        yield return new WaitForSeconds(2f);

        // 出現位置は中央Anchorを想定（必要なら調整）
        if (anchorPositions.TryGetValue(AnchorType.CENTER, out Vector3 pos))
        {
            GameObject hme = EnemyPool.Instance.Generate(E_Type.ENEMY_HME, pos);

            if (hme != null)
            {
                Enemy_BaseSc hmeScript = hme.GetComponent<Enemy_BaseSc>();
                if (hmeScript != null)
                {
                    activeEnemyCount = 1;
                    hmeScript.OnDeath = () =>
                    {
                        activeEnemyCount--;
                    };
                }
            }
        }
        else
        {
            Debug.LogWarning("裏ボス生成位置（Anchor）が見つかりませんでした！");
        }

        // 裏ボス撃破待ち
        yield return new WaitUntil(() => activeEnemyCount <= 0);

        Debug.Log("裏ボスHME撃破！");
        ScoreManagerSc.Instance.GetPlayerBonus();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_TIME);
        SoundManagerSc.Instance.StopBGM();
        SceneLoader.ChangeScene(Scenes.RESULT);
    }
}
