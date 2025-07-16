using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ObjectPool;
using Effect_Type = Common.Effects.Effect_Type;
using System.Drawing;

public class EnemySc : Enemy_BaseSc
{
    [Header ("弾のプレファブ")]
    [SerializeField] GameObject enemy_bullet;
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
    Vector2 min;            // 画面の左下.
    Vector2 max;            // 画面の右上.
    float eSize;            // エネミーサイズ.
    float sizeDistance;     // 壁との距離(サイズに対する倍率)
    #endregion
    #region フラグ.
    bool sideFlag;          // 横移動したか.
    bool debugFlag;         // デバッグモード.
    #endregion

    #region 初期化関数.
    /// <summary>
    /// 変数の初期化.
    /// </summary>
    void InitEnemy()
    {
        // 変数を初期化.
        enemySpeed = Com.ENEMY_SPEED;
        enemySideSpeed = Com.ENEMY_SIDE_SPEED;
        attackTime = Random.Range(Com.ENEMY_FIRE_RND_MIN, Com.ENEMY_FIRE_RND_MAX); // ランダムに初期値を設定.
        debugFlag = Com.DEBUG_MODE_ENEMY;
        sideTime = Com.ENEMY_SIDE_TIME;
        enemyHp = Com.ENEMY_HP;
        enemySide = 0;
        rndFire = 0;
        sideFlag = false;
        EnemyType = ENum.E_Type.ENEMY_NORMAL;
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

    void Update()
    {
        attackTime -= Time.deltaTime; // 次に攻撃するまでのカウントダウン.

        if (!sideFlag)
        {
            sideTime -= Time.deltaTime; 　// 横移動の抽選をするまでのカウントダウン.
        }

        if (attackTime <= 0) // カウントが0になったら.
        {
            EnemyFire();
        }

        if (sideTime <= 0) // カウントが0になったら.
        {
            enemySide = Random.Range(-1, 2); // ランダムに左右に移動するか決める.
            sideTime = Com.ENEMY_SIDE_TIME;  // カウントリセット.
            sideFlag = true;
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
        if (enemy_bullet == null) // Prefabがセットされていなかったら止める.
        {
            Debug.LogWarning("Enemy Bullet prefab is not assigned!");
            return;
        }
        
        if (rndFire == 0)
        {
            Instantiate(enemy_bullet, transform.position, Quaternion.identity); // 弾を撃つ.
        }

        rndFire = Random.Range(0, 6); // 二発目以降、6/1で弾が出るようにする.
        attackTime = Random.Range(Com.ENEMY_FIRE_RND_MIN, Com.ENEMY_FIRE_RND_MAX); // 再攻撃までにかかる時間をランダムに設定.

        if (debugFlag)
        {
            Debug.Log("eAttackTime" + attackTime);
            Debug.Log("rndFire" + rndFire);
        }
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
            EnemyDestroy();
            if (debugFlag)
            {
                Debug.Log("Enemy_Lost");
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
            EffectPool.Instance.Generate(Effect_Type.EFFECT_EXPLOSION, transform.position);
            EnemyDestroy();
        }
    }

    /// <summary>
    /// 被撃墜・撤退処理.
    /// </summary>
    void EnemyDestroy()
    {
        EnemyDeath();
        InitEnemy();
        EnemyPool.Instance.Collect(ENum.E_Type.ENEMY_NORMAL, gameObject);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tags.PLAYER_BULLET))
        {
            Destroy(collision.gameObject);
            EnemyDamage(Com.ENEMY_DAMAGE_BULLET);
        }
        if (collision.CompareTag(tags.PLAYER_BULLET_LASER))
        {
            EnemyDamage(Com.ENEMY_DAMAGE_BULLET);
        }

        if (collision.CompareTag(tags.PLAYER_MISSILE))
        {
            EnemyDamage(Com.ENEMY_DAMAGE_MISSILE);
        }
    }
}
