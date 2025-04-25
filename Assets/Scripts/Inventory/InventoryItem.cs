
using UnityEngine;

public enum AnimalState { Healthy, Wounded }

[System.Serializable]
public class InventoryItem
{
    public ItemData Data;
    public int Count;
    public AnimalState State;

    public InventoryItem(ItemData data, int count = 1, AnimalState state = AnimalState.Healthy)
    {
        Data = data;
        Count = count;
        State = state;
    }
}
