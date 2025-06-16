using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Player_LaserSc : MonoBehaviour
{
    float bulletSpeed;
    float deleteTime;
    float deleteCount;
    // Start is called before the first frame update
    void Start()
    {
        deleteTime = 0;
        bulletSpeed = Com.PLAYER_BULLET_SPEED;
        deleteCount = Com.PLAYER_BULLET_DELETE_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        deleteTime += Time.deltaTime; // ”­Ë‚³‚ê‚Ä‚©‚ç‚ÌŠÔ‚ğŒv‘ª.
        transform.position += new Vector3(0, bulletSpeed * Time.deltaTime); // ’e‚ÌˆÚ“®ˆ—.

        if (deleteTime > deleteCount) // ”­ËŒãˆê’èŠÔŒo‰ß‚µ‚½‚ç.
        {
            Destroy(gameObject); // ©g‚ğíœ.
        }
    }
}
