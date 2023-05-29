using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BonusSaving
{
    /* public static List<Bonus> UsedBonused = new List<Bonus>();

    public static void AddUsedBonus(Bonus bonus)
    {
        UsedBonused.Add(bonus);
    }

    public static void CancelAllBonuses()
    {
        foreach(Bonus bonus in UsedBonused)
        {   
            bonus.Cancel();
        }
    } */

    public static List<Bonus> Bonuses
    {
        get
        {
            if(LevelManager.Instance.ActualLevel.bonuses == null)
            {
                return new List<Bonus>();
            }
            else
            {
                return LevelManager.Instance.ActualLevel.bonuses.AllBonuses;
            }
        }
    } 

    public static void CancelAllBonuses()
    {
        if(Bonuses.Count == 0) return;
        
        foreach(Bonus bonus in Bonuses)
        {   
            bonus.Cancel();
        }
    }
}
