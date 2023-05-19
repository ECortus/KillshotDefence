using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public static MoneyUI Instance { get; set; }

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private float counterPlusBySecond = 0.1f;

    private static int money { get { return Statistics.Money; } set { Statistics.Money = value; } }
    int currentMoneyCount = 0;
    [SerializeField] private int bound = 3;

    Coroutine coroutine;

    void Awake() => Instance = this;

    void Start()
    {
        Money.Load();
        ResetMoney();
    }

    public void UpdateMoney()
    {
        if(!gameObject.activeInHierarchy)
        {
            ResetMoney();
            return;
        }

        if(coroutine == null && currentMoneyCount != money) coroutine = StartCoroutine(Coroutine());
    }

    public void ResetMoney()
    {
        currentMoneyCount = money;
        IntoText(currentMoneyCount);
    }

    IEnumerator Coroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.014f);

        while(currentMoneyCount != money)
        {
            currentMoneyCount = (int)Mathf.Lerp(currentMoneyCount, money, counterPlusBySecond * Time.deltaTime);
            if (Mathf.Abs(currentMoneyCount - money) <= bound) break;

            IntoText(currentMoneyCount);

            yield return wait;
        }

        ResetMoney();

        StopCoroutine(coroutine);
        coroutine = null;
    }

    void IntoText(int value)
    {
        int money = value;
        string text = $"{money}";

        /* string text = "";
        int thousands = money / 1000;
        int hundreds = money % 1000;

        if(thousands == 0) text = $"{hundreds}";
        else
        {
            text = $"{thousands}." + $"{hundreds / 10}K";
        } */

        moneyText.text = text;
    }
}
