using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private int Level => weapon.Level;
    private int MaxLevel => weapon.MaxLevel;
    private int CostOfProgress => weapon.CostOfProgress;
    private int CostOfBuy => weapon.CostOfBuy;

    [Space]
    [SerializeField] private List<Image> levelsImages = new List<Image>();
    [SerializeField] private Sprite buyedLevel, availableLevel;
    [SerializeField] private TextMeshProUGUI costText, buyText;

    [Space]
    [SerializeField] private GameObject buying;
    [SerializeField] private GameObject upgrading;

    void OnEnable()
    {
        if(Level == -1)
        {
            OnBuyingCanvas();
        }
        else
        {
            OffBuyingCanvas();
        }

        RefreshLevelGrid();
        RefreshText();
    }

    void OnBuyingCanvas()
    {
        buying.SetActive(true);
        upgrading.SetActive(false);
    }

    void OffBuyingCanvas()
    {
        buying.SetActive(false);
        upgrading.SetActive(true);
    }

    public void Buy()
    {
        if(Statistics.Money >= CostOfBuy)
        {
            Money.Minus(CostOfBuy);
            weapon.UpLevel();

            RefreshLevelGrid();
            RefreshText();

            OffBuyingCanvas();

            GameManager.Instance.weaponsInfo.AddRequireWeapon(weapon.Name);
        }
    }

    public void OnButtonClick()
    {
        if(Level == MaxLevel) return;

        if(Level == -1)
        {
            Buy();
            return;
        }

        if(Statistics.Money >= CostOfProgress)
        {
            Money.Minus(CostOfProgress);
            weapon.UpLevel();

            RefreshLevelGrid();
            RefreshText();
        }
    }

    void RefreshText()
    {
        buyText.text = $"{CostOfBuy}";
        
        if(Level == MaxLevel)
        {
            costText.text = $"---";
        }
        else
        {
            costText.text = $"{CostOfProgress}";
        }
    }

    void RefreshLevelGrid()
    {
        if(Level == -1)
        {
            /* for(int i = 0; i < MaxLevel; i++)
            {
                levelsImages[i].sprite = availableLevel;
            } */
            return;
        }

        Sprite sprite;
        for(int i = 0; i < MaxLevel; i++)
        {
            levelsImages[i].gameObject.SetActive(true);

            if(i < Level) sprite = buyedLevel;
            else sprite = availableLevel;

            levelsImages[i].sprite = sprite;
        }
    }
}
