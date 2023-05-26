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
    
    [SerializeField] private LevelBonuses bonuses;

    [Space]
    public LevelWaves LevelWaves;
    [SerializeField] private EnemyWavesGenerator Generator;
    [SerializeField] private LevelTimer timer;

    public async void StartLevel()
    {
        ResetLevel();

        if(bonuses != null)
        {
            bonuses.On();
            await UniTask.WaitUntil(() => !bonuses.Active);
        }

        moneyOnStart = Statistics.Money;

        Generator.On();
        timer.On();
        GameManager.Instance.SetActive(true);
    }

    public void EndLevel()
    {
        if(!GameManager.Instance.isActive) return;

        if(bonuses != null) bonuses.RemoveBonus();
        timer.Off();
        Money.Plus(LevelWaves.Reward);

        Generator.KillAllEnemies();

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
        if(bonuses != null) bonuses.RemoveBonus();

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

        if(LevelManager.Instance.GetRealIndex() % LevelManager.Instance.LevelsOnBiom == 0) 
        {
            Debug.Log("reset biom");
            ResetBiom();
        }
        weaponsInfo.ResetOnLevel();

        Generator.Off();
        Generator.ResetAllEnemies();

        timer.Off();

        GameManager.Instance.SetActive(false);
    }

    public void ResetBiom()
    {
        /* weaponsInfo.ResetOnBiom(); */
        BonusSaving.CancelAllBonuses();
    }

    public void IsLevelComplete()
    {
        if(!GameManager.Instance.isActive) return;

        if(!Generator.isWorking)
        {
            if(Generator.AllDied)
            {
                EndLevel();
            }
        }
    }
}
