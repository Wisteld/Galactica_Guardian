using Common;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_BossSc : MonoBehaviour
{
    [Header("弾のPrefab")]
    [SerializeField] GameObject enemy_bullet;
    [SerializeField] GameObject enemy_bullet_lock;
    [SerializeField] GameObject enemy_missile;
    #region 変数.
    float attackTime;       // エネミーの攻撃間隔.
    float enemySpeed;       // エネミーの移動速度.
    float enemySideSpeed;   // エネミーの横移動速度.
    float sideTime;         // 横移動するか抽選する間隔.
    int enemySide;          // エネミーの横移動の有無.
    int rndFire;            // ランダムに二発目以降の弾を発射するか決める.
    int enemyHp;            // エネミーの体力.

    Vector3 enemyPos;       // エネミーの現在座標.
    Camera cam;             // メインカメラの範囲.
    float eSize;            // エネミーサイズ.
    float sizeDistance;     // 壁との距離(サイズに対する倍率)

    Vector2 max;
    Vector2 min;

    bool sideFlag;          // 横移動したか.
    bool entryFlag;         // 登場演出中か.
    bool debugFlag = Com.DEBUG_MODE_ENEMY;         // デバッグモード.
    #endregion

    #region 初期化関数.
    /// <summary>
    /// 変数の初期化.
    /// </summary>
    void InitEnemy()
    {
        // 変数を初期化.
        enemySpeed = Com.ENEMY_BOSS_SPEED;
        enemySideSpeed = Com.ENEMY_SIDE_SPEED;
        attackTime = Random.Range(Com.ENEMY_BOSS_FIRE_RND_MIN, Com.ENEMY_BOSS_FIRE_RND_MAX); // ランダムに初期値を設定.
        sideTime = Com.ENEMY_SIDE_TIME;
        enemySide = 0;
        rndFire = 0;
        enemyHp = Com.ENEMY_BOSS_HP;
        sideFlag = false;
        entryFlag = false;
    }
    /// <summary>
    /// エネミーの大きさを取得.
    /// </summary>
    void InitEnemySize()
    {
        eSize = GetComponent<BoxCollider2D>().size.x / 2;
        sizeDistance = Com.ENEMY_DISTANCE;
        min = Camera.main.ScreenToWorldPoint(Vector2.zero); // 画面の左下を取得.
        max = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)); // 画面の右上を取得.
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        InitEnemy();
        InitEnemySize();
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = transform.position;

        if (enemyPos.y > max.y / 2 && !entryFlag)
        {

        }
        else if (!entryFlag)
        {
            entryFlag = true;
        }
    }

    #region Update内関数.
    void EnemyRange()
    {
        enemyPos = gameObject.transform.position; // 現在位置を取得.
        #region 画面端から出ないようにする処理.
        if (enemyPos.x >= max.x - eSize * sizeDistance) // 右端の判定.
        {
            transform.position = new Vector3(max.x - eSize * sizeDistance, transform.position.y, 0); // 端から出ないようにする.
            enemySideSpeed = -enemySideSpeed;
            if (debugFlag)
            {
                Debug.Log("Enemy_Right");
            }
        }
        if (enemyPos.x <= min.x + eSize * sizeDistance) // 左端の判定.
        {
            transform.position = new Vector3(min.x + eSize * sizeDistance, transform.position.y, 0); // 端から出ないようにする.
            enemySideSpeed = -enemySideSpeed;
            if (debugFlag)
            {
                Debug.Log("Enemy_Left");
            }
        }
        // 画面外判定.
        if (enemyPos.y <= min.y - eSize) // 下端の判定.
        {
            Destroy(gameObject);
            if (debugFlag)
            {
                Debug.Log("Enemy_Lost");
            }
        }
        #endregion
    }
    #endregion

    #region Update外関数.

    #endregion
}
