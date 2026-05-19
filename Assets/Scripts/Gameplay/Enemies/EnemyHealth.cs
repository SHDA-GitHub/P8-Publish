using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class EnemyHealth : BaseEnemy
{
    [SerializeField] private Image healthBarFill;
    private Camera _mainCam;
    private PlayerEXP playerEXP;
    public float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float EXPReward;
    void Start()
    {
        health = maxHealth;
        playerEXP = FindFirstObjectByType<PlayerEXP>();
    }
    private void Update()
    {
        if (health <= 0)
        {
            Death();
        }
        healthBarFill.fillAmount = health / maxHealth;
        
    }

    protected override void Death()
    {
        playerEXP.EXP += EXPReward;
        Debug.Log("Enemy Dies");
        base.Death();
    }
}