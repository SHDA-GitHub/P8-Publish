using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerHealth : BasePlayer
{
    [SerializeField] private Image healthBarFill;
    public float health;
    [SerializeField] private float maxHealth;
    void Start()
    {
        health = maxHealth;
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
        base.Death();
    }
}