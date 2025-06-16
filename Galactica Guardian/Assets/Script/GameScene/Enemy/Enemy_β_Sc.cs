using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_β_Sc : MonoBehaviour
{
    [Header("誘導ミサイルのPrefab")]
    [SerializeField] GameObject missile_prefab;
    #region 変数.
    float attackTime;       // エネミーの攻撃間隔.
    float enemySpeed;       // エネミーの移動速度.
    float enemySideSpeed;   // エネミーの横移動速度.
    int enemyHp;            // エネミーの体力.

    Vector3 enemyPos;       // エネミーの現在座標.
    Camera cam;             // メインカメラの範囲.
    float eSize;            // エネミーサイズ.
    float sizeDistance;     // 壁との距離(サイズに対する倍率)

    bool debugFlag;         // デバッグモード.
    #endregion

    #region 初期化関数.
    /// <summary>
    /// 変数の初期化.
    /// </summary>
    void InitEnemy()
    {
        //attackTime = ;
        enemySpeed = Com.ENEMY_SPEED_β;
        //enemySideSpeed = ;
        enemyHp = Com.ENEMY_HP_β;
    }
    #endregion

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            EnemyDestroy();
        }
    }

    void EnemyDestroy()
    {
        Destroy(gameObject);
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
