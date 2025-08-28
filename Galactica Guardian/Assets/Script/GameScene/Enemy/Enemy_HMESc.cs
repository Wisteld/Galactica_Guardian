using Common;
using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HMESc : Enemy_BaseSc
{
    [Header("弾のPrefab")]
    [SerializeField] GameObject enemy_bullet;
    [SerializeField] GameObject enemy_bullet_lock;
    [SerializeField] GameObject enemy_missile;
    [Header("弾の発射位置")]
    [SerializeField] Transform fire_point_center;
    [SerializeField] Transform fire_point_left;
    [SerializeField] Transform fire_point_right;
    #region 変数.
    float attackTime;       // エネミーの攻撃間隔.
    float enemySpeed;       // エネミーの移動速度.
    float enemySideSpeed;   // エネミーの横移動速度.
    float sideTime;         // 横移動するか抽選する間隔.
    int enemySide;          // エネミーの横移動の有無.
    int enemyHp;            // エネミーの体力.

    Vector3 enemyPos;       // エネミーの現在座標.
    Vector2 min;            // 画面の左下.
    Vector2 max;            // 画面の右上.
    float eSize;            // エネミーサイズ.
    float sizeDistance;     // 壁との距離(サイズに対する倍率)

    bool sideFlag;          // 横移動したか.
    bool bottomFlag;        // 画面の下まで移動したか.
    bool debugFlag;         // デバッグモード.
    #endregion

    #region 初期化関数.
    /// <summary>
    /// 変数の初期化.
    /// </summary>
    void InitEnemy()
    {
        // 変数を初期化.
        enemySpeed = Com.ENEMY_SPEED_HME;
        enemySideSpeed = Com.ENEMY_SIDE_SPEED_HME;
        attackTime = Com.ENEMY_FIRE_RATE_HME;
        debugFlag = Com.DEBUG_MODE_ENEMY;
        sideTime = Com.ENEMY_SIDE_TIME;
        enemySide = 0;
        enemyHp = Com.ENEMY_HME_HP;
        sideFlag = false;
        bottomFlag = false;
    }
    /// <summary>
    /// エネミーの大きさを取得.
    /// </summary>
    void InitEnemySize()
    {
        eSize = GetComponent<BoxCollider2D>().size.x / 2;
        sizeDistance = Com.ENEMY_DISTANCE;
        min = GameManagerSc.Instance.screenMin;
        max = GameManagerSc.Instance.screenMax;
    }
    #endregion

    #region UnityEvent
    void Start()
    {
        InitEnemy();

        InitEnemySize();
    }

    // Update is called once per frame
    void Update()
    {
        attackTime -= Time.deltaTime; // 次に攻撃するまでのカウントダウン.

        if (!sideFlag)
        {
            sideTime -= Time.deltaTime; 　// 横移動の抽選をするまでのカウントダウン.
        }

        if (attackTime <= 0 && transform.position.y > max.y / 2) // カウントが0かつ画面の上側に居れば.
        {
            EnemyFire();
        }

        if (sideTime <= 0) // カウントが0になったら.
        {
            enemySide = Random.Range(-1, 2); // ランダムに左右に移動するか決める.
            sideTime = Com.ENEMY_SIDE_TIME;  // カウントリセット.
            // sideFlag = true;
        }

        if (transform.position.y < max.y - eSize　&& bottomFlag)
        {
            transform.position -= new Vector3(enemySide * enemySideSpeed * Time.deltaTime, -enemySpeed * Time.deltaTime);
            EnemyRange();
            return;
        }
        else if (bottomFlag)
        {
            bottomFlag = false;
        }

        EnemyMove();
    }

    #endregion

    #region Update内関数.
    /// <summary>
    /// エネミー移動.
    /// </summary>
    void EnemyMove()
    {
        transform.position -= new Vector3(enemySide * enemySideSpeed * Time.deltaTime, enemySpeed * Time.deltaTime);

        EnemyRange();
    }

    /// <summary>
    /// エネミー攻撃.
    /// </summary>
    void EnemyFire()
    {
        if (enemy_bullet == null || enemy_bullet_lock == null || enemy_missile == null) // Prefabがセットされていなかったら止める.
        {
            Debug.LogWarning("Enemy Bullet prefab is not assigned!");
            return;
        }
        Instantiate(enemy_bullet_lock, fire_point_center.position, Quaternion.identity); // 弾を撃つ.
        Instantiate(enemy_bullet, fire_point_left.position, Quaternion.identity);        // 弾を撃つ.
        Instantiate(enemy_bullet, fire_point_right.position, Quaternion.identity);       // 弾を撃つ.
        Instantiate(enemy_missile, fire_point_left.position, Quaternion.identity);       // ミサイルを撃つ.
        Instantiate(enemy_missile, fire_point_right.position, Quaternion.identity);      // ミサイルを撃つ.
        attackTime = Com.ENEMY_FIRE_RATE_HME;
    }

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
            bottomFlag = true;
            if (debugFlag)
            {
                Debug.Log("Enemy_loop");
            }
        }
        #endregion
    }

    #endregion

    #region Update外関数.

    /// <summary>
    /// ダメージ処理.
    /// </summary>
    /// <param name="damage">ダメージ数値</param>
    void EnemyDamage(int damage)
    {
        enemyHp -= damage;

        if (enemyHp <= 0)
        {
            ScoreManagerSc.Instance.UpdateScore(Score.SCORE_ENEMY_HME);
            EnemyDeath();
            EnemyDestroy();
        }
    }

    /// <summary>
    /// 被撃墜処理.
    /// </summary>
    void EnemyDestroy()
    {
        InitEnemy();
        EnemyPool.Instance.Collect(ENum.E_Type.ENEMY_HME, gameObject);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.PLAYER_BULLET))
        {
            BulletPool.Instance.Collect(collision.gameObject, Bullets.B_Type.PLAYER_BULLET);
            EnemyDamage(Com.ENEMY_DAMAGE_BULLET);
        }

        if (collision.CompareTag(Tags.PLAYER_BULLET_LASER))
        {
            EnemyDamage(Com.ENEMY_DAMAGE_BULLET);
        }

        if (collision.CompareTag(Tags.PLAYER_MISSILE))
        {
            EnemyDamage(Com.ENEMY_DAMAGE_MISSILE);
        }
    }
}
