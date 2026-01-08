#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;

public static class WebGLTouchDetector
{
    [DllImport("__Internal")]
    private static extern bool IsWebGLTouchDevice();

    public static bool IsTouchDevice()
    {
        return IsWebGLTouchDevice();
    }
}
#endif