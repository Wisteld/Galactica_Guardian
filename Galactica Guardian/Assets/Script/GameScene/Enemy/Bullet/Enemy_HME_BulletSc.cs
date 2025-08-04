using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HME_BulletSc : MonoBehaviour
{
    Transform playerTransform; // プレイヤーの座標.
    float bulletSpeed; // 弾速.
    float bulletTime;  // 弾が消えるまでの時間.
    float ignoreRange; // プレイヤーを無視する距離.
    Vector3 direction; // 発射方向.
    Vector3 toPlayer;
    // Start is called before the first frame update
    void Start()
    {
        ignoreRange = Com.ENEMY_IGNORE_RANGE; // プレイヤーを無視する距離を初期化.
        SearchPlayer();
        bulletSpeed = Com.ENEMY_HME_BULLET_SPEED; // 弾速を初期化.
        bulletTime = Com.ENEMY_BULLET_DELETE; // 弾が消えるまでの時間を初期化.
    }

    // Update is called once per frame
    void Update()
    {
        bulletTime -= Time.deltaTime;
        transform.position += direction * bulletSpeed * Time.deltaTime; // 弾の移動処理.
        Debug.DrawLine(transform.position, transform.position + direction * 3f, Color.red, 2.0f);

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
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER); // プレイヤーを探す.

        if (player == null) // プレイヤーが見つからなかったら.
        {
            Debug.LogWarning("Player Search Failed");
            direction = Vector2.down;
            return;
        }

        playerTransform = player.transform;
        toPlayer = playerTransform.position - transform.position;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance < ignoreRange || toPlayer.y > 0f) // プレイヤーが上に居るか、距離が近かったら.
        {
            direction = Vector2.down;
        }
        else // プレイヤーが一定以上下に居たら.
        {
            direction = (playerTransform.position - transform.position).normalized; // プレイヤーの座標への方向ベクトルを取得.
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
