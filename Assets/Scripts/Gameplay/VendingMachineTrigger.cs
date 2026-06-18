using UnityEngine;

public class VendingMachineTrigger : MonoBehaviour
{
    [SerializeField] private UpgradeVendingMachine vendingMachine;
    [SerializeField] private Player player;

    private bool playerInside;
    private bool previousShootState;
    private bool previousSlashState;

    private void Awake()
    {
        vendingMachine = FindFirstObjectByType<UpgradeVendingMachine>();
        player = FindFirstObjectByType<Player>();
    }
    private void Update()
    {
        if (playerInside && player.isShooting && !previousShootState)
        {
            vendingMachine.PurchaseUpgrade();
        }

        if (playerInside && player.slashActive && !previousSlashState)
        {
            vendingMachine.PurchaseUpgrade();
        }

        previousShootState = player.isShooting;
        previousSlashState = player.slashActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}