using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BaseSc : MonoBehaviour
{
    [SerializeField] GameObject enemy_prefab;

    float popTimer;
    // Start is called before the first frame update
    void Start()
    {
        popTimer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        popTimer -= Time.deltaTime;

        if ( popTimer < 0)
        {
            popTimer = 5;
            Instantiate(enemy_prefab);
        }
    }
}
