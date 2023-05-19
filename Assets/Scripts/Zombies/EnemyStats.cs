using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{
    public float DefaultMaxHealth = 10f;
    public float DefaultDamage = 1f;
    public UnityEvent DeathEvent;

    [Space]
    [SerializeField] private ParticleType effectType = ParticleType.DeathZombieEffect;
    [SerializeField] private GameObject effect;

    public void AddEffect()
    {
        ParticlePool.Instance.InsertEnemyEffect(effectType, effect, transform.position + Vector3.up * 1f);
    }
}
