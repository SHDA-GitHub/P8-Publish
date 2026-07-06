using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{
    private DataCarrying data;

    [Header("EXP")]
    [SerializeField] private Image EXPBarFill;
    public float EXP;
    public float EXPIncrease;
    [SerializeField] private float maxEXP = 100;
    [SerializeField] private float exponentialEXP = 1.5f;

    public static Action OnLevelUp;

    private void Start()
    {
        EXP = 0;
        EXPIncrease = 0;

        data = FindFirstObjectByType<DataCarrying>();
    }

    private void Update()
    {
        if (EXP >= maxEXP)
        {
            LevelUp();
        }

        EXPBarFill.fillAmount = EXP / maxEXP;
    }

    public void AddEXP(float expReward)
    {
        float gained = expReward + EXPIncrease;

        EXP += gained;

        if (data != null)
            data.AddEXPGained(gained);
    }

    private void LevelUp()
    {
        EXP -= maxEXP;
        maxEXP *= exponentialEXP;
        OnLevelUp?.Invoke();
    }
}