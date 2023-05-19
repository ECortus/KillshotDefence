using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float defaultMaxHealth = 100f;
    public float MaxHealth
    {
        get
        {
            return defaultMaxHealth;
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
    public void GetHit(float dmg)
    {
        if(Died) return;

        Health -= dmg;

        if(Health < 0f)
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

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        if(go.tag == "Enemy")
        {
            Enemy enemy = go.GetComponent<Enemy>();
            GetHit(enemy.Damage);
            enemy.Off();
        }
    }
}
