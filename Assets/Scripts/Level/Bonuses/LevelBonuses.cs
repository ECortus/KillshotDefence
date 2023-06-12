using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBonuses : MonoBehaviour
{
    public List<Bonus> AllBonuses = new List<Bonus>();
    [HideInInspector] public List<Bonus> Bonuses = new List<Bonus>();
    [SerializeField] private BonusUI ui;
    [SerializeField] private int RequiredCount;

    public bool isChoised => ChoisedBonus != null;
    [HideInInspector] public Bonus ChoisedBonus;

    public bool Active => gameObject.activeSelf;

    void FormList()
    {
        Bonuses.Clear();
        BonusType type;

        List<Bonus> list = new List<Bonus>();
        list.AddRange(AllBonuses);

        for(int i = 0; i < list.Count; i++)
        {
            type = list[i].Type;

            switch(type)
            {
                case BonusType.UpWeaponParameters:
                    Weapon weapon = WeaponsInfoController.Instance.GetWeapon($"{list[i].WeaponName}");
                    if(weapon.Level < 0 || !WeaponsInfoController.Instance.RequireToArm.Contains(weapon.Name))
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                    break;
                default:
                    break;
            }
        }

        int index = 0;
        for(int i = 0; i < RequiredCount; i++)
        {
            index = Random.Range(0, list.Count);
            if(Bonuses.Contains(list[index]))
            {
                continue;
            }
            else
            {
                Bonuses.Add(list[index]);
                list.Remove(list[index]);
            }
        }

        ChoisedBonus = null;
    }

    public void On()
    {
        gameObject.SetActive(true);

        FormList();

        ui.Reset();
        ui.RefreshButtons();

        GameManager.Instance.SetActive(false);
    }

    public void Off()
    {
        GameManager.Instance.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetBonus(Bonus bonus)
    {
        ChoisedBonus = bonus;
    }

    public void ContinueButtonFunc()
    {
        ApplyBonus();
        Off();
    }

    public void ApplyBonus()
    {
        ChoisedBonus.Apply();
        /* BonusSaving.AddUsedBonus(ChoisedBonus); */
    }

    public void RemoveBonus()
    {
        if(ChoisedBonus != null) ChoisedBonus.Cancel();
    }
}
