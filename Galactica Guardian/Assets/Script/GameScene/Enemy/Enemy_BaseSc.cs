using UnityEngine;
using System;

public class Enemy_BaseSc : MonoBehaviour
{
    // Start is called before the first frame update
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
