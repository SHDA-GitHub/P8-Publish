using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BasePlayer
{
    [SerializeField] private Image healthBarFill;
    public float health;
    public float maxHealth;
    public float regenAmount = 0;

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

        if (health < maxHealth)
        {
            health += regenAmount * Time.deltaTime;
            health = Mathf.Min(health, maxHealth);
        }

        healthBarFill.fillAmount = health / maxHealth;
    }

    protected override void Death()
    {
        base.Death();
    }
}