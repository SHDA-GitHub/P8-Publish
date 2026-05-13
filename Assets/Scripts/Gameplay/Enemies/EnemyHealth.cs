using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class EnemyHealth : BaseEnemy
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private LookAtConstraint lookAtConstraint;
    private Camera _mainCam;
    public float health;
    [SerializeField] private float maxHealth;
    void Start()
    {
        health = maxHealth;
        _mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        lookAtConstraint.AddSource(new ConstraintSource { sourceTransform = _mainCam.gameObject.transform, weight = 1 });
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