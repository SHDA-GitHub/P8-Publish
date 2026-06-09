using TMPro;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI killsText;
    public int currency;
    public int currencyIncrease;
    public int kills = 0;

    void Start()
    {
        currency = 0;
        currencyIncrease = 0;
        kills = 0;
    }

    public void AddEXP(int currencyReward)
    {
        currency = currency + currencyReward * currencyIncrease;
    }

    void Update()
    {
        currencyText.text = currency.ToString();
        killsText.text = kills.ToString();
    }
}
