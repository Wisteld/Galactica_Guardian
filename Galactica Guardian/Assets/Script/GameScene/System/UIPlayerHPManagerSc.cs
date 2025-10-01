using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHPManagerSc : MonoBehaviour
{
    [SerializeField] Image[] hpSegments;   // 並べたHPアイコン
    [SerializeField] Sprite[] fullSprites;    // [0]=左, [1]=中, [2]=右
    [SerializeField] Sprite[] emptySprites;   // [0]=左, [1]=中, [2]=右


    void Start()
    { 
        PlayerSc.Instance.PlayerHPChange += UpdateHPUI;
        UpdateHPUI(PlayerSc.Instance.playerHp); // 初期表示
    }

    void OnDestroy()
    {
        PlayerSc.Instance.PlayerHPChange -= UpdateHPUI;
    }

    void UpdateHPUI(int currentHP)
    {
        for (int i = 0; i < hpSegments.Length; i++)
        {
            hpSegments[i].sprite = (i < currentHP) ? fullSprites[i] : emptySprites[i];
        }
    }
}
