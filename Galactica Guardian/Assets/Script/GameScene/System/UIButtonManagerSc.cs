using UnityEngine;
using Common;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections;

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
        StartCoroutine(StartDelay());
    }

    void UpdateUIVisibility()
    {
        bool show = false;

#if UNITY_WEBGL && !UNITY_EDITOR
        // JavaScript連携でWebGLタッチデバイスを検出
        show = WebGLTouchDetector.IsTouchDevice();
#else
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
            show = true;
        else
            show = Input.touchSupported && SystemInfo.deviceType != DeviceType.Desktop;
#endif

        if (Com.DEBUG_MODE_SYSTEM)
            show = true;

        touchUI?.SetActive(show);
        onScreenStick?.SetActive(show);

        if (Com.DEBUG_MODE_SYSTEM)
            Debug.Log($"[UIButtonManagerSc] Platform:{Application.platform}, TouchSupported:{Input.touchSupported}, Show:{show}");
    }

    public void OnFirePressed()
    {
        if (Com.DEBUG_MODE_SYSTEM) { Debug.Log("Fire Button Push"); }
    }

    IEnumerator StartDelay()
    {
        yield return null; // InputSystem初期化待ち
        UpdateUIVisibility();
    }
}
