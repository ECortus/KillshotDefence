using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavesGenerator : MonoBehaviour
{
    private LevelWaves LevelWaves => LevelManager.Instance.ActualLevel.LevelWaves;

    private List<EnemyWave> Waves => LevelWaves.Waves;
    private float DelayBetweenEnemies => LevelWaves.DelayBetweenEnemies;
    private float DelayBetweenSlots => LevelWaves.DelayBetweenSlots;
    private float DelayBetweenWaves => LevelWaves.DelayBetweenWaves;

    public bool AllDied
    {
        get
        {
            bool value = true;
            foreach(Enemy enemy in EnemiesPool)
            {
                if(!enemy.Died)
                {
                    value = false;
                    break;
                }
            }
            return value;
        }
    }
    public List<Enemy> EnemiesPool = new List<Enemy>();

    private Vector3 SpawnPoint 
    {
        get
        {
            Vector3 point;
            Vector3 center = transform.position;

            point = center + new Vector3(
                Random.Range(-transform.localScale.x / 2f, transform.localScale.x / 2f),
                0f,
                Random.Range(-transform.localScale.z / 2f, transform.localScale.z / 2f)
            );
            point.y = 0f;

            return point;
        }
    }

    private Vector3 SpawnRotation 
    {
        get
        {
            return transform.eulerAngles;
        }
    }

    void AddToPool(Enemy enm)
    {
        EnemiesPool.Add(enm);
    }
    
    public bool isWorking => coroutine != null;
    Coroutine coroutine;

    public void On()
    {
        if(coroutine == null && LevelWaves.isGenerate) coroutine = StartCoroutine(Work());
        
        if(!LevelWaves.isGenerate)
        {
            foreach(Enemy enemy in EnemiesPool)
            {
                enemy.On();
            }
        }
    }

    public void Off()
    {
        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }

    IEnumerator Work()
    {
        WaitForSeconds waitEnemy = new WaitForSeconds(DelayBetweenEnemies);
        WaitForSeconds waitSlot = new WaitForSeconds(DelayBetweenSlots - DelayBetweenEnemies);
        WaitForSeconds waitWave = new WaitForSeconds(DelayBetweenWaves - DelayBetweenSlots - DelayBetweenEnemies);

        int t = 0;

        foreach(EnemyWave wave in Waves)
        {
            if(t > 0) yield return waitWave;
            foreach(EnemySlot slot in wave.Enemies)
            {
                if(t > 0) yield return waitSlot;
                for(int i = 0; i < slot.Count; i++)
                {
                    if(t > 0) yield return waitEnemy;
                    
                    Spawn(slot.Enemy);
                    t++;
                }
            }
        }
        yield return null;

        Off();
    }

    void Spawn(Enemy enemy)
    {
        foreach(Enemy enm in EnemiesPool)
        {
            if(enm == null || enm.Type != enemy.Type) continue;

            if(!enm.Active) 
            {
                enm.On(SpawnPoint, SpawnRotation);
                return;
            }
        }

        Enemy scr = Instantiate(enemy);
        scr.On(SpawnPoint, SpawnRotation);
        AddToPool(scr);
    }

    public void ResetAllEnemies()
    {
        if(EnemiesPool.Count > 0)
        {
            foreach(Enemy enemy in EnemiesPool)
            {
                enemy.Off();
            }
        }
    }
}
