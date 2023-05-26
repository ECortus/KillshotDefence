using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusUI : MonoBehaviour
{
    [SerializeField] private LevelBonuses main;
    [SerializeField] private List<BonusButtonUI> bonusesUI = new List<BonusButtonUI>();
    [SerializeField] private GameObject continueButton;

    [Space]
    [SerializeField] private Sprite choise;
    [SerializeField] private Sprite unchoise;

    void Update()
    {
        if(main.isChoised)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void RefreshButtonsChoise(BonusButtonUI button)
    {
        for(int i = 0; i < bonusesUI.Count; i++)
        {
            if(bonusesUI[i] == button) bonusesUI[i].Image.sprite = choise;
            else bonusesUI[i].Image.sprite = unchoise;
        }
    }

    public void RefreshButtons()
    {
        for(int i = 0; i < bonusesUI.Count; i++)
        {
            if(bonusesUI[i].ContainsInMain()) bonusesUI[i].gameObject.SetActive(true);
            else bonusesUI[i].gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        for(int i = 0; i < bonusesUI.Count; i++)
        {
            bonusesUI[i].Image.sprite = unchoise;
        }
    }
}
