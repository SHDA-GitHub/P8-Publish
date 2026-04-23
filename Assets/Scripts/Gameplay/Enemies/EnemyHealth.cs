using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : BaseEnemy
{
    [SerializeField] private Image healthBarFill;

    public ushort health;
    [SerializeField] private ushort maxHealth;
    void Start()
    {
        health = maxHealth;
    }

    protected override void Update()
    {
        healthBarFill.fillAmount = health / maxHealth;
    }

    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
}