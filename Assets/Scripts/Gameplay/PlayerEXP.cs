using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{
    [Header("EXP")]
    [SerializeField] private Image EXPBarFill;
    public float EXP = 0;
    [SerializeField] private float maxEXP = 100;
    [SerializeField] private float exponentialEXP = 50;

    public static Action OnLevelUp;

    private void Start()
    {
        EXP = 0;
    }

    private void Update()
    {
        if (EXP >= maxEXP)
        {
            LevelUp();
        }

        EXPBarFill.fillAmount = EXP / maxEXP;
    }

    private void LevelUp()
    {
        EXP -= maxEXP;
        maxEXP += exponentialEXP;
        OnLevelUp?.Invoke();
    }
}