using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;
    void Awake() => Instance = this;

    private List<ParticleSystem> BulletEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> RocketEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> BuckshotEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> GrenadeEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> HitEnemyEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> DeathZombieEffectPool = new List<ParticleSystem>();

    public GameObject InsertAmmoEffect(ParticleType type, GameObject obj, Vector3 pos)
    {
        List<ParticleSystem> list = new List<ParticleSystem>();

        switch(type)
        {
            case ParticleType.BulletEffect:
                list = BulletEffectPool;
                break;
            case ParticleType.RocketEffect:
                list = RocketEffectPool;
                break;
            case ParticleType.BuckshotEffect:
                list = BuckshotEffectPool;
                break;
            case ParticleType.GrenadeEffect:
                list = GrenadeEffectPool;
                break;
            default:
                return null;
        }

        GameObject go = Insert(ref list, obj, pos);

        switch(type)
        {
            case ParticleType.BulletEffect:
                BulletEffectPool = list;
                break;
            case ParticleType.RocketEffect:
                RocketEffectPool = list;
                break;
            case ParticleType.BuckshotEffect:
                BuckshotEffectPool = list;
                break;
            case ParticleType.GrenadeEffect:
                GrenadeEffectPool = list;
                break;
            default:
                return null;
        }

        return go;
    }

    public GameObject InsertEnemyEffect(ParticleType type, GameObject obj, Vector3 pos)
    {
        List<ParticleSystem> list = new List<ParticleSystem>();

        switch(type)
        {
            case ParticleType.HitEnemyEffect:
                list = HitEnemyEffectPool;
                break;
            case ParticleType.DeathZombieEffect:
                list = DeathZombieEffectPool;
                break;
            default:
                return null;
        }

        GameObject go = Insert(ref list, obj, pos);

        switch(type)
        {
            case ParticleType.HitEnemyEffect:
                HitEnemyEffectPool = list;
                break;
            case ParticleType.DeathZombieEffect:
                DeathZombieEffectPool = list;
                break;
            default:
                return null;
        }

        return go;
    }

    GameObject Insert(ref List<ParticleSystem> list, GameObject obj, Vector3 pos)
    {
        foreach(ParticleSystem ps in list)
        {
            if(ps == null) continue;

            if(!ps.isPlaying) 
            {
                ps.transform.position = pos;
                ps.Play();
                return ps.gameObject;
            }
        }

        ParticleSystem scr = Instantiate(obj, pos, Quaternion.Euler(Vector3.zero)).GetComponent<ParticleSystem>();
        list.Add(scr);

        return scr.gameObject;
    }
}

[System.Serializable]
public enum ParticleType
{
    Default, BulletEffect, BuckshotEffect, GrenadeEffect, RocketEffect, HitEnemyEffect, DeathZombieEffect
}
