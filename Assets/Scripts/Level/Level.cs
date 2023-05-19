using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

public class Level : MonoBehaviour
{
    public void On() => gameObject.SetActive(true);
    public void Off() => gameObject.SetActive(false);
    public void Eliminate() => Destroy(gameObject);

    public int moneyOnStart { get; set; }

    private Player player => GameManager.Instance.player;
    private PlayerHealth health => GameManager.Instance.health;
    private Shooting shooting => GameManager.Instance.shooting;
    private WeaponsInfoController weaponsInfo => GameManager.Instance.weaponsInfo;

    [Space]
    public LevelWaves LevelWaves;
    [SerializeField] private EnemyWavesGenerator Generator;

    public void StartLevel()
    {
        ResetLevel();

        moneyOnStart = Statistics.Money;

        Generator.On();
        GameManager.Instance.SetActive(true);
    }

    public void EndLevel()
    {
        Money.Plus(LevelWaves.Reward);

        LevelManager.Instance.AddOneToIndex();

        Generator.Off();
        UI.Instance.EndLevel();
        GameManager.Instance.SetActive(false);
    }

    public void NextLevel()
    {
        Generator.ResetAllEnemies();
        UI.Instance.NextLevel();
    }

    public void LoseLevel()
    {
        Generator.Off();
        UI.Instance.LoseLevel();
        GameManager.Instance.SetActive(false);
    }

    public void ResetLevel()
    {
        player.Reset();
        health.Restore();

        shooting.FullAmmoAllWeapons();
        shooting.Reset();
        weaponsInfo.Reset();

        Generator.Off();
        Generator.ResetAllEnemies();

        GameManager.Instance.SetActive(false);
    }

    public void IsLevelComplete()
    {
        if(!Generator.isWorking)
        {
            if(Generator.AllDied)
            {
                EndLevel();
            }
        }
    }
}
