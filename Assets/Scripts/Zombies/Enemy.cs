using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class Enemy : EnemyController
{
    public virtual EnemyType Type { get; }

    private float DefaultMaxHealth { get { return stats.DefaultMaxHealth; } }
    private float DefaultDamage { get { return stats.DefaultDamage; } }
    private UnityEvent DeathEvent { get { return stats.DeathEvent; } }
    
    public float MaxHealth => DefaultMaxHealth;

    private float _health = 0f;
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if(healthUI != null) healthUI.Refresh();
        }
    }

    public float Damage => DefaultDamage;

    [Header("Ref-s:")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private RagdollController ragdoll;
    [SerializeField] private ZombieHealthUI healthUI;

    public Vector3 Center
    {
        get
        {
            return transform.position + Vector3.up * 1f;
        }
    }

    public void On(Vector3 pos = new Vector3(), Vector3 rot = new Vector3())
    {
        /* ragdoll.Off(); */
        this.enabled = true;

        Restore();
        Activate(pos, rot);

        Died = false;
        ChangeTagMaskToActive();
    }

    public void Off()
    {
        Died = true;

        this.enabled = false;
        Deactivate();

        LevelManager.Instance.ActualLevel.IsLevelComplete();
    }

    public void ChangeTagMaskToActive()
    {
        gameObject.tag = "Enemy";
        gameObject.layer = 10;

        ragdoll.SetTagMask(gameObject.tag, 0);
    }

    public void ChangeTagMaskToNonActive()
    {
        gameObject.tag = "Untagged";
        gameObject.layer = 0;

        ragdoll.SetTagMask(gameObject.tag, gameObject.layer);
    }

    public void GetHit(float dmg, float forceOfFORCEAWAY = 0f, Vector3 missileDirectionOfFORCEAWAY = new Vector3())
    {
        if(Died) return;

        Health -= dmg;

        if(Health < 0f)
        {
            Death();
            ragdoll.FORCEAWAY(forceOfFORCEAWAY, missileDirectionOfFORCEAWAY);
        }
    }

    public void Death()
    {
        if(Died) return;

        Died = true;
        ResetTarget();
        SetMove(0);
        /* animator.enabled = false; */
        OffAnimation();

        ChangeTagMaskToNonActive();

        DeathEvent.Invoke();

        LevelManager.Instance.ActualLevel.IsLevelComplete();

        DelayToOff();
    }

    public void Heal(float hp)
    {
        Health += hp;
        if(Health > MaxHealth) Health = MaxHealth;
    }

    public void Restore()
    {
        Health = MaxHealth;
    }

    async void DelayToOff()
    {
        await UniTask.Delay(8000);
        if(GameManager.Instance.isActive) Off();
    }
}
