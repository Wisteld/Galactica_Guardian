using UnityEngine;
using Common;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class UIButtonManagerSc : MonoBehaviour
{
    [Header("UIオブジェクト")]
    public GameObject touchUI;       // タッチ用ボタン.
    public GameObject onScreenStick; // On-Screen Stick.

    private void Awake()
    {
        // Enhanced Touch を有効化.
        EnhancedTouchSupport.Enable();
    }

    private void OnDestroy()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        // 確実にセットされているか確認.
        if (touchUI == null || onScreenStick == null)
        {
            Debug.LogWarning("TouchUI or OnScreenStick がセットされていません！");
            return;
        }
        UpdateUIVisibility();
    }

    /// <summary>
    /// 実行環境に応じてタッチUIを表示/非表示.
    /// </summary>
    void UpdateUIVisibility()
    {
        bool show = false;

#if UNITY_WEBGL
        // WebGLならタッチスクリーンがある場合のみ表示(タッチスクリーン付きPCの場合非表示).
        bool isTouchDevice = Touchscreen.current != null && 
                     SystemInfo.deviceType != DeviceType.Desktop;
        show = isTouchDevice;
#else
        // モバイルなら表示、PC/その他は非表示.
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            show = true;
        }
#endif
        if (Com.DEBUG_MODE_SYSTEM)
        {
            show = true;
        }

        if (touchUI != null) touchUI.SetActive(show);
        if (onScreenStick != null) onScreenStick.SetActive(show);

        if (Com.DEBUG_MODE_SYSTEM)
        {
            Debug.Log($"Debug_Mode_System:MobileUI_{show}");
        }
    }

    public void OnFirePressed()
    {
        if (Com.DEBUG_MODE_SYSTEM) { Debug.Log("Fire Button Push"); }
    }
}
