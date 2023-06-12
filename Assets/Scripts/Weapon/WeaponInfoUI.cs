using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class WeaponInfoUI : MonoBehaviour
{
    public bool Active => gameObject.activeSelf;

    public Weapon Weapon;
    [SerializeField] private Shooting shooting;
    private int Level => Weapon.Level;
    private int MaxLevel => Weapon.MaxLevel;
    private int AmmoAmount => Weapon.AmmoAmount;
    private float ReloadTime => Weapon.ReloadTime;

    [Space]
    [SerializeField] private List<Image> levelsImages = new List<Image>();
    [SerializeField] private Sprite buyedLevel, availableLevel;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Slider reloadSlider;

    public void OnButtonClick()
    {
        shooting.SetWeapon(Weapon.Name);
        shooting.IsShooting = false;
    }

    public void On()
    {
        gameObject.SetActive(true);

        Weapon.FullAmmo();
        shooting.IsShooting = false;
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

    public void Reset()
    {
        reloadSlider.value = reloadSlider.maxValue;
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
