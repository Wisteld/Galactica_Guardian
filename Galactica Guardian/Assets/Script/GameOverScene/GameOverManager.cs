using System.Collections;
using UnityEngine;
using Common;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] AudioClip clip_gameover;

    void Start()
    {
        StartCoroutine(ReturnTitle());
    }

    IEnumerator ReturnTitle()
    {
        SoundManagerSc.Instance.PlayBGM(clip_gameover, false);
        yield return new WaitForSeconds(clip_gameover.length);

        SceneLoader.ChangeScene(Scenes.TITLE);
    }
}
