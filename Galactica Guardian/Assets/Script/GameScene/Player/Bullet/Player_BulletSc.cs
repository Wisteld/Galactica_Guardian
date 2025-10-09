using UnityEngine;
using Common;
using ObjectPool;

public class Player_BulletSc : MonoBehaviour
{
    float bulletSpeed;
    float deleteTime;
    float deleteCount;
    // Start is called before the first frame update
    void OnEnable()
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
            BulletPool.Instance.Collect(gameObject, Bullets.B_Type.PLAYER_BULLET); // ©g‚ğíœ.
        }
    }
}
