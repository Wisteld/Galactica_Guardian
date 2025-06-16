using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet_LockOnSc : MonoBehaviour
{
    Transform playerTransform; // プレイヤーの座標.
    float bulletSpeed; // 弾速.
    float bulletTime;  // 弾が消えるまでの時間.
    float ignoreRange; // プレイヤーを無視する距離.
    Vector3 direction; // 発射方向.
    // Start is called before the first frame update
    void Start()
    {
        ignoreRange = Com.ENEMY_IGNORE_RANGE; // プレイヤーを無視する距離を初期化.
        SearchPlayer();
        bulletSpeed = Com.ENEMY_BULLET_SPEED / 2; // 弾速を初期化.
        bulletTime = Com.ENEMY_BULLET_DELETE * 2; // 弾が消えるまでの時間を初期化.
    }

    // Update is called once per frame
    void Update()
    {
        bulletTime -= Time.deltaTime;
        transform.position -= direction * bulletSpeed * Time.deltaTime; // 弾の移動処理.

        if (bulletTime < 0)
        {
            Delete();
        }
    }

    /// <summary>
    /// プレイヤーを探して方向ベクトルを取得する.
    /// </summary>
    void SearchPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // プレイヤーを探す.
        
        if (player == null) // プレイヤーが見つからなかったら.
        {
            direction = Vector2.down;
            return;
        }

        if (playerTransform.position.y < playerTransform.position.y - ignoreRange) // プレイヤーが一定以上下に居たら.
        {
            direction = (playerTransform.position - transform.position).normalized; // プレイヤーの座標への方向ベクトルを取得.
        }
        else // 距離が近かったら.
        {
            direction = Vector2.down;
        }
    }

    /// <summary>
    /// 削除.
    /// </summary>
    void Delete()
    {
        Destroy(gameObject);
    }
}
