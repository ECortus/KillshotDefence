using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusButtonUI : MonoBehaviour
{
    [SerializeField] private LevelBonuses main;
    [SerializeField] private BonusUI ui;
    public Image Image;
    [SerializeField] private Bonus bonus;

    public bool ContainsInMain() => main.Bonuses.Contains(bonus);

    public void OnButtonClick()
    {
        main.SetBonus(bonus);
        ui.RefreshButtonsChoise(this);
    }
}
