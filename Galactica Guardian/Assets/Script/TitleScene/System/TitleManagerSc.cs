using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Common;

public class TitleManagerSc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneLoader.ChangeScene(Scenes.GAME);
    }
}
