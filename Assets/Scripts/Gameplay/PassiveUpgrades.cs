using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Passive Upgrades/Item Data")]
public class PassiveUpgrades : ScriptableObject
{
    public string itemName;

    [TextArea(3, 10)]
    public string description;
}