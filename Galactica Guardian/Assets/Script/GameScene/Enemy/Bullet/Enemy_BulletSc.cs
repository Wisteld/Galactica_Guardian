using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ObjectPool;

public class Enemy_BulletSc : MonoBehaviour
{
    float bulletSpeed; // ’e‘¬.
    float bulletTime;  // ’e‚ªÁ‚¦‚é‚Ü‚Å‚ÌŠÔ.
    // Start is called before the first frame update
    void OnEnable()
    {
        bulletSpeed = Com.ENEMY_BULLET_SPEED; // ’e‘¬‚ğ‰Šú‰».
        bulletTime = Com.ENEMY_BULLET_DELETE; // ’e‚ªÁ‚¦‚é‚Ü‚Å‚ÌŠÔ‚ğ‰Šú‰».
    }

    // Update is called once per frame
    void Update()
    {
        bulletTime -= Time.deltaTime;
        transform.position -= new Vector3(0, bulletSpeed * Time.deltaTime); // ’e‚ÌˆÚ“®ˆ—.

        if (bulletTime < 0)
        {
            Delete();
        }
    }

    void Delete()
    {
        BulletPool.Instance.Collect(gameObject, Bullets.B_Type.ENEMY_BULLET);
    }
}
