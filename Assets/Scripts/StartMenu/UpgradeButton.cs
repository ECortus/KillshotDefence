using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private int GameLevelToUnlock => weapon.GameLevelToUnlock;
    private int Level => weapon.Level;
    private int MaxLevel => weapon.MaxLevel;
    private int CostOfProgress => weapon.CostOfProgress;
    private int CostOfBuy => weapon.CostOfBuy;

    private bool isChoised => WeaponsInfoController.Instance.RequireToArm.Contains(weapon.Name);

    [Space]
    [SerializeField] private List<Image> levelsImages = new List<Image>();
    [SerializeField] private Sprite buyedLevel, availableLevel;
    [SerializeField] private TextMeshProUGUI costText, buyText, lockText;

    [Space]
    [SerializeField] private Image mainImage;
    [SerializeField] private TextMeshProUGUI armedText;
    [SerializeField] private Sprite choisedSprite, unChoisedSprite;

    [Space]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite buttonAvailable, buttonUnavailable;

    [Space]
    [SerializeField] private GameObject locked;
    [SerializeField] private GameObject buying;
    [SerializeField] private GameObject upgrading;

    /* void OnEnable()
    {
        Refresh();
    } */

    void UpdateButton()
    {
        if(Statistics.Money < CostOfProgress || Level == MaxLevel)
        {
            buttonImage.sprite = buttonUnavailable;
        }
        else
        {
            buttonImage.sprite = buttonAvailable;
        }
    }

    public void Refresh()
    {
        if(Level == -1 && GameLevelToUnlock > LevelManager.Instance.GetRealIndex())
        {
            OnLockedCanvas();
        }
        else if(Level == -1)
        {
            OnBuyingCanvas();
        }
        else
        {
            OnUpgradingCanvas();
        }

        UpdateButton();
        RefreshSprite();
        RefreshLevelGrid();
        RefreshText();
    }

    void OnBuyingCanvas()
    {
        buying.SetActive(true);

        upgrading.SetActive(false);
        locked.SetActive(false);
    }

    void OnUpgradingCanvas()
    {
        upgrading.SetActive(true);

        buying.SetActive(false);
        locked.SetActive(false);
    }

    void OnLockedCanvas()
    {
        locked.SetActive(true);

        upgrading.SetActive(false);
        buying.SetActive(false);
    }

    public void Buy()
    {
        if(Statistics.Money >= CostOfBuy && GameLevelToUnlock <= LevelManager.Instance.GetRealIndex() 
            && Level == -1)
        {
            Money.Minus(CostOfBuy);
            weapon.UpLevel();

            if(WeaponsInfoController.Instance.RequireToArm.Count < 4) 
                WeaponsInfoController.Instance.AddRequireWeapon(weapon.Name);
            
            StartMenu.Instance.RefreshAllButtons();
        }
    }

    void RefreshSprite()
    {
        if(isChoised)
        {
            mainImage.sprite = choisedSprite;
            armedText.text = "Remove";
        }
        else 
        {
            mainImage.sprite = unChoisedSprite;
            armedText.text = "Take";
        }
    }

    public void SetArmedStatus()
    {
        if(Level < 0)
        {
            Buy();
            return;
        }

        if(isChoised && WeaponsInfoController.Instance.RequireToArm.Count < 2)
        {
            RefreshSprite();
            return;
        }

        if(!isChoised)
        {
            WeaponsInfoController.Instance.AddRequireWeapon(weapon.Name);
        }
        else
        {
            WeaponsInfoController.Instance.RemoveRequireWeapon(weapon.Name);
        }

        StartMenu.Instance.RefreshAllButtons();
        RefreshSprite();
    }

    public void Upgrade()
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

            StartMenu.Instance.RefreshAllButtons();

            if(!Tutorial.Instance.Complete && Tutorial.Instance.State == TutorialState.UPGRADE)
            {
                Tutorial.Instance.SetState(TutorialState.NONE);
                Tutorial.Instance.Complete = true;
            }
        }
    }

    void RefreshText()
    {
        lockText.text = $"Lvl {weapon.GameLevelToUnlock + 1}";
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
