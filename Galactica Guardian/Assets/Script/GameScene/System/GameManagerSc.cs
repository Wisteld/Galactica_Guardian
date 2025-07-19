using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    [Header ("生成するエネミー")]
    [SerializeField] GameObject enemy_prefab;
    [SerializeField] GameObject enemyα_prefab;
    [SerializeField] GameObject enemyβ_prefab;
    [SerializeField] GameObject enemy_hme_prefab;
    [SerializeField] GameObject enemy_boss_prefab;
    [SerializeField] GameObject item_carrier_prefab;
    [Header ("エネミー生成データ")]
    [SerializeField] List<WaveSetData> wave_data;
    [Header("生成するエフェクト")]
    [SerializeField] GameObject explosion_prefab;
    [SerializeField] GameObject explosion_min_prefab;
    #endregion
    #region Instance・変数.
    public static GameManagerSc Instance { get; private set; }
    public Vector2 screenMin { get; private set; }
    public Vector2 screenMax { get; private set; }

    Dictionary<AnchorType, Vector3> anchorPositions = new Dictionary<AnchorType, Vector3>();

    int currentWaveIndex;
    int score;
    int enemyKillCount;
    int activeEnemyCount;

    bool isWaveRunning = false;
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
        GameObject[] anchorObjects = GameObject.FindGameObjectsWithTag(tags.POP_ANCHOR); // tag指定でAnchorを配列取得.
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
        score = 0;
        enemyKillCount = 0;
    }
    #endregion

    #region UnityEvent.
    private void Awake()
    {
        EnemyPool.Instance.GenerateEnemy(enemy_prefab, enemyα_prefab, enemyβ_prefab,
            enemy_hme_prefab, enemy_boss_prefab, item_carrier_prefab); // エネミーオブジェクトプール準備.
        EffectPool.Instance.GenerateEffect(explosion_prefab, explosion_min_prefab); // エフェクトオブジェクトプール準備.

        InitCamera();
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
    #endregion

    public bool TryGetAnchor(AnchorType type, out Vector3 pos)
    {
        return anchorPositions.TryGetValue(type, out pos);
    }

    IEnumerator WaveRoutine()
    {
        for (int setIndex = 0; setIndex < wave_data.Count; setIndex++)
        {
            WaveSetData currentWaveSet = wave_data[setIndex];

            for (int waveIndex = 0; waveIndex < currentWaveSet.waves.Count; waveIndex++)
            {
                WaveData currentWaveData = currentWaveSet.waves[waveIndex];
                if (debugFlag)
                {
                    Debug.Log($"開始: Wave {currentWaveIndex + 1} / {wave_data.Count}");
                }

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

                yield return new WaitForSeconds(2f); // 次のWaveまでの待機
                currentWaveIndex++;
            }
        }

        // 全Wave終了後.
        Debug.Log("全Wave終了!");
        yield return new WaitForSeconds(1.5f);
        SceneLoader.ChangeScene(Scenes.RESULT);
    }

    IEnumerator SpawnEnemyWithDelay(EnemySpawnData spawnData)
    {
        yield return new WaitForSeconds(spawnData.appearTime);

        if (anchorPositions.TryGetValue(spawnData.spawnPosition, out Vector3 spawnPos))
        {
            GameObject enemy = EnemyPool.Instance.Generate(spawnData.enemyType, spawnPos);

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
}
