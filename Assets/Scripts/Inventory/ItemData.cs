
using UnityEngine;

public enum ItemType { Resource, Consumable, Animal }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string ItemName;
    public ItemType Type;
    public Sprite Icon;
    public int MaxStack = 1;
}
