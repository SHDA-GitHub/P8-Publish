using UnityEngine;

public class Locator : MonoBehaviour
{
    [SerializeField] private TankEnemy enemyParent;

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;

        if (otherObject.CompareTag("Player"))
        {
            enemyParent.attacking = true;
        }
    }
}
