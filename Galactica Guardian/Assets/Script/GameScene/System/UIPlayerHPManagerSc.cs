using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHPManagerSc : MonoBehaviour
{
    [SerializeField] Image[] hp_segments;   // 並べたHPアイコン
    [SerializeField] Sprite[] full_sprites;    // [0]=左, [1]=中, [2]=右
    [SerializeField] Sprite[] empty_sprites;   // [0]=左, [1]=中, [2]=右


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
        for (int i = 0; i < hp_segments.Length; i++)
        {
            hp_segments[i].sprite = (i < currentHP) ? full_sprites[i] : empty_sprites[i];
        }
    }
}
