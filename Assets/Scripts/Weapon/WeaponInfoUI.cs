using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class WeaponInfoUI : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Shooting shooting;
    private int Level => weapon.Level;
    private int MaxLevel => weapon.MaxLevel;
    private int AmmoAmount => weapon.AmmoAmount;
    private float ReloadTime => weapon.ReloadTime;

    [Space]
    [SerializeField] private List<Image> levelsImages = new List<Image>();
    [SerializeField] private Sprite buyedLevel, availableLevel;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Slider reloadSlider;

    public void OnButtonClick()
    {
        shooting.SetWeapon(weapon.Name);
    }

    public void On()
    {
        gameObject.SetActive(true);

        weapon.FullAmmo();
        RefreshText();
        RefreshLevelGrid();
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void RefreshText()
    {
        ammoText.text = $"{AmmoAmount}";
    }

    public void RefreshLevelGrid()
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

    public async UniTask Reloading()
    {
        reloadSlider.minValue = 0f;
        reloadSlider.maxValue = ReloadTime;

        reloadSlider.value = 0f;

        float time = Time.deltaTime;

        while(reloadSlider.value < reloadSlider.maxValue)
        {
            reloadSlider.value += time;
            await UniTask.Delay((int)(time * 1000));
        }
    }
}
