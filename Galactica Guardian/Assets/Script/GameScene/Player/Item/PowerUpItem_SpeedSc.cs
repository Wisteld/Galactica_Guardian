using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PowerUpItem_SpeedSc : MonoBehaviour
{
    float itemSpeed; // ˆÚ“®‘¬“x.

    void Start()
    {
        itemSpeed = Com.POWERUP_SPEED_SPEED; // ‘¬“x‚ğ‰Šú‰».
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, itemSpeed * Time.deltaTime); // ‰º‚ÉˆÚ“®‚·‚é.
    }
}
