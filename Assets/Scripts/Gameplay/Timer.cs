using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Starting Time")]
    public int startMinutes = 0;
    public int startSeconds = 0;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    private float currentTime;

    void Start()
    {
        currentTime = (startMinutes * 60) + startSeconds;

        UpdateTimerDisplay();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}