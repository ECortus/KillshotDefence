using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float defaultMaxHealth = 100f;
    private float bonus
    {
        get
        {
            return PlayerPrefs.GetFloat("HealthBonus", 0f);
        }
        set
        {
            PlayerPrefs.SetFloat("HealthBonus", value);
            PlayerPrefs.Save();
        }
    }
    public float MaxHealth
    {
        get
        {
            return defaultMaxHealth + bonus;
        }
    }

    private float _Health;
    public float Health
    {
        get
        {
            return _Health;
        }
        set
        {
            _Health = value;
            healthUI.Refresh();
        }
    }

    [Space]
    [SerializeField] private PlayerHealthUI healthUI;

    [Space]
    [SerializeField] private UnityEvent death;
    bool Died = false;

    /* void Start()
    {
        Restore();
    }
 */
    public void AddBonus(float amnt)
    {
        bonus += amnt;
        Restore();
    }

    public void ResetBonus()
    {
        bonus = 0;
    }

    public void GetHit(float dmg)
    {
        if(Died) return;

        Health -= dmg;
        FenceShake.Instance.On(0.25f);

        if(Health < 0.1f)
        {
            Death();
        }
    }

    public void Death()
    {
        Died = true;
        death.Invoke();
    }

    public void Heal(float hp)
    {
        Health += hp;
        if(Health > MaxHealth) Health = MaxHealth;
    }

    public void Restore()
    {
        Health = MaxHealth;
        Died = false;
    }

    /* void OnCollisionEnter(Collision col) */
    void OnTriggerEnter(Collider col)
    {
        GameObject go = col.gameObject;

        if(go.tag == "Enemy")
        {
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.StartAttack(this);
            /* GetHit(enemy.Damage);
            enemy.Off(); */
        }
    }
}
