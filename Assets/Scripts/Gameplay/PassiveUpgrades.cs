using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Passive Upgrades/Item Data")]
public class PassiveUpgrades : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public int effectAmountInt;
    public float effectAmountFloat;
    public bool hasMaxCap = false;

    [Header("Vending Machine")]
    public bool VendingOneRound = true;

    [TextArea(3, 10)]
    public string description;
}