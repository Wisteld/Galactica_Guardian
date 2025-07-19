using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ObjectPool;
using E_Type = Common.ENum.E_Type;

public class PowerUp_CarrierSc : Enemy_BaseSc
{
    [Header ("生成するPrefab")]
    [SerializeField] GameObject PowerUp_Weapon; // パワーアップ：ウェポン.
    [SerializeField] GameObject PowerUp_Speed;  // パワーアップ：スピード.

    float carrierSpeed; // 移動速度.
    Vector3 carrierPos;       // エネミーの現在座標.
    Camera cam;             // メインカメラの範囲.
    Vector2 min;            // 画面の左下.
    Vector2 max;            // 画面の右上.
    float cSize;            // エネミーサイズ.
    float sizeDistance;     // 壁との距離(サイズに対する倍率)

    bool isDestroyed;       // 撃墜済みか判定(二重処理対策).
    bool debugFlag = Com.DEBUG_MODE_ENEMY;

    void Init()
    {
        carrierSpeed = Com.CARRIER_SPEED; // 速度を初期化.
        isDestroyed = false;
    }

    void InitCarrierSize()
    {
        cSize = GetComponent<BoxCollider2D>().size.x / 2;
        sizeDistance = Com.ENEMY_DISTANCE;
        min = GameManagerSc.Instance.screenMin;
        max = GameManagerSc.Instance.screenMax;
    }

    private void OnEnable()
    {
        Init();
    }

    void Start()
    {
        InitCarrierSize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3 (0, carrierSpeed * Time.deltaTime); // 下方向に移動.
        CarrierRange();
    }
    
    void CarrierRange()
    {
        carrierPos = gameObject.transform.position; // 現在位置を取得.
        #region 画面端から出ないようにする処理.
        if (carrierPos.x >= max.x - cSize * sizeDistance) // 右端の判定.
        {
            transform.position = new Vector3(max.x - cSize * sizeDistance, transform.position.y, 0); // 端から出ないようにする.
            if (debugFlag)
            {
                Debug.Log("Enemy_Right");
            }
        }
        if (carrierPos.x <= min.x + cSize * sizeDistance) // 左端の判定.
        {
            transform.position = new Vector3(min.x + cSize * sizeDistance, transform.position.y, 0); // 端から出ないようにする.
            if (debugFlag)
            {
                Debug.Log("Enemy_Left");
            }
        }
        // 画面外判定.
        if (carrierPos.y <= min.y - cSize) // 下端の判定.
        {
            Destroy();
            if (debugFlag)
            {
                Debug.Log("Enemy_Lost");
            }
        }
        #endregion
    }

    /// <summary>
    /// 自壊処理.
    /// </summary>
    void Destroy()
    {
        isDestroyed = true;
        EnemyDeath();
        EnemyPool.Instance.Collect(E_Type.CARRIER, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed) return;
        if (collision.CompareTag(tags.PLAYER)) // プレイヤーに衝突したら.
        {
            Instantiate(PowerUp_Speed, transform.position, transform.rotation); // パワーアップ：スピードアップ生成.
            Destroy();
        }
        if (collision.CompareTag(tags.PLAYER_BULLET)) // プレイヤーの通常弾に当たったら.
        {
            Instantiate(PowerUp_Weapon, transform.position, transform.rotation); // パワーアップ：ウェポン生成.
            Destroy();
        }
        if (collision.CompareTag(tags.PLAYER_BULLET_LASER)) // プレイヤーのレーザー弾に当たったら.
        {
            Instantiate(PowerUp_Weapon, transform.position, transform.rotation); // パワーアップ：ウェポン生成.
            Destroy();
        } 
    }
}
