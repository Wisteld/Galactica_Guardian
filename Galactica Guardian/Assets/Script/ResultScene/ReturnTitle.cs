using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnTitle : MonoBehaviour
{
    [SerializeField] float wait_time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Title());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Title()
    {
        yield return new WaitForSeconds(wait_time);
        SceneLoader.ChangeScene(Scenes.TITLE);
    }
}
