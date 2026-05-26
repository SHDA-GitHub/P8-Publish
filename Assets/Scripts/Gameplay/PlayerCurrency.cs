using TMPro;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI killsText;
    public int currency = 0;
    public int kills = 0;

    void Start()
    {
        currency = 0;
        kills = 0;
    }

    void Update()
    {
        currencyText.text = currency.ToString();
        killsText.text = kills.ToString();
    }
}
