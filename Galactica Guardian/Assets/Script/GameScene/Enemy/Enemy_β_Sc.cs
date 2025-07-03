using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

public class Enemy_β_Sc : MonoBehaviour
{
    [Header("誘導ミサイルのPrefab")]
    [SerializeField] GameObject missile_prefab;
    #region 変数.
    float attackTime;       // エネミーの攻撃間隔.
    float enemySpeed;       // エネミーの移動速度.
    int enemyHp;            // エネミーの体力.
    int enemyAttackCount;   // 攻撃した回数.

    Vector3 enemyPos;       // エネミーの現在座標.
    float eSize;            // エネミーサイズ.
    Vector2 min;
    Vector2 max;

    bool debugFlag = Com.DEBUG_MODE_ENEMY;         // デバッグモード.
    #endregion

    #region 初期化関数.
    /// <summary>
    /// 変数の初期化.
    /// </summary>
    void InitEnemy()
    {
        attackTime = Com.ENEMY_FIRE_RATE_β;
        enemySpeed = Com.ENEMY_SPEED_β;
        enemyHp = Com.ENEMY_HP_β;
        enemyAttackCount = 0;
    }

    void InitEnemySize()
    {
        eSize = GetComponent<BoxCollider2D>().size.x;
        min = Camera.main.ViewportToWorldPoint(Vector2.zero); // 画面の左下を取得.
        max = Camera.main.ViewportToWorldPoint(Vector2.one); // 画面の右上を取得.
    }
    #endregion

    void Start()
    {
        InitEnemy();
        InitEnemySize();
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = transform.position; // 現在位置を取得.

        if(enemyAttackCount < 3)
        {
            attackTime -= Time.deltaTime;
        }
        else
        {
            EnemyReturn();
            return;
        }

        EnemyMove();
        
        EnemyFire();
    }

    #region Update内関数.

    /// <summary>
    /// 移動処理.
    /// </summary>
    void EnemyMove()
    {
        if (enemyPos.y > max.y / 2)
        {
            transform.position -= new Vector3(0, enemySpeed * Time.deltaTime);

            if (debugFlag)
            {
                Debug.Log(enemyPos.y + "β");
            }
            EnemyRange();
        }
    }

    /// <summary>
    /// ミサイル発射処理.
    /// </summary>
    void EnemyFire()
    {
        if (attackTime < 0)
        {
            attackTime = Com.ENEMY_FIRE_RATE_β;
            Instantiate(missile_prefab, transform.position, transform.rotation);
            enemyAttackCount++;
        }
    }

    /// <summary>
    /// 斜め後方に離脱する(左右のより近い方向に離脱)
    /// </summary>
    void EnemyReturn()
    {
        if (transform.position.x < 0)
        {
            transform.position += new Vector3(-enemySpeed * Time.deltaTime, enemySpeed * Time.deltaTime);
        }
        else
        {
            transform.position += new Vector3(enemySpeed * Time.deltaTime, enemySpeed * Time.deltaTime);
        }
        EnemyRange();
    }

    #endregion

    #region Update外関数.

    void EnemyRange()
    {
        #region 画面端処理.
        if (enemyPos.x >= max.x + eSize) // 右端の判定.
        {
            EnemyDestroy();
            if (debugFlag)
            {
                Debug.Log("Enemy_Right");
            }
        }
        if (enemyPos.x <= min.x - eSize) // 左端の判定.
        {
            EnemyDestroy();
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

    /// <summary>
    /// ダメージ処理.
    /// </summary>
    /// <param name="damage">ダメージ数値</param>
    void EnemyDamage(int damage)
    {
        enemyHp -= damage;

        if (enemyHp <= 0)
        {
            EnemyDestroy();
        }
    }

    /// <summary>
    /// 被撃墜処理.
    /// </summary>
    void EnemyDestroy()
    {
        InitEnemy();
        EnemyPool.Instance.Collect(ENum.ENEMY_β, gameObject);
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
