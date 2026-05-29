using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Passive Upgrades/Item Data")]
public class PassiveUpgrades : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public float effectAmount;

    [TextArea(3, 10)]
    public string description;
}