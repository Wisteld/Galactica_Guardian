using UnityEngine;
using System;
using Common;

public class Enemy_BaseSc : MonoBehaviour
{
    public ENum.E_Type EnemyType { get; protected set; }  // 生成時に代入される
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Action OnDeath; // 死亡通知（イベント）.

    /// <summary>
    /// 死亡時に呼ぶ。共通で通知だけ行う.
    /// </summary>
    protected void EnemyDeath()
    {
        OnDeath?.Invoke(); // 死亡通知コール.
    }
}
