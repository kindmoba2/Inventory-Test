
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventoryItem> Items = new();
    public Action OnInventoryChanged;

    private const string SaveKey = "InventoryData";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        {
            var allItems = Resources.LoadAll<ItemData>("Items").ToList();
            LoadInventory(allItems);
        }
    }

    public void AddItem(ItemData data, AnimalState state = AnimalState.Healthy)
    {
        if (data.Type == ItemType.Animal)
        {
            var match = Items.Find(i => i.Data.ID == data.ID && i.State == state && i.Count < i.Data.MaxStack);
            if (match != null) match.Count++;
            else Items.Add(new InventoryItem(data, 1, state));
        }
        else
        {
            var match = Items.Find(i => i.Data.ID == data.ID && i.Count < i.Data.MaxStack);
            if (match != null) match.Count++;
            else Items.Add(new InventoryItem(data));
        }

        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData data)
    {
        var match = Items.Find(i => i.Data.ID == data.ID);
        if (match != null)
        {
            match.Count--;
            if (match.Count <= 0) Items.Remove(match);
        }
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData data, AnimalState? state = null)
    {
        var match = Items.Find(i => i.Data.ID == data.ID && i.State == state);
        if (match != null)
        {
            match.Count--;
            if (match.Count <= 0) Items.Remove(match);
        }
        OnInventoryChanged?.Invoke();
    }

    public void ToggleAnimalState(ItemData data)
    {
        if (data.Type != ItemType.Animal)
        {
            print("неприменимо");
            return;
        }

        var item = Items.Find(i => i.Data.ID == data.ID);
        if (item == null) return;

        var newState = item.State == AnimalState.Healthy ? AnimalState.Wounded : AnimalState.Healthy;
        item.State = newState;
        OnInventoryChanged?.Invoke();
    }

    public void SwapItems(InventorySlot a, InventorySlot b)
    {
        int indexA = a.transform.GetSiblingIndex();
        int indexB = b.transform.GetSiblingIndex();

        if (indexA < Items.Count && indexB < Items.Count)
        {
            (Items[indexA], Items[indexB]) = (Items[indexB], Items[indexA]);
            OnInventoryChanged?.Invoke();
        }
    }

    public void SaveInventory()
    {
        var saveData = new InventorySaveData();
        foreach (var item in Items)
        {
            saveData.items.Add(new InventoryItemData
            {
                ID = item.Data.ID,
                Count = item.Count,
                State = item.State
            });
        }
        var json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadInventory(List<ItemData> allItemData)
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        var json = PlayerPrefs.GetString(SaveKey);
        var saveData = JsonUtility.FromJson<InventorySaveData>(json);

        Items.Clear();
        foreach (var itemData in saveData.items)
        {
            var itemRef = allItemData.Find(i => i.ID == itemData.ID);
            if (itemRef != null)
            {
                Items.Add(new InventoryItem(itemRef, itemData.Count, itemData.State));
            }
        }
        OnInventoryChanged?.Invoke();
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventoryItemData> items = new();
}

[System.Serializable]
public class InventoryItemData
{
    public int ID;
    public int Count;
    public AnimalState State;
}
