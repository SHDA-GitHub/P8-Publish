using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{
    [SerializeField] private Image EXPBarFill;
    public float EXP = 0;
    [SerializeField] private float maxEXP;
    [SerializeField] private float exponentialEXP;
    void Start()
    {
        EXP = 0;
    }
    private void Update()
    {
        if (EXP == maxEXP)
        {
            maxEXP = maxEXP + exponentialEXP;
            EXP = 0;
        }
        EXPBarFill.fillAmount = EXP / maxEXP;
    }
}