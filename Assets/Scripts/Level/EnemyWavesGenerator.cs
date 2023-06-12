using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavesGenerator : MonoBehaviour
{
    private LevelWaves LevelWaves => LevelManager.Instance.ActualLevel.LevelWaves;

    public List<EnemyWave> Waves => LevelWaves.Waves;
    private float DelayBetweenEnemies => LevelWaves.DelayBetweenEnemies;
    private float DelayBetweenSlots => LevelWaves.DelayBetweenSlots;
    /* private float DelayBetweenWaves => LevelWaves.DelayBetweenWaves; */

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

    private Vector3 SpawnDirection
    {
        get
        {
            float sector = Waves[Wave].Enemies[Slot].Sector;
            float count = Waves[Wave].Enemies[Slot].Count;
            int inRow = LevelWaves.MaxInRow;

            inRow = (int)(inRow > count ? count : inRow);
            sector = sector > sectorsCount - 1 ? sectorsCount - 1 : sector;

            float angle = -angleOfSector / 2 + angleOfSector * (sector / sectorsCount)
                + angleOfSector * (1 / sectorsCount) * ((Enemy % inRow) / (float)inRow);

            Vector3 dir = DirectionFromAngle(transform.eulerAngles.y, angle);
            dir += transform.forward * 0.025f * (Enemy / inRow);

            float value = 0.05f;
            dir += new Vector3(
                Random.Range(-value, value),
                0f,
                Random.Range(-value, value)
            );

            return dir;
        }
    }

    private Vector3 SpawnPoint 
    {
        get
        {
            Vector3 point;
            point = Center + SpawnDirection * radius;
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

    private Vector3 Center
    {
        get
        {
            return transform.position;
        }
    }

    [Header("Parameters: ")]
    [SerializeField] private float radius;
    [SerializeField] private float sectorsCount;
    [SerializeField] private float angleOfSector;

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

    [HideInInspector] public int Wave = -1;
    int Slot = -1;
    int Enemy = -1;

    IEnumerator Work()
    {
        WaitForSeconds waitEnemy = new WaitForSeconds(DelayBetweenEnemies);
        WaitForSeconds waitSlot = new WaitForSeconds(DelayBetweenSlots - DelayBetweenEnemies);
        /* WaitForSeconds waitWave = new WaitForSeconds(DelayBetweenWaves - DelayBetweenSlots - DelayBetweenEnemies); */

        int t = 0;
        Wave = -1;

        foreach(EnemyWave wave in Waves)
        {
            Wave++;
            Slot = -1;

            if(t > 0)
            {
                yield return new WaitUntil(() => AllDied);

                LevelManager.Instance.ActualLevel.Bonuses.On();
                yield return new WaitUntil(() => !LevelManager.Instance.ActualLevel.Bonuses.Active);
            }

            LevelManager.Instance.ActualLevel.waveCounter.Refresh();

            foreach(EnemySlot slot in wave.Enemies)
            {
                Slot++;
                Enemy = -1;
                if(t > 0) yield return waitSlot;

                for(int i = 0; i < slot.Count; i++)
                {
                    Enemy++;
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

    public void KillAllEnemies()
    {
        if(EnemiesPool.Count > 0)
        {
            foreach(Enemy enemy in EnemiesPool)
            {
                enemy.Death();
            }
        }
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(Center, radius);

        Gizmos.color = Color.red;
        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angleOfSector / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angleOfSector / 2);

        Gizmos.DrawLine(Center, Center + viewAngle01 * radius);
        Gizmos.DrawLine(Center, Center + viewAngle02 * radius);

        Gizmos.color = Color.blue;
        Vector3 angle;

        for(int i = 1; i < sectorsCount; i++)
        {
            angle = DirectionFromAngle(transform.eulerAngles.y, -angleOfSector / 2 + angleOfSector * (i / sectorsCount));
            Gizmos.DrawLine(Center, Center + angle * radius);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
