using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{
    [Header("EXP")]
    [SerializeField] private Image EXPBarFill;
    public float EXP;
    public float EXPIncrease;
    [SerializeField] private float maxEXP = 100;
    [SerializeField] private float exponentialEXP = 50;

    public static Action OnLevelUp;

    private void Start()
    {
        EXP = 0;
        EXPIncrease = 0;
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
        EXP = EXP + expReward + EXPIncrease;
    }

    private void LevelUp()
    {
        EXP -= maxEXP;
        maxEXP += exponentialEXP;
        OnLevelUp?.Invoke();
    }
}