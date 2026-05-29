using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BasePlayer
{
    [SerializeField] private Image healthBarFill;
    public float health;
    public float maxHealth;
    void Start()
    {
        maxHealth = 100;
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