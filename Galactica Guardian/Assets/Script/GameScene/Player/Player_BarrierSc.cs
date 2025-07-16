using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Threading;
using System;

public class Player_BarrierSc : MonoBehaviour
{
    [Header ("当たり判定")]
    [SerializeField] BoxCollider2D barrier_collider; // 当たり判定.
    [SerializeField] AudioClip clip_barrier_guard; // 弾を防いだ時の効果音.
    int barrierHp;      // 耐久値.
    int barrietLowHp;   // 低HPライン.
    Animator animator;  // アニメーター.
    PlayerSc parentSc;    // プレイヤー(親)のスクリプト.
    SpriteRenderer sr;
    public Action<bool> onBarrierBreak;

    // bool lowHpFlag = false; // 低HPフラグ.
    bool debugFlag = Com.DEBUG_MODE_PLAYER; // デバッグモード.
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SearchPlayer();
        animator = GetComponent<Animator>(); // アニメーターをセット.
        barrierHp = Com.PLAYER_BARRIER_HP;   // 耐久値を初期化.
        barrietLowHp = barrierHp / 4;        // 低耐久ラインを最大値の1/4に.
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Playerを探して親子関係(子)になる.
    /// </summary>
    void SearchPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Playerタグの付いたオブジェクトを探す.
        if (player != null) // プレイヤーが見つかったら.
        {
            transform.SetParent(player.transform);  // プレイヤーと親子関係(子)になる.
            transform.localPosition = Vector3.zero; // プレイヤーの中心に配置（必要に応じて調整）
            parentSc = GetComponentInParent<PlayerSc>();
            parentSc.SetBarrierRender(sr);
            if (parentSc == null)
            {
                Debug.LogWarning("PlayerSc not found!");
            }
        }
        else // もし見つからなかったら.
        {
            Debug.LogWarning("Player not found!"); 
        }
    }

    /// <summary>
    /// バリアを消滅させる.
    /// </summary>
    void BarrierDestroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 消滅アニメーション終了を検知.
    /// </summary>
    void OnBreakAnimationEnd()
    {
        onBarrierBreak?.Invoke(true);
        parentSc.BarrierLost();
        BarrierDestroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy_Bullet"))
        {
            Destroy(collision.gameObject);
            barrierHp--;
            SoundManagerSc.Instance.PlaySE(clip_barrier_guard);

            if (barrierHp <= barrietLowHp) // バリアの耐久が低くなったら.
            {
                animator.SetBool("LowHP", true);
            }

            if (barrierHp <= 0)
            {
                animator.SetBool("BarrierBreak", true);
            }

            if (debugFlag)
            {
                Debug.Log("BarrierHP" +  barrierHp);
            }
        }
    }
}
