using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Weapon : MonoBehaviour
{
    public bool isReloading { get; set; }
    public virtual string Name { get; }
    public virtual ShootingType ShootType { get; }
    protected virtual string PrefsKey { get; }
    protected virtual ObjectType AmmoType { get; }

    public int MaxLevel = 5;

    public int Level
    {
        get
        {
            return PlayerPrefs.GetInt(PrefsKey, 0);
        }
        set
        {
            PlayerPrefs.SetInt(PrefsKey, value);
            PlayerPrefs.Save();
        }
    }

    public void UpLevel()
    {
        if(Level == MaxLevel) return;

        Money.Minus(CostOfProgress);

        int value = Level + 1;
        Level = value;

        infoUI.RefreshLevelGrid();
        AmmoAmount = MaxAmmoAmount;
    }

    [Header("Weapon par-s:")]
    [SerializeField] private float defaultDamage;
    [SerializeField] private float damageUpPerLevel;
    public float DamageUpPerLevel { get { return damageUpPerLevel * (1f + Level / 10f); } }
    public float Damage { get { return defaultDamage + DamageUpPerLevel * Level; } }

    [Space]
    [SerializeField] private float defaultReloadTime;
    [SerializeField] private float reloadTimeDownPerLevel;
    public float ReloadTimeDownPerLevel { get { return reloadTimeDownPerLevel * (1f + Level / 10f); } }
    public float ReloadTime { get { return defaultReloadTime + ReloadTimeDownPerLevel * Level; } }

    [Space]
    [SerializeField] private int defaultAmmoAmount;
    [SerializeField] private float ammoAmountUpPerLevel;
    public int AmmoAmountUpPerLevel { get { return (int)(ammoAmountUpPerLevel * (1f + Level / 10f)); } }
    private int _ammoAmount;
    public int AmmoAmount
    {
        get
        {
            return _ammoAmount;
        }
        set
        {
            _ammoAmount = value;
            infoUI.RefreshText();
        }
    }
    public int MaxAmmoAmount { get { return defaultAmmoAmount + AmmoAmountUpPerLevel * Level; } }
    public void FullAmmo()
    {
        AmmoAmount = MaxAmmoAmount;
    }

    [Space]
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

    public virtual void Shot() { }

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
