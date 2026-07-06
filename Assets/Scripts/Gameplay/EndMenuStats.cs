using TMPro;
using UnityEngine;

public class EndMenuStats : MonoBehaviour
{
    [Header("Run Stats")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI currencyText;

    [Header("Upgrade Slots")]
    public TextMeshProUGUI upgrade1;
    public TextMeshProUGUI upgrade2;
    public TextMeshProUGUI upgrade3;
    public TextMeshProUGUI upgrade4;

    private void Start()
    {
        DataCarrying data = DataCarrying.Instance;

        waveText.text = $"Survived up to Wave {data.currentWave}";
        timeText.text = $"Survived for {FormatTime(data.currentTime)}";
        killsText.text = $"Killed {data.kills} marine life";
        expText.text = $"Gained {Mathf.RoundToInt(data.totalEXPGained)} EXP";
        currencyText.text = $"Collected {data.totalCurrencyGained} coins";

        TextMeshProUGUI[] slots =
        {
            upgrade1,
            upgrade2,
            upgrade3,
            upgrade4
        };

        int index = 0;

        foreach (var upgrade in data.cappedUpgradeChoices)
        {
            if (index >= slots.Length)
                break;

            slots[index].text =
                $"{upgrade.Key}\nLvl {upgrade.Value}";

            index++;
        }

        while (index < slots.Length)
        {
            slots[index].text = "";
            index++;
        }
    }

    private string FormatTime(float time)
    {
        int seconds = Mathf.FloorToInt(time);

        int minutes = seconds / 60;
        seconds %= 60;

        if (minutes == 0)
            return $"{seconds} second{(seconds == 1 ? "" : "s")}";

        if (seconds == 0)
            return $"{minutes} minute{(minutes == 1 ? "" : "s")}";

        return $"{minutes} minute{(minutes == 1 ? "" : "s")} {seconds} second{(seconds == 1 ? "" : "s")}";
    }
}