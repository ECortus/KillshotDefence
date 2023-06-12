using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Weapon : MonoBehaviour
{
    public bool isReloading { get; set; }
    public virtual string Name { get; }
    protected virtual int DefaultLevel { get; }
    public virtual ShootingType ShootType { get; }
    protected virtual string PrefsKey { get; }
    protected virtual ObjectType AmmoType { get; }

    private string FalseUpgradeKey => PrefsKey + "FalseUpgrade";

    public int MaxLevel = 5;
    public int GameLevelToUnlock = -1;

    public int Level
    {
        get
        {
            return PlayerPrefs.GetInt(PrefsKey, DefaultLevel);
        }
        set
        {
            PlayerPrefs.SetInt(PrefsKey, value);
            PlayerPrefs.Save();
        }
    }

    public int FalseUpgrades 
    {
        get
        {
            return PlayerPrefs.GetInt(FalseUpgradeKey, 0);
        }
        set
        {
            PlayerPrefs.SetInt(FalseUpgradeKey, value);
            PlayerPrefs.Save();
        }
    }

    public void UpLevel()
    {
        if(Level == MaxLevel) return;

        int value = Level + 1;
        Level = value;

        AmmoAmount = MaxAmmoAmount;
        infoUI.RefreshLevelGrid();
    }

    public void DownLevel()
    {
        if(Level <= 0 || FalseUpgrades == 0) return;

        int value;
        for(int i = 0; i < FalseUpgrades; i++)
        {
            if(Level == 0) break;

            value = Level - 1;
            Level = value;

            AmmoAmount = MaxAmmoAmount;
            infoUI.RefreshLevelGrid();
        }
        FalseUpgrades = 0;
    }

    [Header("Weapon par-s:")]
    [SerializeField] private float defaultDamage;
    [SerializeField] private float damageUpPerLevel;
    private float DamageBonus
    {
        get
        {
            return PlayerPrefs.GetFloat($"{PrefsKey}_CD", 0f);
        }
        set
        {
            PlayerPrefs.SetFloat($"{PrefsKey}_CD", value);
            PlayerPrefs.Save();
        }
    }
    public void SetDamageBonus(float value) => DamageBonus += value;
    public void ResetDamageBonus() => DamageBonus = 0f;
    public float DamageUpPerLevel { get { return damageUpPerLevel * (1f + Level / 10f); } }
    public float Damage { get { return defaultDamage + DamageUpPerLevel * Level + DamageBonus; } }

    [Space]
    public float FORCEAWAY = 1000f;

    [Space]
    [SerializeField] private float defaultReloadTime;
    [SerializeField] private float reloadTimeDownPerLevel;
    private int CoolDownBonus
    {
        get
        {
            return PlayerPrefs.GetInt($"{PrefsKey}_CD", 100);
        }
        set
        {
            PlayerPrefs.SetInt($"{PrefsKey}_CD", value);
            PlayerPrefs.Save();
        }
    }
    public void SetCDBonus(int perc) => CoolDownBonus += (int)(CoolDownBonus * (perc / 100f));
    public void ResetCDBonus() => CoolDownBonus = 100;
    public float ReloadTimeDownPerLevel { get { return reloadTimeDownPerLevel * (1f + Level / 10f); } }
    public float ReloadTime { get { return (defaultReloadTime + ReloadTimeDownPerLevel * Level) * CoolDownBonus / 100f; } }

    [Space]
    [SerializeField] private int defaultAmmoAmount;
    [SerializeField] private float ammoAmountUpPerLevel;
    private int AmmoBonus = 0;
    public void SetAmmoBonus(int value)
    {
        AmmoBonus += value;
        AmmoAmount = MaxAmmoAmount;
    }
    public void ResetAmmoBonus() => AmmoBonus = 0;
    public int AmmoAmountUpPerLevel { get { return (int)(ammoAmountUpPerLevel * (1f + Level / 10f)); } }
    public int AmmoAmount
    {
        get
        {
            return PlayerPrefs.GetInt($"{PrefsKey}_Ammo", 0);
        }
        set
        {
            PlayerPrefs.SetInt($"{PrefsKey}_Ammo", value);
            PlayerPrefs.Save();
            infoUI.RefreshText();
        }
    }
    public int MaxAmmoAmount { get { return defaultAmmoAmount + AmmoAmountUpPerLevel * Level + AmmoBonus; } }
    public void FullAmmo()
    {
        AmmoAmount = MaxAmmoAmount;
    }

    public void ResetBonuses()
    {
        ResetDamageBonus();
        ResetCDBonus();
        ResetAmmoBonus();
    }

    [Space]
    public int CostOfBuy = 100;
    [SerializeField] private int defaultCostOfProgress;
    [SerializeField] private int costUpPerLevel;
    public int CostUpPerLevel { get { return (int)(costUpPerLevel * (1f + Level / 10f)); } }
    public int CostOfProgress { get { return defaultCostOfProgress + CostUpPerLevel * Level; } }

    [Space]
    [SerializeField] private float DelayBetweenShots = 0.1f;

    [Header("Weapon ref-s: ")]
    public GameObject ammo;
    public Transform muzzle;
    public Vector3 Direction => muzzle.forward;

    [Space]
    [SerializeField] private WeaponInfoUI infoUI;
    public WeaponAnimation anim;

    /* void Start()
    {
        FullAmmo();
    } */

    public void On()
    {
        this.enabled = true;
        gameObject.SetActive(true);
    }

    public void Off()
    {
        this.enabled = false;
        gameObject.SetActive(false);
    }

    public virtual void Shot() { if(anim != null) anim.Play(); }

    public async UniTask ReduceAmmo()
    {
        AmmoAmount--;

        if(AmmoAmount == 0)
        {
            await Reload();
        }
        else
        {
            await UniTask.Delay((int)(DelayBetweenShots * 1000));
        }
    }

    public async UniTask Reload()
    {
        isReloading = true;

        await infoUI.Reloading();
        FullAmmo();

        isReloading = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(muzzle.position, muzzle.position + muzzle.forward * 100f);
    }
}
