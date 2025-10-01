using System.Collections.Generic;
using UnityEngine;
using E_Type = Common.ENum.E_Type;
using E_Anchor = Common.ENum.AnchorType;

[System.Serializable]
public class EnemySpawnData
{
    [Tooltip ("生成タイミング（Wave開始からの秒数）")]
    public float appearTime;

    [Tooltip ("生成座標アンカー")]
    public E_Anchor spawnPosition;

    [Tooltip ("生成するエネミーの種類")]
    public E_Type enemyType;
}

[System.Serializable]
public class WaveData
{
    public string waveName;
    public List<EnemySpawnData> spawns = new List<EnemySpawnData>();
    public AudioClip waveBGM;
    public bool isLoopBGM;
    public bool isBossWave;
    public bool isHMEWave;
}

[CreateAssetMenu(fileName = "WaveSetData", menuName = "Waves/Wave Set")]
public class WaveSetData : ScriptableObject
{
    public List<WaveData> waves = new List<WaveData>();
}