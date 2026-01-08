using UnityEngine.SceneManagement;

public class SceneLoader
{
    /// <summary>
    /// シーン読み込み.
    /// </summary>
    /// <param name="scene">遷移先のシーン.</param>
    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// シーン読み込み(非同期).
    /// </summary>
    /// <param name="scene">遷移先のシーン.</param>
    public static void ChangeSceneAsync(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    /// <summary>
    /// 現在のシーンをリロード.
    /// </summary>
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}