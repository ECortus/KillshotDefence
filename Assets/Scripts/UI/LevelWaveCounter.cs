using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelWaveCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level, waves;
    private EnemyWavesGenerator gen => LevelManager.Instance.ActualLevel.Generator;

    public void Refresh()
    {
        level.text = $"Level {LevelManager.Instance.GetIndex() + 1}";
        waves.text = $"Wave {gen.Wave + 1}/{gen.Waves.Count}";
    }
}
