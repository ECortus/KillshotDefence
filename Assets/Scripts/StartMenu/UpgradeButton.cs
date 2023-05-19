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

    [Space]
    [SerializeField] private List<Image> levelsImages = new List<Image>();
    [SerializeField] private Sprite buyedLevel, availableLevel;
    [SerializeField] private TextMeshProUGUI costText;

    void Start()
    {
        RefreshLevelGrid();
        RefreshText();
    }

    public void OnButtonClick()
    {
        if(Statistics.Money > CostOfProgress)
        {
            weapon.UpLevel();
            RefreshLevelGrid();
            RefreshText();
        }
    }

    void RefreshText()
    {
        costText.text = $"{CostOfProgress}";
    }

   void RefreshLevelGrid()
    {
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
