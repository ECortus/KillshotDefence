using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LevelWaves", menuName = "Level")]
public class LevelWaves : ScriptableObject
{
    public bool isGenerate = true;
    public float TimeToWin = 25f;
    public int Reward = 1000;

    public List<EnemyWave> Waves = new List<EnemyWave>();

    [Header("Generator par-s: ")]
    public int MaxInRow = 5;
    public float DelayBetweenEnemies = 10f;
    public float DelayBetweenSlots = 1f;
    /* public float DelayBetweenWaves = 10f; */
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemySlot> Enemies = new List<EnemySlot>();
}

[System.Serializable]
public class EnemySlot
{
    public Enemy Enemy;
    public int Count;
    public int Sector;
}
