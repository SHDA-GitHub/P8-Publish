using TMPro;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    private DataCarrying data;

    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI killsText;
    public int currency;
    public int currencyIncrease;
    public int kills = 0;

    void Start()
    {
        currency = 0;
        currencyIncrease = 1;
        kills = 0;

        data = FindFirstObjectByType<DataCarrying>();
    }

    public void AddCurrency(int currencyReward)
    {
        int gained = currencyReward * currencyIncrease;

        currency += gained;

        if (data != null)
            data.AddCurrencyGained(gained);
    }

    void Update()
    {
        currencyText.text = currency.ToString();
        killsText.text = kills.ToString();
    }
}
