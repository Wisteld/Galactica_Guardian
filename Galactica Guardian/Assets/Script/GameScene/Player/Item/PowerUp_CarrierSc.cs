using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PowerUp_CarrierSc : MonoBehaviour
{
    [Header ("生成するPrefab")]
    [SerializeField] GameObject PowerUp_Weapon; // パワーアップ：ウェポン.
    [SerializeField] GameObject PowerUp_Speed;  // パワーアップ：スピード.

    float carrierSpeed; // 移動速度.
    int random;         // 乱数.

    void Start()
    {
        carrierSpeed = Com.CARRIER_SPEED; // 速度を初期化.
        random = Random.Range(0,10);      // 乱数を生成.
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3 (0, carrierSpeed * Time.deltaTime); // 下方向に移動.
    }

    /// <summary>
    /// 自壊処理.
    /// </summary>
    void Destroy()
    {
        Destroy(gameObject); // 自身を削除.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tags.PLAYER)) // プレイヤーに衝突したら.
        {
            Instantiate(PowerUp_Speed,transform.position,transform.rotation); // パワーアップ：スピードアップ生成.
            Destroy();
        }
        if (collision.CompareTag(tags.PLAYER_BULLET)) // プレイヤーの通常弾に当たったら.
        {
            Instantiate(PowerUp_Weapon, transform.position, transform.rotation); // パワーアップ：ウェポン生成.
            Destroy();
        }
        if (collision.CompareTag(tags.PLAYER_BULLET_LASER)) // プレイヤーのレーザー弾に当たったら.
        {
            if (random > 2)
            {
                Instantiate(PowerUp_Weapon, transform.position, transform.rotation); // パワーアップ：ウェポン生成.
            }
            else
            {
                Instantiate(PowerUp_Speed, transform.position, transform.rotation); // パワーアップ：スピードアップ生成.
            }
            Destroy();
        }
    }
}
